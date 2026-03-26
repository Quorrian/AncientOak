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
    public override List<CardModel> CardList =>
    [
        ModelDb.Card<ExpandingForce>(),
        ModelDb.Card<HammerArm>(),
        ModelDb.Card<PsychicTerrain>(),
        ModelDb.Card<TrickRoom>(),
    ];

    protected override async Task SummonPet()
    {
        var creature = await PlayerCmd.AddPet<MovePackPet>(Owner);
        //if (creature.Monster is MovePackPet pet) pet.SubName = "-REUNICLUS";
        PetHelper.StartIdle(creature);
        // TODO try to make this update visuals for other types of pet
    }
}