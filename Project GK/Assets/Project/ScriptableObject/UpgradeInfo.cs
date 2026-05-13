using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeInfo", menuName = "Scriptable Objects/UpgradeInfo")]
public class UpgradeInfo : ScriptableObject
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
            upgrades.Add(new Upgrade
            {
                upgradeType = u.upgradeType,
                maxUpgrades = u.maxUpgrades,
                upgradesLeft = u.maxUpgrades
            });
        }
    }

    public void UseUpgrade(EUpgrades usingUpgrade)
    {
        Upgrade upgrade = upgrades.Find(u => u.upgradeType == usingUpgrade);

        if (upgrade == null)
        {
            Debug.LogError("This upgrade does not exist anymore");
            return;
        }
            

        upgrade.upgradesLeft -= 1;

        if (upgrade.upgradesLeft <= 0)
        {
            upgrades.Remove(upgrade);
        }
    }
}

[Serializable]
public class Upgrade
{
    public EUpgrades upgradeType;
    public int maxUpgrades; //How often this can be upgraded
    public int upgradesLeft;

    public string upgradeName;
    public string upgradeDescription;

}