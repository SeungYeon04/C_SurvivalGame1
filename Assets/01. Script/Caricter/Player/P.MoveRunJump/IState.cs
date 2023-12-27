using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState //인터페이스라 내부구현 없음 
{
    public void Enter(); 
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}
