using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum ItemType //소모? 장착 타입?
    {
        Resource, 
        Equipable, 
        Consumable 
    }

    public enum ConsumableType
    {
        Hunger, 
        Health 
    }


[CreateAssetMenu(fileName ="Item", menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; 
    public string description; 
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount; 
}

