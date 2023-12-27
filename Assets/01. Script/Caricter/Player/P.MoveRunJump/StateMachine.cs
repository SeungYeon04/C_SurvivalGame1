using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI; 
//상태머신 
public abstract class StateMachine //추상클래스 
{
    protected IState currentState; //인터페이스인 스크립트 왜 불러왔을까 

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void HandleInput() 
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update(); 
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
