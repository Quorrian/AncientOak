using AncientOak.AncientOakCode.Misc;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientOak.AncientOakCode.Cards.Rotom;

[Pool(typeof(EventCardPool))]
public class WillOWisp() : CustomCardModel(1, CardType.Skill,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<DisintegrationPower>(4M),
        new PowerVar<StrengthPower>(2M)
    ];

    public override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        PetHelper.PlaySkill<MovePackPet>(Owner);
        
        await PowerCmd.Apply<DisintegrationPower>(play.Target, DynamicVars[nameof(DisintegrationPower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(play.Target, -DynamicVars[nameof(DisintegrationPower)].BaseValue, Owner.Creature, this);
    }

    public override void OnUpgrade()
    {
        DynamicVars[nameof(DisintegrationPower)].UpgradeValueBy(2M);
        DynamicVars.Strength.UpgradeValueBy(1M);
    }
}