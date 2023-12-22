using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    public void Enter(); 
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}
