using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientOak.AncientOakCode.Misc;

public class CountTransformsSingleton : SingletonModel
{
    public override bool ShouldReceiveCombatHooks => true;

    public int TransformCount { get; set; } = 0;
    
    public override async Task BeforeCombatStart()
    {
        TransformCount = 0;
        await Task.CompletedTask;
    }
    public override Task AfterCombatEnd(CombatRoom room)
    {
        TransformCount = 0;
        return Task.CompletedTask;
    }
    public override Task AfterCombatVictory(CombatRoom room)
    {
        TransformCount = 0;
        return Task.CompletedTask;
    }
}

public static class CountTransformsSubscriber
{
    public static readonly CountTransformsSingleton Singleton = ModelDb.GetById<CountTransformsSingleton>(ModelDb.GetId<CountTransformsSingleton>());

    public static void Subscribe()
    {
        ModHelper.SubscribeForCombatStateHooks(AncientOakMainFile.ModId, _ => [Singleton]);
    }
}