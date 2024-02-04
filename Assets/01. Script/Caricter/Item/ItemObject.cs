using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour, IInteractable 
{
    public ItemData item; //�����͸� ������ ���� �� �ִ� 

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", item.displayName);
    } 
    public void OnInteract()
    {
        Inventory.instance.AddItem(item); //������ �� �κ����� ������ ��. 
        Destroy(gameObject);
    }

}
