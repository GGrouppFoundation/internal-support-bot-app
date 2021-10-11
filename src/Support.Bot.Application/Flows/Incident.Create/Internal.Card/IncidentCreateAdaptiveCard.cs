﻿using System;
using GGroupp.Infra;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GGroupp.Internal.Support.Bot;

internal static class IncidentCreateAdaptiveCard
{
    public static IActivity CreateIncidentCreateActivity(this DialogContext dialogContext, IncidentCreateFlowIn input)
        =>
        new AdaptiveCardJson("1.3")
        {
            Type = "AdaptiveCard",
            Body = new object[]
            {
                new
                {
                    type = "TextBlock",
                    size = "Medium",
                    weight = "Bolder",
                    text = input.Title
                },
                new
                {
                    type = "ColumnSet",
                    columns = new object[]
                    {
                        new
                        {
                            type = "Column",
                            items = new object[]
                            {
                                new
                                {
                                    type = "TextBlock",
                                    text = "От:"
                                }
                            },
                            width = "auto"
                        },
                        new
                        {
                            type = "Column",
                            items = new object[]
                            {
                                new
                                {
                                    type = "TextBlock",
                                    text = input.CustomerTitle,
                                    weight = "Bolder"
                                }
                            },
                            width = "auto"
                        }
                    }
                },
                new
                {
                    type = "TextBlock",
                    wrap = true,
                    text = input.Description
                }
            },
            Actions = new object[]
            {
                new
                {
                    type = "Action.Submit",
                    title = "Создать",
                    data = IncidentCreateActionDataJson.Create
                },
                new
                {
                    type = "Action.Submit",
                    title = "Отменить",
                    data = IncidentCreateActionDataJson.Cancel
                }
            }
        }
        .Pipe(
            dialogContext.Context.Activity.CreateReplyFromCard);
}

