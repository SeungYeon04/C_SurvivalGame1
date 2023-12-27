using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState 
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    { }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier; //런이니까 런속도로 맞춰 가져옴 
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }


}
