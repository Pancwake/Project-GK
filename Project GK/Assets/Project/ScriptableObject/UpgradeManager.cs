using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeManager", menuName = "Scriptable Objects/UpgradeManager")]
public class UpgradeManager : ScriptableObject
{
    [Header("List of all Upgrades")]
    [SerializeField] List<Upgrades> baseUpgrades;

    [Header("List of upgrades that can be bought")]
    public List<Upgrade> upgrades;

    public void ResetUpgrades()
    {
        upgrades = new List<Upgrade>();

        foreach (Upgrades us in baseUpgrades)
        {
            //Apply all types automatically
            foreach (Upgrade u in us.upgrades)
            {
                u.type = us.upgradeType;
            }

            Upgrade upgrade = us.upgrades[0]; //Add the first upgrade

            upgrades.Add(upgrade);
        }
    }

    public float GetUpgradeAmount(Upgrade upgrade)
    {
        return upgrade.amount;
    }

    public void UseUpgrade(Upgrade upgrade)
    {
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
            upgrades[upgradeIndex] = nextLevelUpgrade;
        }
        else
        {
            upgrades.Remove(upgrade);
        }
        
    }

    public Upgrade UpgradeToNextLevel(Upgrade upgrade)
    {
        Upgrades upgradesList = baseUpgrades.Find(us => us.upgradeType == upgrade.type);

        int nextLevel = upgradesList.upgrades.IndexOf(upgrade);
        nextLevel += 1;

        if (nextLevel < upgradesList.upgrades.Count)
        {
            Debug.Log("Upgrade " + upgrade.name + " to " + upgradesList.upgrades[nextLevel].name);

            return upgradesList.upgrades[nextLevel];
        }

        Debug.Log("Upgrade " + upgrade.name + " is the last in the line.");

        return null;
    }
}

[Serializable]
public class Upgrades
{
    public EUpgrades upgradeType;
    public List<Upgrade> upgrades;
}

[Serializable]
public class Upgrade
{
    [HideInInspector] public EUpgrades type; //Hide because it gets assigned automatically
    public Sprite image;
    public string name;
    [TextArea(2, 3)] public string description;
    public int price;
    public float amount;
}