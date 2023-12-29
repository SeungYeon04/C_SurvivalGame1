using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour, IInteractable 
{
    public ItemData Item; //데이터만 가지고 있을 수 있는 

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
