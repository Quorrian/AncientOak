using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientOak.AncientOakCode.Misc;

public static class PetHelper
{
    public const string AttackAnimation = "Attack";
    public const string SkillAnimation = "Skill";
    public const string PowerAnimation = "Power";
    public const string IdleAnimation = "Idle";
    private const float DisappearWait = 0.3f;
    
    public static async Task SwapVisuals(Creature? pet, PetVisual petVisual, bool animate = false)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
        var rootNode = creatureNode?.GetNode("MovePackPet");
        if (rootNode is null)
            return;
        MainFile.Logger.LogMessage(LogLevel.Warn, "Replacing Visuals", 0);
        if (animate)
        {
            PlayAnimation(pet, "Disappear");
            await Cmd.CustomScaledWait(DisappearWait, DisappearWait);
        }
        var sprite2D = rootNode.GetNode<Sprite2D>("%Sprite2D");
        var offsetNode = rootNode.GetNode<Node2D>("%Visuals");
        var newTexture = ResourceLoader.Load<Texture2D>(petVisual.TextureResourcePath);
        sprite2D.SetTexture(newTexture);
        sprite2D.Scale = new Vector2(petVisual.Scale, petVisual.Scale);
        offsetNode.Position = new Vector2(offsetNode.Position.X, petVisual.YPosition);
        MainFile.Logger.LogMessage(LogLevel.Warn, "Replaced Visuals", 0);
        if (animate)
            await Cmd.CustomScaledWait(DisappearWait, DisappearWait);
    }

    private static AnimationNodeStateMachinePlayback? GetAnimationStateMachine(Creature? pet)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
        var animationTree = creatureNode?.GetNode<AnimationTree>("MovePackPet/AnimationTree");
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
        stateMachine.Start(IdleAnimation);
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
        PlayAnimation(pet, AttackAnimation);
    }

    public static void PlaySkill<T>(Player player) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet, SkillAnimation);
    }

    public static void PlayPower<T>(Player player) where T : MonsterModel
    {
        var pet = player.PlayerCombatState?.GetPet<T>();
        PlayAnimation(pet, PowerAnimation);
    }
}