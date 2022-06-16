using System;
using GGroupp.Infra.Bot.Builder;

namespace GGroupp.Internal.Support;

using IUserSetSearchFunc = IAsyncValueFunc<UserSetSearchIn, Result<UserSetSearchOut, Failure<UserSetSearchFailureCode>>>;

internal static class OwnerGetFlowStep
{
    internal static ChatFlow<IncidentCreateFlowState> GetOwner(
        this ChatFlow<IncidentCreateFlowState> chatFlow, IUserSetSearchFunc userSetSearchFunc)
        =>
        chatFlow.SendText(
            _ => "Нужно выбрать ответственного")
        .AwaitLookupValue(
            OwnerGetHelper.GetDefaultOwnerAsync,
            userSetSearchFunc.SearchUsersOrFailureAsync,
            OwnerGetHelper.CreateResultMessage,
            MapFlowState);

    private static IncidentCreateFlowState MapFlowState(IncidentCreateFlowState flowState, LookupValue ownerValue)
        =>
        flowState with
        { 
            OwnerId = ownerValue.Id, 
            OwnerFullName = ownerValue.Name
        };
}