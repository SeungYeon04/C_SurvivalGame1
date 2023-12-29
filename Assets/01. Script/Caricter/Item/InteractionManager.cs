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
                if (hit.collider.gameObject != curInteractGameobject) //������
                {
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
   
                }
            }
            else
            {
                curInteractGameobject = null;
                curInteractable = null; //���̾�����!! 
                promptText.gameObject.SetActive(false);
            }

        }
    }

    private void SetPromptText() //������ ��ȣ�ۿ� Eǥ�� 
    {
            promptText.gameObject.SetActive(true);
            promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt()); //������ �� �޾ƿ���.. 
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && curInteractable != null) //? == ? ���ǰ� �� ������ curInteractable�ٶ󺸸�? 
        {
            curInteractable.OnInteract();
            curInteractGameobject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false); 
        }
    }
}