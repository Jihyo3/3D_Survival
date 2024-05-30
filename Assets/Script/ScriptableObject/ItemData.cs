using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resourcs,
    Special
}

public enum ConsumableType
{
    Health,
    Hunger
}

public enum SpecialType
{
    Speed,
    Size
}

[Serializable]

public class ItemDataConsumbale
{
    public ConsumableType type;
    public float value;
}

[Serializable]
public class SpecialItemData
{
    public SpecialType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
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

    [Header("Consumable")]
    public ItemDataConsumbale[] consumables;

    [Header("SpecialEat")]
    public SpecialItemData[] specialeat;
}
