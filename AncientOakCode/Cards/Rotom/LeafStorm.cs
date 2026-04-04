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
public class LeafStorm() : CustomCardModel(3, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15M, ValueProp.Move),
        new PowerVar<BoostNextAttackPower>(50M),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("CalculatedHits").WithMultiplier((_, _) => GetNumTransformedThisCombat())
    ];

    private static int GetNumTransformedThisCombat()
    {
        return CountTransformsSubscriber.Singleton.TransformCount;
    }

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        PetHelper.PlayAttack<MovePackPet>(Owner);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitCount(GetNumTransformedThisCombat())
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack")
            .Execute(choiceContext);
        await PowerCmd.Apply<BoostNextAttackPower>(Owner.Creature, -DynamicVars[nameof(BoostNextAttackPower)].BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}