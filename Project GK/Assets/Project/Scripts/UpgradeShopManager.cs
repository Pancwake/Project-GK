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
        UpdateInfo();
        OpenShop();
    }

    void Update()
    {
        
    }

    void OpenShop()
    {
        availableUpgrades = new List<Upgrade>(upgradeManager.upgrades);

        for (int i = 0; i < upgradeHandlers.Count; i++)
        {
            if (availableUpgrades.Count <= 0)
            {
                Debug.LogError("No more upgrades left");
                return;
            }

            //Apply held upgrade to the right slot
            if (upgradeManager.heldUpgrade != null)
            {
                if (i == upgradeManager.heldSlot)
                {
                    upgradeHandlers[i].ApplyUpgrade(this, upgradeManager.heldUpgrade);
                    RemoveHeldUpgrade(); //Remove held upgrade after assigning
                    continue;
                }
            }

            int rng = Random.Range(0, availableUpgrades.Count);

            upgradeHandlers[i].ApplyUpgrade(this, availableUpgrades[rng]);

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

    public void ExitShop()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    public void HoldUpgrade(Upgrade upgrade, IndividualUpgradeShopHandler upgradeHandler)
    {
        int upgradeIndex = upgradeHandlers.IndexOf(upgradeHandler);
        upgradeManager.AssignHeldUpgrade(upgrade, upgradeIndex);

        //Only allow 1 hold per shop visit
        foreach (var upgradeH in upgradeHandlers)
        {
            upgradeH.HideHoldButton();
        }
    }

    public void RemoveHeldUpgrade()
    {
        upgradeManager.AssignHeldUpgrade(); //Set automatically to null
    }

    public int GetMoney()
    {
        return gameInfo.money;
    }
}
