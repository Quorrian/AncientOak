using AncientOak.AncientOakCode.Cards.Rotom;
using AncientOak.AncientOakCode.Misc;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientOak.AncientOakCode.Relics;

[Pool(typeof(EventRelicPool))]
public class RotomPack() : MovePack
{
    public override List<CardModel> CardList =>
    [
        ModelDb.Card<Charge>(),
        ModelDb.Card<Discharge>(),
        ModelDb.Card<WillOWisp>(),
        ModelDb.Card<AirSlash>(),
    ];

    protected override async Task SummonPet()
    {
        var creature = await PlayerCmd.AddPet<MovePackPet>(Owner);
        await PetHelper.SwapVisuals(creature, PetVisual.Rotom);
    }
}