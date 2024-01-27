//using System; ���� �������� ��� ����. 
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events; 
using static UnityEditor.Progress;

public class ItemSlot
{
    public ItemData item;
    public int quantity; 
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlot; //��ũ��Ʈ �̸��̾���
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]

    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
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
        slots = new ItemSlot[uiSlot.Length]; //�����ڷ�� uiSlots�̳� ���� 

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlot[i].index = i;
            uiSlot[i].Clear();
        }

        ClearSeletecItemWindow();
    }

 
    public void Toggle() //�κ��丮 â ���� �������. 
    {
        if(inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false); 
        } 
        else
        {
            inventoryWindow.SetActive(true); 
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
            ItemSlot slotToStackTo = GetItemStack(item); //���� �� ���� �װ� 
            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return; 
            }
        }
        ItemSlot emptySlot = GetEmptySlot(); //�� ó���� �� ������? 
        if(emptySlot != null) //�ƴϸ� �� ĭ ã��. 
        {
            emptySlot.item = item;
            emptySlot.quantity = 1; 
            UpdateUI(); 
            return; 
        }

        ThrowItem(item); //�� á���� ���� ���� �ʿ����
    } 

    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f)); 
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            
                uiSlot[i].Set(slots[i]); 
            else
                uiSlot[i].Clear(); 
            
        }
    } 

    ItemSlot GetItemStack(ItemData item)
    {
        for(int i = 0; i < slots.Length; i++ )
        {
            if (slots[i].item != item && slots[i].quantity < item.maxStackAmount)
                return slots[i];  //���� �� ���� �ϰ� �ø��� 
        }
        return null; //���� ���·� ������ִ�.. 
    } 

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];  //���� �� ���� �ϰ� �ø��� 
        } 
        return null; 
    } 

    public void SelectItem(int index)
    {
        if (slots[index].item != null)
            return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemName.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        /*
        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }*/
        
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlot[index].equipped); //������ �������̳� 
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlot[index].equipped);
        dropButton.SetActive(true);
    }

    private void ClearSeletecItemWindow() 
    { 

        selectedItem = null;
        selectedItemName.text = string.Empty; 
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

    void UpEquip(int index)
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
