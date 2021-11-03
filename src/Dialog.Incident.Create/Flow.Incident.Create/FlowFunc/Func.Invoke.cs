using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra.Bot.Builder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace GGroupp.Internal.Support.Bot;

partial class IncidentCreateFlowFunc
{
    public ValueTask<ChatFlowStepResult<Unit>> InvokeAsync(
        DialogContext dialogContext, IncidentCreateFlowIn input, CancellationToken cancellationToken = default)
        =>
        ChatFlow.Start(
            dialogContext ?? throw new ArgumentNullException(nameof(dialogContext)),
            input ?? throw new ArgumentNullException(nameof(input)))
        .SendActivity(
            dialogContext.Context.Activity.CreateConfirmationActivity)
        .Await()
        .ForwardValue(
            CheckResponseAsync)
        .MapFlowState(
            @in => new IncidentCreateIn(
                ownerId: @in.OwnerId,
                customerId: @in.CustomerId,
                title: @in.Title,
                description: @in.Description,
                caseTypeCode: @in.CaseTypeCode,
                caseOriginCode: flowConfiguration.CaseOriginCode))
        .ForwardValue(
            CreateIncidentAsync)
        .CompleteValueAsync(
            cancellationToken);

    private ValueTask<ChatFlowStepResult<IncidentCreateFlowIn>> CheckResponseAsync(
        DialogContext dialogContext, IncidentCreateFlowIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            dialogContext.Context.Activity, cancellationToken)
        .Pipe(
            activity => activity.IsConfirmed() ? Result.Present(input) : default)
        .MapFailure(
            async (failure, token) =>
            {
                var failureActivity = MessageFactory.Text("Создание инцидента отменено");
                await dialogContext.Context.SendActivityAsync(failureActivity, token).ConfigureAwait(false);

                return failure;
            })
        .Fold(
            ChatFlowStepResult.Next,
            _ => ChatFlowStepResult.Interrupt());

    private ValueTask<ChatFlowStepResult<Unit>> CreateIncidentAsync(
        DialogContext dialogContext, IncidentCreateIn incident, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            incident, cancellationToken)
        .PipeValue(
            incidentCreateFunc.InvokeAsync)
        .MapSuccess(
            incident => new IncidentLink(
                title: incident.Title,
                url: GetIncidentUrl(incident)))
        .Fold<ChatFlowStepResult<Unit>>(
            async (incidentLink, token) =>
            {
                var successActivity = IncidentCreateActivity.CreateSuccess(incidentLink);
                await dialogContext.Context.SendActivityAsync(successActivity, token).ConfigureAwait(false);

                return default(Unit);
            },
            async (failure, token) =>
            {
                logger.LogError(failure.FailureMessage);

                var failureActivity = MessageFactory.Text(UnexpectedFailureMessage);
                await dialogContext.Context.SendActivityAsync(failureActivity, token).ConfigureAwait(false);

                return ChatFlowStepResult.Interrupt();
            });

    private string GetIncidentUrl(IncidentCreateOut incident)
        =>
        string.Format(CultureInfo.InvariantCulture, flowConfiguration.IncidentCardUrlTemplate, incident.Id);

    private const string UnexpectedFailureMessage
        =
        "Не удалось создать инцидент. Возможно сервис не доступен. Обратитесь к администратору или повторите попытку позже";
}