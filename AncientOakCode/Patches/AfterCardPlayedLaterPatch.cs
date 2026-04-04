using AncientOak.AncientOakCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace AncientOak.AncientOakCode.Patches;


[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class AfterCardPlayedLaterPatch
{
    [HarmonyPostfix]
    public static async Task PostFix(Task __result, PlayerChoiceContext choiceContext, CardModel __instance)
    {
        await __result;
        if (__instance is ICardPlayLater card)
            await card.AfterCardPlayedLater(choiceContext);
    }
}