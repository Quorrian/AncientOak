using AncientOak.AncientOakCode.Misc;
using AncientOak.AncientOakCode.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Cards.Rotom;

[Pool(typeof(EventCardPool))]
public class Charge() : CustomCardModel(2, CardType.Skill,
    CardRarity.Ancient, TargetType.Self)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(11M, ValueProp.Move),
        new PowerVar<BoostNextAttackPower>(50M)
    ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        PetHelper.PlaySkill<MovePackPet>(Owner);
        
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<BoostNextAttackPower>(Owner.Creature, DynamicVars[nameof(BoostNextAttackPower)].BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2M);
}