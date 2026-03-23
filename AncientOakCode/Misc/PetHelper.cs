using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientOak.AncientOakCode.Misc;

public static class PetHelper
{
    public static void PlayAnimation(Creature? pet, string animationName)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(pet);
        var animationPlayer = creatureNode?.GetNode("%AnimationPlayer") as AnimationPlayer;
        if (animationPlayer == null || animationPlayer.IsPlaying())
            return;
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