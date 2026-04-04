using AncientOak.AncientOakCode.Misc;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

namespace AncientOak.AncientOakCode.Patches;


[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Transform), typeof(IEnumerable<CardTransformation>), typeof(Rng), typeof(CardPreviewStyle))]
public static class CountTransformsPatch
{
    public static Task<IEnumerable<CardPileAddResult>> PostFix(Task<IEnumerable<CardPileAddResult>> results)
    {
        CountTransformsSubscriber.Singleton.TransformCount += results.Result.Count();
        return results;
    }
}