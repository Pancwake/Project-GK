using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndividualUpgradeShopHandler : MonoBehaviour
{
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDescription;
    [SerializeField] TextMeshProUGUI upgradePrice;

    [SerializeField] GameObject notPurchased;
    [SerializeField] GameObject purchased;

    UpgradeShopManager upgradeShopManager;
    Upgrade upgrade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notPurchased.SetActive(true);
        purchased.SetActive(false);
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

    void ShowPurchased()
    {
        notPurchased.SetActive(false);
        purchased.SetActive(true);
    }
}
