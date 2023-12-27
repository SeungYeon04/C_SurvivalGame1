using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI; 
//���¸ӽ� 
public abstract class StateMachine //�߻�Ŭ���� 
{
    protected IState currentState; //�������̽��� ��ũ��Ʈ �� �ҷ������� 

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
