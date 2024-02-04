using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour, IInteractable 
{
    public ItemData item; //데이터만 가지고 있을 수 있는 

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", item.displayName);
    } 
    public void OnInteract()
    {
        Inventory.instance.AddItem(item); //눌렀을 때 인벤으로 아이템 들어감. 
        Destroy(gameObject);
    }

}
