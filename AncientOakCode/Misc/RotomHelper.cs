using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace AncientOak.AncientOakCode.Misc;

public static class RotomHelper
{
    public static async Task ChooseAndTransform(this CardModel card, PlayerChoiceContext choiceContext, List<CardModel> choices)
    {
        AncientOakMainFile.Logger.LogMessage(LogLevel.Warn, "Choose And Transform.", 0);
        if (card.CombatState == null) return;
        var createdChoices = choices.Select(x => card.CombatState.CreateCard(x, card.Owner)).ToList();
        if (card.IsUpgraded)
            foreach (var createdChoice in createdChoices)
                CardCmd.Upgrade(createdChoice);
        
        var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, createdChoices, card.Owner);
        if (chosenCard == null)
            return;
        await CardCmd.Transform(card, chosenCard);
    }
}