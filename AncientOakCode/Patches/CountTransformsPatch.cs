using AncientOak.AncientOakCode.Misc;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

namespace AncientOak.AncientOakCode.Patches;


[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Transform), typeof(IEnumerable<CardTransformation>), typeof(Rng), typeof(CardPreviewStyle))]
public static class CountTransformsPatch
{
    [HarmonyPostfix]
    public static async Task<IEnumerable<CardPileAddResult>> PostFix(Task<IEnumerable<CardPileAddResult>> __result)
    {
        AncientOakMainFile.Logger.LogMessage(LogLevel.Warn, "Count Transform Patch", 0);
        var realResults = (await __result).ToList();
        CountTransformsSubscriber.Singleton.TransformCount += realResults.Count;
        return realResults;
    }
}