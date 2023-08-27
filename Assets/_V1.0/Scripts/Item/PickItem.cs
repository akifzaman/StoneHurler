using UnityEngine;

public class PickItem : MonoBehaviour, IPickable
{
    public Item item;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.ItemIcon;
    }
    public void Pick()
    {
        Item addedItem = InventoryManager.Instance.AddItemToInventory(item);
        if (addedItem != null) Destroy(gameObject);
    }

}
