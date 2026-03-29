using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AncientOak.AncientOakCode.Cards.Rotom;

public static class RotomHelper
{
    public static async Task ChooseAndTransform(this CardModel card, PlayerChoiceContext choiceContext, List<CardModel> choices)
    {
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