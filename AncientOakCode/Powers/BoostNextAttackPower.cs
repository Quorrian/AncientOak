using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

using MegaCrit.Sts2.Core.ValueProps;

namespace AncientOak.AncientOakCode.Powers;


public class BoostNextAttackPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;

    public override object InitInternalData() => new Data();

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        var internalData = GetInternalData<Data>();
        if (internalData.CommandToModify != null || command.ModelSource != null && command.ModelSource is not CardModel || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.CommandToModify = command;
        internalData.AmountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner != dealer || !props.IsPoweredAttack())
            return 1M;
        var internalData = GetInternalData<Data>();
        return internalData.CommandToModify != null && cardSource != null && cardSource != internalData.CommandToModify.ModelSource || internalData.CommandToModify != null && internalData.CommandToModify.Attacker != dealer
            ? 1M
            : 1M + Amount / 100M;
    }

    public override async Task AfterAttack(AttackCommand command)
    {
        var internalData = GetInternalData<Data>();
        if (command != internalData.CommandToModify)
            return;
        await PowerCmd.ModifyAmount(this, -internalData.AmountWhenAttackStarted, null, null);
    }

    private class Data
    {
        public AttackCommand? CommandToModify;
        public int AmountWhenAttackStarted;
    }
}