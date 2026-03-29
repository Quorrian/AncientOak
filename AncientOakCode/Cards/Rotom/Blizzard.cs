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
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Cards.Rotom;

[Pool(typeof(EventCardPool))]
public class Blizzard() : CustomCardModel(2, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    private const string Frost = "Frost";
    
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15M, ValueProp.Move),
        new IntVar(Frost, 2)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Overheat>(IsUpgraded),
        HoverTipFactory.FromCard<LeafStorm>(IsUpgraded),
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<FrostOrb>()
    ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var pet = Owner.PlayerCombatState?.GetPet<MovePackPet>();
        await PetHelper.SwapVisuals(pet, PetVisual.RotomFrost, true);
        PetHelper.PlayAnimation(pet, PetHelper.AttackAnimation);
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack")
            .Execute(choiceContext);
        
        for (var i = 0; i < DynamicVars[Frost].IntValue; ++i)
            await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
        
        await this.ChooseAndTransform(choiceContext, [ModelDb.Card<Overheat>(), ModelDb.Card<LeafStorm>()]);
    }

    public override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}