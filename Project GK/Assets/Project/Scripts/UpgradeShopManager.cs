using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeShopManager : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;
    [SerializeField] UpgradeManager upgradeManager;

    List<Upgrade> availableUpgrades;

    [SerializeField] List<IndividualUpgradeShopHandler> upgradeHandlers;

    [SerializeField] TextMeshProUGUI money;

    void Start()
    {
        upgradeManager.ResetUpgrades(); //Only for debugging

        UpdateInfo();
        OpenShop();
    }

    void Update()
    {
        
    }

    void OpenShop()
    {
        availableUpgrades = new List<Upgrade>(upgradeManager.upgrades);

        foreach (IndividualUpgradeShopHandler upgradeHandler in upgradeHandlers)
        {
            if (availableUpgrades.Count <= 0)
            {
                Debug.LogError("No more upgrades left");
                return;
            }

            int rng = Random.Range(0, availableUpgrades.Count);

            upgradeHandler.ApplyUpgrade(this, availableUpgrades[rng]);

            availableUpgrades.RemoveAt(rng); //Remove to not have duplicates
        }
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        gameInfo.BuyUpgrade(upgrade);

        UpdateInfo();
    }

    void UpdateInfo()
    {
        money.text = gameInfo.money.ToString();
    }

    public int GetMoney()
    {
        return gameInfo.money;
    }
}
