using UnityEngine;
[CreateAssetMenu(menuName = "New Item")]
public class Item : ScriptableObject
{
    public Sprite ItemIcon;
    public string ItemName;
    public string ItemDamage;
    public GameObject ItemPrefab;
}
