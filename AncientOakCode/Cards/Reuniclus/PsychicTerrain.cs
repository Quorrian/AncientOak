using AncientOak.AncientOakCode.Misc;
using AncientOak.AncientOakCode.Powers;
using AncientOak.AncientOakCode.Relics;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientOak.AncientOakCode.Cards.Reuniclus;


[Pool(typeof(EventCardPool))]
public class PsychicTerrain() : CustomCardModel(1, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PsychicTerrainPower>(50M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        PetHelper.PlayPower<MovePackPet>(Owner);
        await PowerCmd.Apply<PsychicTerrainPower>(Owner.Creature, DynamicVars["PsychicTerrainPower"].BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade() => DynamicVars["PsychicTerrainPower"].UpgradeValueBy(25M);
}