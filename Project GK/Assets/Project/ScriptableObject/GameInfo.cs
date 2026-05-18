using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    [Header("Base Stats")] //The base numbers for each stat
    [SerializeField] int baseMaxHealth;
    [SerializeField] int baseRepelMoneyReward;
    [SerializeField] int baseCatchMoneyReward;
    [SerializeField] int baseCatchHealPercentage;
    [SerializeField] float baseMoneyMultiplierIncrease;
    [SerializeField] float baseGoalAreaSize;
    [SerializeField] int baseCatchAreaPercentage;
    [SerializeField] float speedIncreasePercentagePerDifficulty;
    [SerializeField] float baseForgivingRepelRadius;

    [SerializeField] public int stadiumAmount; //How many stadiums there are
    [SerializeField] public int levelsPerStadium; //The amount of levels that are per stadium
    [SerializeField] public int shotsPerLevel; //The amount of shots that are per level 

    [Header("Do not change")]

    [Header("Gameplay Stats")] //Stats that stay static during gameplay and can maybe be upgraded
    [SerializeField] public int maxHealth;
    [SerializeField] public int repelMoneyReward;
    [SerializeField] public int catchMoneyReward;
    [SerializeField] public int catchHealPercentage;
    [SerializeField] public float moneyMultiplierIncrease;
    [SerializeField] public float goalAreaSize;
    [SerializeField] public int catchAreaPercentage;
    [SerializeField] public float difficultySpeedModifier;
    [SerializeField] public float upgradeSpeedModifier;
    [SerializeField] public float forgivingRepelRadius;

    [SerializeField] public int currentStadiumIndex; //The stadium the player is currently on
    [SerializeField] public int currentStadiumLevel; //The level this stadium is currently on

    [Header("Active Stats")] //Stats that change in gameplay
    public int currentHealth;
    public int money;
    public int combo;
    public float moneyMultiplier;
    public bool paused;

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
                maxHealth += (int)upgradeAmount; //Increase max health by that amount
                currentHealth += (int)upgradeAmount; //Heal the amount too
                break;
            case EUpgrades.catchHealPercentage:
                catchHealPercentage += (int)upgradeAmount;
                break;
            case EUpgrades.goalAreaPercentage:
                float sizeIncrease = baseGoalAreaSize * (float)(upgradeAmount / 100); //Get percentage increase
                goalAreaSize += sizeIncrease; //Increase size by that amount
                break;
            case EUpgrades.catchAreaPercentage:
                catchAreaPercentage += (int)upgradeAmount; //Increase catching area by that amount
                break;
            case EUpgrades.shootSpeedDecreasePercent:
                float speedPercentage = (float)(upgradeAmount/100);
                upgradeSpeedModifier -= (float)speedPercentage;
                break;
            case EUpgrades.forgivingRepelRadiusIncreasePercent:
                float repelBoxRadiusIncrease = baseForgivingRepelRadius * (float)(upgradeAmount / 100);
                forgivingRepelRadius += (float)repelBoxRadiusIncrease;
                break;
            default:
                Debug.LogError("No upgrade set for: " + upgrade.ToString());
                break;
        }

        upgradeManager.UseUpgrade(upgrade);
    }

    public void CalculateDifficulty()
    {
        if (currentStadiumLevel <= 1)
        {
            difficultySpeedModifier = 1;
            return;
        }
        
        difficultySpeedModifier = 1 + ((currentStadiumLevel - 1) * (speedIncreasePercentagePerDifficulty / 100)); //Level 2: (2 - 1) * 0.2f = 1 increase 
    }

    public void ResetStats()
    {
        upgradeManager.ResetUpgrades();

        maxHealth = baseMaxHealth;
        repelMoneyReward = baseRepelMoneyReward;
        catchMoneyReward = baseCatchMoneyReward;
        catchHealPercentage = baseCatchHealPercentage;
        moneyMultiplierIncrease = baseMoneyMultiplierIncrease;
        goalAreaSize = baseGoalAreaSize;
        catchAreaPercentage = baseCatchAreaPercentage;
        forgivingRepelRadius = baseForgivingRepelRadius;

        currentHealth = baseMaxHealth;
        money = 0;
        combo = 0;
        moneyMultiplier = 1;
        currentStadiumIndex = 1;
        currentStadiumLevel = 1;
        difficultySpeedModifier = 1;
        upgradeSpeedModifier = 1;
        paused = false;
    }
}

public enum EUpgrades
{
    maxHealth,
    catchHealPercentage,
    goalAreaPercentage,
    catchAreaPercentage,
    shootSpeedDecreasePercent,
    forgivingRepelRadiusIncreasePercent,
}