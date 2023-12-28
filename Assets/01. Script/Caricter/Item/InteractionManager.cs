using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract(); 
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance; 
    public LayerMask layerMask;

    private GameObject curInteractGameobject;
    private IInteractable curInteractable; 

    //public text

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
