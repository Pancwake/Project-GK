using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeManager", menuName = "Scriptable Objects/UpgradeManager")]
public class UpgradeManager : ScriptableObject
{
    [Header("List of all Upgrades")]
    [SerializeField] List<Upgrade> baseUpgrades;

    [Header("List of upgrades that have uses left")]
    public List<Upgrade> upgrades;

    public void ResetUpgrades()
    {
        upgrades = new List<Upgrade>();

        foreach (Upgrade u in baseUpgrades)
        {
            if (u.upgradeLevel == 1) //Only add level 1 upgrades
            {
                upgrades.Add(new Upgrade
                {
                    upgradeType = u.upgradeType,
                    upgradeLevel = u.upgradeLevel,
                    upgradeName = u.upgradeName,
                    upgradeDescription = u.upgradeDescription,
                    upgradeAmount = u.upgradeAmount,
                });
            }
        }
    }

    public float GetUpgradeAmount(EUpgrades usingUpgrade)
    {
        Upgrade upgrade = upgrades.Find(u => u.upgradeType == usingUpgrade);

        return upgrade.upgradeAmount;
    }

    public void UseUpgrade(EUpgrades usingUpgrade)
    {
        Upgrade upgrade = upgrades.Find(u => u.upgradeType == usingUpgrade);

        if (upgrade == null)
        {
            Debug.LogError("This upgrade does not exist anymore");
            return;
        }

        //Check if this upgrade has a next level version
        Upgrade nextLevelUpgrade = UpgradeToNextLevel(upgrade);

        if (nextLevelUpgrade != null)
        {
            int upgradeIndex = upgrades.IndexOf(upgrade);
        }
        else
        {
            upgrades.Remove(upgrade);
        }
        
    }

    public Upgrade UpgradeToNextLevel(Upgrade upgrade)
    {
        int nextLevel = upgrade.upgradeLevel + 1;

        bool upgradeFound = false;
        Upgrade newUpgrade = null;

        foreach (Upgrade nextUpgrade in baseUpgrades)
        {
            if (nextUpgrade.upgradeLevel == nextLevel && nextUpgrade.upgradeType == upgrade.upgradeType)
            {
                upgradeFound = true;
                newUpgrade = nextUpgrade;
                break;
            }
        }

        if (upgradeFound)
        {
            return newUpgrade;
        }
        else
        {
            return null;
        } 
    }
}

[Serializable]
public class Upgrades
{
    public List<Upgrade> upgrades;
}

[Serializable]
public class Upgrade
{
    public EUpgrades upgradeType;
    public int upgradeLevel;
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeCost;
    public float upgradeAmount;
}