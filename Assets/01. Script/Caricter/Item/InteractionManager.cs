using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public CharacterController Controller { get; private set; } 

    public TextMeshProUGUI promptText;
    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;


            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) 
            {
                if (hit.collider.gameObject != curInteractGameobject) //문제점
                {
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
   
                }
            }
            else
            {
                curInteractGameobject = null;
                curInteractable = null; //널이었구나!! 
                promptText.gameObject.SetActive(false);
            }

        }
    }

    private void SetPromptText() //아이템 상호작용 E표시 
    {
            promptText.gameObject.SetActive(true);
            promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt()); //위에서 널 받아오네.. 
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && curInteractable != null) //? == ? 눌렷고 그 시점에 curInteractable바라보면? 
        {
            curInteractable.OnInteract();
            curInteractGameobject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false); 
        }
    }
}