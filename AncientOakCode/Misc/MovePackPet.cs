using AncientOak.AncientOakCode.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace AncientOak.AncientOakCode.Misc;

public class MovePackPet : MonsterModel
{
    public const float AnimDelay = 0.8f;
    public override int MinInitialHp => 9999;
    public override int MaxInitialHp => 9999;
    public override bool IsHealthBarVisible => false;

    public override string VisualsPath => "res://AncientOak/Scenes/Reuniclus/reuniclus.tscn";

    public override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var initialState = new MoveState("NOTHING_MOVE", _ => Task.CompletedTask);
        initialState.FollowUpState = initialState;
        return new MonsterMoveStateMachine([initialState], initialState);
    }
}
