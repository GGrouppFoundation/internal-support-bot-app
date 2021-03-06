using System.Collections.Generic;
using GGroupp.Infra.Bot.Builder;

namespace GGroupp.Internal.Support;

internal static class ConfirmIncidentFlowStep
{
    internal static ChatFlow<IncidentCreateFlowState> ConfirmIncident(this ChatFlow<IncidentCreateFlowState> chatFlow)
        =>
        chatFlow.AwaitConfirmation(CreateOption);

    private static ConfirmationCardOption CreateOption(IChatFlowContext<IncidentCreateFlowState> context)
        =>
        new(
            questionText: "Создать инцидент?",
            confirmButtonText: "Создать",
            cancelButtonText: "Отменить",
            cancelText: "Создание инцидента было отменено",
            fieldValues: new KeyValuePair<string, string?>[]
            {
                new("Заголовок", context.FlowState.Title),
                new("Клиент", context.FlowState.CustomerTitle),
                new("Контакт", context.FlowState.ContactFullName ?? "--"),
                new("Тип обращения", context.FlowState.CaseTypeTitle),
                new("Приоритет", context.FlowState.PriorityTitle),
                new("Описание", context.FlowState.Description)
            });
}