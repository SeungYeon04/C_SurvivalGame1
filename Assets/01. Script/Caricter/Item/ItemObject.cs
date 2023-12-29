using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour, IInteractable 
{
    public ItemData Item; //�����͸� ������ ���� �� �ִ� 

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", Item.displayName);
    } 
    public void OnInteract()
    {
        //Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }

}
