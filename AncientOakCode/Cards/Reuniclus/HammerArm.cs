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

namespace AncientOak.AncientOakCode.Cards.Reuniclus;


[Pool(typeof(EventCardPool))]
public class HammerArm() : CustomCardModel(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20M, ValueProp.Move),
        new PowerVar<SpeedPower>(2)
    ];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        PetHelper.PlayAttack<MovePackPet>(Owner);
        var attackCommand = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack");
        await attackCommand.Execute(choiceContext);
        await PowerCmd.Apply<SpeedPower>(Owner.Creature, -DynamicVars[nameof(SpeedPower)].BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(5M);
}