using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    [Header("Base Stats")] //The base numbers for each stat
    [SerializeField] int baseMaxHealth;
    [SerializeField] int baseGoalHealthPenalty;
    [SerializeField] int baseRepelMoneyReward;
    [SerializeField] int baseCatchMoneyReward;
    [SerializeField] int baseCatchHeal;
    [SerializeField] float basePointsMultiplierIncrease;

    [Header("Do not change")]

    [Header("Gameplay Stats")] //Stats that stay static during gameplay and can maybe be upgraded
    [SerializeField] public int maxHealth;
    [SerializeField] public int goalHealthPenalty;
    [SerializeField] public int repelMoneyReward;
    [SerializeField] public int catchMoneyReward;
    [SerializeField] public int catchHeal;
    [SerializeField] public float moneyMultiplierIncrease;

    [Header("Active Stats")] //Stats that change in gameplay
    public int currentHealth;
    public int money;
    public int combo;
    public float moneyMultiplier;

    [SerializeField] UpgradeManager upgradeManager;

    public void CalculateMultiplier()
    {
        if (combo <= 0) //Dont do anything if no combo
            return;

        //1.2 = 1 + (0.1 * (3 - 1)) 
        moneyMultiplier = 1 + (moneyMultiplierIncrease * (combo - 1)); //combo -1 so it starts at the second hit
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        float upgradeAmount = upgradeManager.GetUpgradeAmount(upgrade);

        money -= upgrade.price;

        EUpgrades upgradeType = upgrade.type;

        switch (upgradeType)
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
                moneyMultiplierIncrease += upgradeAmount;
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
        repelMoneyReward = baseRepelMoneyReward;
        catchMoneyReward = baseCatchMoneyReward;
        catchHeal = baseCatchHeal;
        moneyMultiplierIncrease = basePointsMultiplierIncrease;

        currentHealth = baseMaxHealth;
        money = 0;
        combo = 0;
        moneyMultiplier = 1;
    }
}

public enum EUpgrades
{
    maxHealth,
    goalHealthPenalty,
    catchHeal,
    pointsMultiplierIncrease
}