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
public class AirSlash() : CustomCardModel(1, CardType.Attack,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    private const string StrengthLossKey = "StrengthLoss";
    
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10M, ValueProp.Move),
        new(StrengthLossKey, 3M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<HydroPump>(IsUpgraded),
        HoverTipFactory.FromCard<Blizzard>(IsUpgraded)
    ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        var pet = Owner.PlayerCombatState?.GetPet<MovePackPet>();
        await PetHelper.SwapVisuals(pet, PetVisual.RotomFan, true);
        PetHelper.PlayAnimation(pet, PetHelper.AttackAnimation);
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack")
            .Execute(choiceContext);
        await PowerCmd.Apply<ManglePower>(play.Target, DynamicVars[StrengthLossKey].BaseValue, Owner.Creature, this);
        
        await this.ChooseAndTransform(choiceContext, [ModelDb.Card<HydroPump>(), ModelDb.Card<Blizzard>()]);
    }

    public override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M);
        DynamicVars[StrengthLossKey].UpgradeValueBy(1M);
    }
}