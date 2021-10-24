using System;
using GGroupp.Infra.Bot.Builder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using PrimeFuncPack;

namespace GGroupp.Internal.Support.Bot;

using IUserLogInFlowFunc = IAsyncValueFunc<DialogContext, Unit, ChatFlowStepResult<UserLogInFlowOut>>;
using IIncidentCreateFlowFunc = IAsyncValueFunc<DialogContext, IncidentCreateFlowIn, ChatFlowStepResult<Unit>>;
using IIncidentCustomerFindFlowFunc = IAsyncValueFunc<DialogContext, Unit, ChatFlowStepResult<IncidentCustomerFindFlowOut>>;
using IIncidentTitleGetFlowFunc = IAsyncValueFunc<DialogContext, IncidentTitleGetFlowIn, ChatFlowStepResult<IncidentTitleGetFlowOut>>;
using IIncidentTypeGetFlowFunc = IAsyncValueFunc<DialogContext, Unit, ChatFlowStepResult<IncidentTypeGetFlowOut>>;

internal static class BotDependencyFlow
{
    public static Dependency<IUserLogInFlowFunc> UseUserLogInFlow(this Dependency<UserState> userStateDependency)
        =>
        BotDependencyApi.UseADUserGetApi()
        .With(
            BotDependencyApi.UseUserGetApi())
        .With(
            BotServiceProvider.GetConfiguration<UserLogInConfiguration>)
        .With(
            userStateDependency)
        .With(
            BotServiceProvider.GetLoggerFactory)
        .UseUserLogInFlow();

    public static Dependency<IIncidentCreateFlowFunc> UseIncidentCreateFlow()
        =>
        BotDependencyApi.UseIncidentCreateApi()
        .With(
            BotServiceProvider.GetConfiguration<DataverseApiClientConfiguration>)
        .With(
            BotServiceProvider.GetLoggerFactory)
        .UseIncidentCreateFlow();

    public static Dependency<IIncidentCustomerFindFlowFunc> UseIncidentCustomerFindFlow()
        =>
        BotDependencyApi.UseCustomerSetSearchApi()
        .With(
            BotServiceProvider.GetLoggerFactory)
        .UseIncidentCustomerFindFlow();

    public static Dependency<IIncidentTitleGetFlowFunc> UseIncidentTitleGetFlow()
        =>
        IncidentTitleGetFlowDependency.UseIncidentTitleGetFlow();

    public static Dependency<IIncidentTypeGetFlowFunc> UseIncidentTypeGetFlow()
        =>
        IncidentTypeGetFlowDependency.UseIncidentTypeGetFlow();

}
