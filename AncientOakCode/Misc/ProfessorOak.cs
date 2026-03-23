using AncientOak.AncientOakCode.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace AncientOak.AncientOakCode.Misc;

[Pool(typeof(AncientEventModel))]
public class ProfessorOak : CustomAncientModel
{
    protected override OptionPools MakeOptionPools => new OptionPools(MakePool([
        AncientOption<StarterChoice>()
    ]));

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}