using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public GameObject stone;
    public Transform throwPosition;
    [SerializeField] private float speedMultiplier;
    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    public bool ThrowStone(float value)
    {
        if (InventoryManager.Instance.inventory.Items.Count > 0)
        {
            var go = Instantiate(stone, throwPosition.position, Quaternion.identity);
            go.GetComponent<ItemController>().IncreaseMoveSpeed(speedMultiplier, value);
            InventoryManager.Instance.RemoveFromInventory();
            return true;
        }
        return false;
    }
}
