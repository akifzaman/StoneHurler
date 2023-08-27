using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public GameObject stone;
    public Transform throwPosition;
    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    public bool ThrowStone()
    {
        if (InventoryManager.Instance.inventory.Items.Count > 0)
        {
            var go = Instantiate(stone, throwPosition.position, Quaternion.identity);
            go.GetComponent<ItemController>().IncreaseMoveSpeed(70f);
            InventoryManager.Instance.inventory.Items.RemoveAt(InventoryManager.Instance.inventory.Items.Count - 1);
            return true;
        }
        return false;
    }
}
