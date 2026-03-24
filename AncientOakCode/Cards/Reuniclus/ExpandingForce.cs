using AncientOak.AncientOakCode.Misc;
using AncientOak.AncientOakCode.Powers;
using AncientOak.AncientOakCode.Relics;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Cards.Reuniclus;


[Pool(typeof(EventCardPool))]
public class ExpandingForce() : CustomCardModel(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override TargetType TargetType
        => HasPsychicTerrain ? TargetType.AllEnemies : TargetType.AnyEnemy;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15M, ValueProp.Move)
    ];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        PetHelper.PlayAttack<MovePackPet>(Owner);
        var attackCommand = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack");
        attackCommand = HasPsychicTerrain
            ? attackCommand.TargetingAllOpponents(CombatState)
            : attackCommand.Targeting(cardPlay.Target);
        await attackCommand.Execute(choiceContext);
    }

    public override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(5M);
    
    private bool HasPsychicTerrain
        => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<PsychicTerrainPower>();
}