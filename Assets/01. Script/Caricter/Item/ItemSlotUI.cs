using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText; //TextMeshProUGUI 그 폰트 가능한 텍스트UI노.. 
    private ItemSlot curSlot;
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>(); 
    }

    private void OnEnable()
    {
        outline.enabled = equipped; 
    }

    //인벤토리 아이콘에 아이템 모 하는 건 여기서!!. 
    public void Set(ItemSlot slot)
    {
        curSlot = slot; 
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon; 
        quatityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty; //slot.quantity > 1 은 1보다 크면?. 

        if (outline != null ) 
        {
            outline.enabled = equipped; 
        }
    }

    public void Clear()
    {
        curSlot = null; 
        icon.gameObject.SetActive(false); 
        quatityText.text = string.Empty; 
    } 

    public void OnButtonClick()
    {
        Inventory.instance.SelectItem(index);
    }
}
