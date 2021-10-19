using System;
using System.Collections.Generic;
using GGroupp.Infra.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace GGroupp.Internal.Support.Bot;

using IIncidentTypeGetFlowFunc = IAsyncValueFunc<DialogContext, Unit, ChatFlowStepResult<IncidentTypeGetFlowOut>>;

internal sealed partial class IncidentTypeGetFlowFunc : IIncidentTypeGetFlowFunc
{
    private static readonly IReadOnlyDictionary<int, string> typeCodes;

    public static IncidentTypeGetFlowFunc Instance { get; }

    static IncidentTypeGetFlowFunc()
    {
        Instance = new();
        typeCodes = new Dictionary<int, string>()
        {
            [1] = "������",
            [2] = "��������",
            [3] = "������"
        };
    }

    private IncidentTypeGetFlowFunc()
    {
    }
}
