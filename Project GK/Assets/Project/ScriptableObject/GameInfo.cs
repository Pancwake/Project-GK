using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    [Header("Base Stats")] //The base numbers for each stat
    [SerializeField] int baseMaxHealth;
    [SerializeField] int baseGoalHealthPenalty;
    [SerializeField] int baseRepelPoints;
    [SerializeField] int baseCatchPoints;
    [SerializeField] int baseCatchHeal;
    [SerializeField] float basePointsMultiplierIncrease;

    [Header("Upgrade Amount")] //The amount a stat will be upgraded
    [SerializeField] int maxHealthUpgradeAmount;
    [SerializeField] int goalHealthPenaltyUpgradeAmount;
    [SerializeField] int catchHealUpgradeAmount;
    [SerializeField] float pointsMultiplierIncreaseUpgradeAmount;

    [Header("Do not change")]

    [Header("Gameplay Stats")] //Stats that stay static during gameplay and can maybe be upgraded
    [SerializeField] public int maxHealth;
    [SerializeField] public int goalHealthPenalty;
    [SerializeField] public int repelPoints;
    [SerializeField] public int catchPoints;
    [SerializeField] public int catchHeal;
    [SerializeField] public float pointsMultiplierIncrease;

    [Header("Active Stats")] //Stats that change in gameplay
    public int currentHealth;
    public int points;
    public int combo;
    public float pointsMultiplier;

    public void CalculateMultiplier()
    {
        if (combo <= 0) //Dont do anything if no combo
            return;

        //1.2 = 1 + (0.1 * (3 - 1)) 
        pointsMultiplier = 1 + (pointsMultiplierIncrease * (combo - 1)); //combo -1 so it starts at the second hit
    }

    public void Upgrade(EUpgrades upgrade)
    {
        switch (upgrade)
        {
            case EUpgrades.maxHealth:
                maxHealth += maxHealthUpgradeAmount;
                break;
            case EUpgrades.goalHealthPenalty:
                goalHealthPenalty += goalHealthPenaltyUpgradeAmount;
                break;
            case EUpgrades.catchHeal:
                catchHeal += catchHealUpgradeAmount;
                break;
            case EUpgrades.pointsMultiplierIncrease:
                pointsMultiplierIncrease += pointsMultiplierIncreaseUpgradeAmount;
                break;
            default:
                Debug.LogError("No upgrade set for: " + upgrade.ToString());
                break;
        }
    }

    public void ResetStats()
    {
        maxHealth = baseMaxHealth;
        goalHealthPenalty = baseGoalHealthPenalty;
        repelPoints = baseRepelPoints;
        catchPoints = baseCatchPoints;
        catchHeal = baseCatchHeal;
        pointsMultiplierIncrease = basePointsMultiplierIncrease;

        currentHealth = baseMaxHealth;
        points = 0;
        combo = 0;
        pointsMultiplier = 1;
    }
}

public enum EUpgrades
{
    maxHealth,
    goalHealthPenalty,
    catchHeal,
    pointsMultiplierIncrease
}