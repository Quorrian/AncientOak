using AncientOak.AncientOakCode.Misc;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Cards.Rotom;

[Pool(typeof(EventCardPool))]
public class HydroPump() : CustomCardModel(2, CardType.Attack,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20M, ValueProp.Move)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Overheat>(IsUpgraded),
        HoverTipFactory.FromCard<LeafStorm>(IsUpgraded),
        HoverTipFactory.FromPower<ArtifactPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        var pet = Owner.PlayerCombatState?.GetPet<MovePackPet>();
        await PetHelper.SwapVisuals(pet, PetVisual.RotomWash, true);
        PetHelper.PlayAnimation(pet, PetHelper.AttackAnimation);
        
        await CreatureCmd.LoseBlock(play.Target, play.Target.Block);
        if (play.Target.HasPower<ArtifactPower>())
            await PowerCmd.Remove<ArtifactPower>(play.Target);
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack")
            .Execute(choiceContext);
        await this.ChooseAndTransform(choiceContext, [ModelDb.Card<Overheat>(), ModelDb.Card<LeafStorm>()]);
    }

    public override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(5M);
}