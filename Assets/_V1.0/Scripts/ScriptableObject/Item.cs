using UnityEngine;
[CreateAssetMenu(menuName = "New Item")]
public class Item : ScriptableObject
{
    public Sprite ItemIcon;
    public string ItemName;
    public float ItemDamage;
    //public GameObject ItemPrefab;
}
