using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Inventory inventory = new Inventory();
    public int inventoryCapacity;

    #region Singleton
    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

    public Item AddItemToInventory(Item item)
    {
        if (inventory.Items.Count >= inventoryCapacity) return null;
        inventory.Items.Add(item);
        if (!inventory.InventoryItemsCount.ContainsKey(item.ItemName))
        {
            List<Item> newItemsList = new List<Item>();
            newItemsList.Add(item);
            inventory.InventoryItemsCount.Add(item.ItemName, newItemsList);
        }
        else inventory.InventoryItemsCount[item.ItemName].Add(item);
        UIManager.Instance.OnStoneCountUpdated(inventory.Items.Count);
        return item;
    }
    public void RemoveFromInventory()
    {
        inventory.Items.RemoveAt(inventory.Items.Count - 1);
        UIManager.Instance.OnStoneCountUpdated(inventory.Items.Count);
    }
}
