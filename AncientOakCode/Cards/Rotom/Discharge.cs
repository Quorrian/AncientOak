using AncientOak.AncientOakCode.Misc;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Cards.Rotom;

[Pool(typeof(EventCardPool))]
public class Discharge() : CustomCardModel(2, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(15M, ValueProp.Move),
            new PowerVar<WeakPower>(1M)
        ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        PetHelper.PlayAttack<MovePackPet>(Owner);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(CombatState.HittableEnemies, DynamicVars.Weak.BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M);
        DynamicVars.Weak.UpgradeValueBy(1M);
    }
}