using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndividualUpgradeShopHandler : MonoBehaviour
{
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDescription;
    [SerializeField] TextMeshProUGUI upgradePrice;

    [SerializeField] GameObject purchased;
    [SerializeField] GameObject held;
    [SerializeField] GameObject holdButton;
    [SerializeField] List<GameObject> hideWhenPurchased;
    [SerializeField] List<GameObject> hideWhenHeld;

    UpgradeShopManager upgradeShopManager;
    Upgrade upgrade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        purchased.SetActive(false);
        held.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyUpgrade(UpgradeShopManager upgradeShopManager, Upgrade upgrade)
    {
        this.upgradeShopManager = upgradeShopManager;
        this.upgrade = upgrade;

        upgradeImage.sprite = upgrade.image;
        upgradeName.text = upgrade.name;
        upgradeDescription.text = upgrade.description;
        upgradePrice.text = upgrade.price.ToString();
    }

    public void BuyUpgrade()
    {
        float money = upgradeShopManager.GetMoney();

        if (money >= upgrade.price) //If upgrade affordable
        {
            upgradeShopManager.BuyUpgrade(upgrade);
            ShowPurchased();
        }
    }

    public void HoldUpgrade()
    {
        upgradeShopManager.HoldUpgrade(upgrade, this);
        ShowHeld();
    }

    void ShowPurchased()
    {
        foreach (GameObject obj in hideWhenPurchased)
        {
            obj.SetActive(false);
        }
        purchased.SetActive(true);
    }

    void ShowHeld()
    {
        foreach (GameObject obj in hideWhenHeld)
        {
            obj.SetActive(false);
        }
        held.SetActive(true);
    }

    public void HideHoldButton()
    {
        holdButton.SetActive(false);
    }
}
