using UnityEngine;

public class PickItem : MonoBehaviour, IPickable
{
    public Item item;
    public bool isPickable;
    private void Start()
    {
        isPickable = true;
        GetComponent<SpriteRenderer>().sprite = item.ItemIcon;
    }
    public bool Pick()
    {
        if (isPickable)
        {
            Item addedItem = InventoryManager.Instance.AddItemToInventory(item);
            if (addedItem != null)
            {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

}
