//using System; ���� �������� ��� ����. 
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class ItemSlot
{
    public ItemData item;
    public int quantity; 
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots; //��ũ��Ʈ �̸��̾���
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]

    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemNames;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerConditions condition;

    [Header("Events")]

    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    public static Inventory instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerConditions>();
    }

    private void Start()
    {
        
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length]; //�����ڷ�� uiSlots�̳� ���� 

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i; //Null�̶� �ߴ� �κ�
            uiSlots[i].Clear();
        }

        ClearSeletecItemWindow();
    }

 
    public void Toggle() //�κ��丮 â ���� �������. 
    {
        if(inventoryWindow.activeInHierarchy) //���̶���Ű â���� �����ֳ�?. 
        {
            inventoryWindow.SetActive(false); 
            onCloseInventory?.Invoke(); //������ ��. 
            controller.ToggleCursor(false); //Ŀ�� ���ִ� ��. 
        } 
        else
        {
            inventoryWindow.SetActive(true); 
            onOpenInventory?.Invoke(); //������ ��. 
            controller.ToggleCursor(true); //�κ��丮���� Ŀ�� ���. 
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        //�������� ������ �������� ������. 
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy; 
    }
  
    public void AddItem(ItemData item)
    { 
        if(item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item); //���� �� ���� 
            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++; //�������� ã��. 
                UPdateUI();
                return; 
            }
        }
        ItemSlot emptySlot = GetEmptySlot(); //�� ó���� �� ������? 
        if(emptySlot != null) //�ƴϸ� �� ĭ ã��. 
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UPdateUI(); 
            return; 
        }

        ThrowItem(item); //�� á���� ���� ���� �ʿ����
    } 

    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f)); 
    }

    void UPdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            
                uiSlots[i].Set(slots[i]); 
            else
                uiSlots[i].Clear(); 
            
        }
    } 



    ItemSlot GetItemStack(ItemData item)
    {
        for(int i = 0; i < slots.Length; i++ )
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount) //maxStackAmount���� ������?. 
                return slots[i];  //���� �� ���� �ϰ� �ø��� 
        }
        return null; //���� ���·� ������ִ�.. 
    } 

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) //�������� �� ��. 
                return slots[i];  //���� �� ���� �ϰ� �ø���. 
        } 
        return null; 
    } 

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemNames.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        
    //    for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //    {
    //        selectedItemNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
    //        selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
     //   }
        
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped); //������ �������̳� �ε��� ��Ţ�̳�. 
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    private void ClearSeletecItemWindow() 
    { 

        selectedItem = null;
        selectedItemNames.text = string.Empty; 
        selectedItemDescription.text = string.Empty;

        selectedItemStatNames.text = string.Empty;   
        selectedItemStatValues.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false); //������ �������̳� 
        unEquipButton.SetActive(false); 
        dropButton.SetActive(false); 


    }

    public void OnUseButton()
    {

    }

    public void OnEquipButton()
    {

    } 

    void UnEquip(int index)
    {

    }

    public void OnUnEquipButton()
    {

    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem(); 
    }

    private void RemoveSelectedItem()
    {
    } 

    public void RemoveItem(ItemData item)
    {

    } 

    public bool HasItems(ItemData item, int quantity)
    {
        return false; 
    }
}
