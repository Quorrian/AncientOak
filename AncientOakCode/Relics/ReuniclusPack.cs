using AncientOak.AncientOakCode.Cards.Reuniclus;
using AncientOak.AncientOakCode.Misc;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientOak.AncientOakCode.Relics;

[Pool(typeof(EventRelicPool))]
public class ReuniclusPack() : MovePack
{
    public override IEnumerable<CardModel> CardList =>
    [
        new ExpandingForce(),
        new HammerArm(),
        new PsychicTerrain(),
        new TrickRoom()
    ];

    protected override async Task SummonPet()
    {
        await PlayerCmd.AddPet<MovePackPet<ReuniclusPack>>(Owner);
    }
}