using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace AncientOak.AncientOakCode.Patches;

[HarmonyPatch(typeof(MonsterModel), nameof(MonsterModel.CreateVisuals))]
public static class CreateVisualsPatch
{
    private static readonly MethodInfo _visualsPathGetter = typeof(MonsterModel)
        .GetProperty("VisualsPath", BindingFlags.Instance | BindingFlags.NonPublic)
        .GetGetMethod(true);

    public static bool Prefix(MonsterModel __instance, ref NCreatureVisuals __result)
    {
        var path = (string)_visualsPathGetter.Invoke(__instance, null);
        var scene = PreloadManager.Cache.GetScene(path);

        try
        {
            __result = scene.Instantiate<NCreatureVisuals>();
            return false;
        }
        catch (InvalidCastException)
        {
            // Mod scene didn't register the script properly
        }

        var raw = scene.Instantiate<Node2D>();
        var visuals = new NCreatureVisuals();
        visuals.Name = raw.Name;
        raw.ReplaceBy(visuals,true);
        raw.QueueFreeSafely();
        __result = visuals;

        return false;
    }
}