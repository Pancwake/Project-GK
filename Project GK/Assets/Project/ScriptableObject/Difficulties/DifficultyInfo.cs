using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyInfo", menuName = "Scriptable Objects/DifficultyInfo")]
public class DifficultyInfo : ScriptableObject
{
    [Header("Base Stats")] //The base numbers for each stat
    [SerializeField] public int baseMaxHealth;
    [SerializeField] public int baseRepelMoneyReward;
    [SerializeField] public int baseCatchMoneyReward;
    [SerializeField] public int baseCatchHealPercentage;
    [SerializeField] public float baseMoneyMultiplierIncrease;
    [SerializeField] public float baseGoalAreaSize;
    [SerializeField] public int baseCatchAreaPercentage;
    [SerializeField] public float speedIncreasePercentagePerDifficulty;
    [SerializeField] public float baseForgivingRepelRadius;
}

public enum EDifficulties
{
    Easy,
    Medium,
    Hard
}