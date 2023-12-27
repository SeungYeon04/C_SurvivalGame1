using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine 
{
    public Player Player { get; }

    //여기 정의였네 
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }

    public PlayerRunState RunState { get; }


    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }

    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;


    public float JumpForce { get; set; }

    public Transform MainCameraTransform { get; set; }


    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);


        MainCameraTransform = Camera.main.transform;

        MovementSpeed = player.Data.GroundedData.BaseSpeed;
        RotationDamping = player.Data.GroundedData.BaseRotationDamping;
    }

   // internal void ChangeState(object walkState)
   // {
    //    throw new NotImplementedException();
   // }
}
