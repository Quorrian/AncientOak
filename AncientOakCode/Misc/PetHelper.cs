using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientOak.AncientOakCode.Misc;

public static class PetHelper
{
    //private const string PetAnimationScene = "res://AncientOak/Scenes/poke_animator.tscn";

    // public static void AddAnimationNode(Creature? pet)
    // {
    //     var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
    //     var petNode = creatureNode?.GetNode("MovePackPet");
    //     if (petNode is null)
    //         return;
    //     var animScene = ResourceLoader.Load<PackedScene>(PetAnimationScene).Instantiate();
    //     petNode.AddChild(animScene);
    // }

    private static AnimationNodeStateMachinePlayback? GetAnimationStateMachine(Creature? pet)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
        var animationTree = creatureNode?.GetNode<AnimationTree>("MovePackPet/AnimationPlayer/AnimationTree");
        return (AnimationNodeStateMachinePlayback?)animationTree?.Get("parameters/playback");
    }
    
    public static void StartIdle(Creature? pet)
    {
        var stateMachine = GetAnimationStateMachine(pet);
        if (stateMachine == null)
        {
            MainFile.Logger.LogMessage(LogLevel.Warn, "No Animation Tree node found. Couldn't start Idle.", 0);
            return;
        }
        stateMachine.Start("Idle");
    }
    
    public static void PlayAnimation(Creature? pet, string animationName)
    {
        var stateMachine = GetAnimationStateMachine(pet);
        if (stateMachine == null)
        {
            MainFile.Logger.LogMessage(LogLevel.Warn, $"No Animation Tree node found. Couldn't start {animationName}.", 0);
            return;
        }
        stateMachine.Travel(animationName);
    }

    public static void PlayAnimation<T>(Player player, string animationName) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet, animationName);
    }

    public static void PlayAttack<T>(Player player) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet,"Attack");
    }

    public static void PlaySkill<T>(Player player) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet,"Skill");
    }

    public static void PlayPower<T>(Player player) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet,"Power");
    }
}