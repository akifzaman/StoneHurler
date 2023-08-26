using System.Collections.Generic;

[System.Serializable]
public class Inventory
{
    public List<Item> Items = new List<Item>();
    public Dictionary<string, List<Item>> InventoryItemsCount = new Dictionary<string, List<Item>>();
}