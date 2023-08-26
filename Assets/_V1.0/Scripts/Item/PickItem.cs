using UnityEngine;

public class PickItem : MonoBehaviour, IPickable
{
    [SerializeField] private Item item;
    [SerializeField] private PlayerController playerController;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.ItemIcon;
    }
    public void Pick()
    {
        Item addedItem = InventoryManager.Instance.AddItemToInventory(item);
        playerController.player.Mass += 10;
        if (addedItem != null) Destroy(gameObject);
    }

}
