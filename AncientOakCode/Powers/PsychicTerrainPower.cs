using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Powers;

public class PsychicTerrainPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // if not all enemies, normal damage. if all enemies, multiply damage
        return !props.IsPoweredAttack() || cardSource == null || cardSource.Owner.Creature != Owner || cardSource.TargetType != TargetType.AllEnemies
            ? 1M
            : 1M + Amount / 100M;
    }
}