//using System; 랜덤 오류나서 잠깐 없앰. 
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
    public ItemSlotUI[] uiSlots; //스크립트 이름이엇누
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
        slots = new ItemSlot[uiSlots.Length]; //강의자료는 uiSlots이네 ㅋㅋ 

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i; //Null이라 뜨는 부분
            uiSlots[i].Clear();
        }

        ClearSeletecItemWindow();
    }

 
    public void Toggle() //인벤토리 창 등장 사라지기. 
    {
        if(inventoryWindow.activeInHierarchy) //하이라이키 창에서 켜져있나?. 
        {
            inventoryWindow.SetActive(false); 
            onCloseInventory?.Invoke(); //꺼지는 것. 
            controller.ToggleCursor(false); //커서 없애는 것. 
        } 
        else
        {
            inventoryWindow.SetActive(true); 
            onOpenInventory?.Invoke(); //켜지는 것. 
            controller.ToggleCursor(true); //인벤토리에서 커서 사용. 
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        //켜져있음 켜지고 꺼져있음 꺼져라. 
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
            ItemSlot slotToStackTo = GetItemStack(item); //쌓을 수 있음 
            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++; //아이템을 찾고. 
                UPdateUI();
                return; 
            }
        }
        ItemSlot emptySlot = GetEmptySlot(); //위 처리가 안 됐으면? 
        if(emptySlot != null) //아니면 빈 칸 찾는. 
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UPdateUI(); 
            return; 
        }

        ThrowItem(item); //꽉 찼으면 지금 당장 필요없다
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
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount) //maxStackAmount보다 작으면?. 
                return slots[i];  //쌓을 수 있음 앃고 올리구 
        }
        return null; //넣을 상태로 만들어주는.. 
    } 

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) //아이템이 안 든. 
                return slots[i];  //쌓을 수 있음 앃고 올리구. 
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
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped); //아이템 장착중이냐 인덱스 이큅이냐. 
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
        equipButton.SetActive(false); //아이템 장착중이냐 
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
