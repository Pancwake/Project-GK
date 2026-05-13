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

    [SerializeField] UpgradeManager upgradeManager;

    public void CalculateMultiplier()
    {
        if (combo <= 0) //Dont do anything if no combo
            return;

        //1.2 = 1 + (0.1 * (3 - 1)) 
        pointsMultiplier = 1 + (pointsMultiplierIncrease * (combo - 1)); //combo -1 so it starts at the second hit
    }

    public void Upgrade(EUpgrades upgrade)
    {
        float upgradeAmount = upgradeManager.GetUpgradeAmount(upgrade);

        switch (upgrade)
        {
            case EUpgrades.maxHealth:
                maxHealth += (int)upgradeAmount;
                break;
            case EUpgrades.goalHealthPenalty:
                goalHealthPenalty += (int)upgradeAmount;
                break;
            case EUpgrades.catchHeal:
                catchHeal += (int)upgradeAmount;
                break;
            case EUpgrades.pointsMultiplierIncrease:
                pointsMultiplierIncrease += upgradeAmount;
                break;
            default:
                Debug.LogError("No upgrade set for: " + upgrade.ToString());
                break;
        }

        upgradeManager.UseUpgrade(upgrade);
    }

    public void ResetStats()
    {
        upgradeManager.ResetUpgrades();

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