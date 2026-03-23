using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace AncientOak.AncientOakCode.Powers;


public class SpeedPower: CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;
    
    
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : Math.Max(0M, count + (Amount/2M));
    }
}