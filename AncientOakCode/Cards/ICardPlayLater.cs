using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AncientOak.AncientOakCode.Cards;

public interface ICardPlayLater
{
    public Task AfterCardPlayedLater(PlayerChoiceContext choiceContext);
}