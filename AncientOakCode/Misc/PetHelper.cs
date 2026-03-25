using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientOak.AncientOakCode.Misc;

public static class PetHelper
{
    private const string PetAnimationScene = "res://AncientOak/Scenes/poke_animator.tscn";
    
    public static void PlayAnimation(Creature? pet, string animationName)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
        var petNode = creatureNode?.GetNode("MovePackPet");
        if (petNode == null)
        {
            MainFile.Logger.LogMessage(LogLevel.Warn, "No MovePackPet node.", 0);
            return;
        }
        var animationPlayer = petNode.GetNode<AnimationPlayer>("AnimationPlayer");
        //var animationPlayer = creatureNode?.GetChildrenRecursive<AnimationPlayer>().FirstOrDefault();
        if (animationPlayer == null)
        {
            // Add animation player node
            var animScene = ResourceLoader.Load<PackedScene>(PetAnimationScene).Instantiate();
            //var visuals = creature.GetNode<Node2D>("Visuals");
            petNode.AddChild(animScene);
            animationPlayer = petNode.GetNode<AnimationPlayer>("AnimationPlayer");
        }
        
        MainFile.Logger.LogMessage(LogLevel.Warn, string.Join(',',animationPlayer.GetAnimationList()), 0);
        if (animationPlayer.IsPlaying())
            MainFile.Logger.LogMessage(LogLevel.Warn, $"Already playing {animationPlayer.CurrentAnimation}", 0);
        animationPlayer.Play(animationName);
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