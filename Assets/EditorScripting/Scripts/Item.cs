using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject {
    public Sprite sprite;
    public string itemName;
    public string description;
}
