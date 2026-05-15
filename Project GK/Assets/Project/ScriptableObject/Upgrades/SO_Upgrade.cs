using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public EUpgrades type;
    public Sprite image;
    public string uName;
    [TextArea(2, 3)] public string description;
    public int price;
    public float amount;
}