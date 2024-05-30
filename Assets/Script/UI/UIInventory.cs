using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject InventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStartName;
    public TextMeshProUGUI selectedStartValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        InventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStartName.text = string.Empty;
        selectedStartValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false); 
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if(IsOpen())
        {
            InventoryWindow.SetActive(false);
        }
        else
        {
            InventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return InventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 아이템이 중복 가능한지 canStack
        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        //비어있는 슬롯을 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        //있다면
        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //없다면
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex= index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStartName.text = string.Empty;
        selectedStartValue.text = string.Empty;

        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStartName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStartValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable || selectedItem.type == ItemType.Special);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnuseButton()
    {
        if(selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length;i++)
            {
                switch(selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }

        if (selectedItem.type == ItemType.Special)
        {
            for (int i = 0; i < selectedItem.specialeat.Length; i++)
            {
                switch (selectedItem.specialeat[i].type)
                {
                    case SpecialType.Size:
                        condition.SizeUp(selectedItem.specialeat[i].value);
                        condition.SizeUpTime(8f);
                        break;
                    //case SpecialType.Speed:
                    //    condition.Eat(selectedItem.specialeat[i].value);
                    //    break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }
    
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0 )
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
}
