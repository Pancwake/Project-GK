using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI shot;

    [SerializeField] GameInfo gameInfo;
    GameManager gameManager;

    [SerializeField] GameObject catchText;
    [SerializeField] TextMeshProUGUI healAmountText;
    [SerializeField] float catchTextTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        string healthText = gameInfo.currentHealth + "/" + gameInfo.maxHealth;
        health.text = healthText;

        money.text = gameInfo.money.ToString();

        string levelText = gameInfo.currentStadiumLevel + "/" + gameInfo.levelsPerStadium;
        level.text = levelText;

        string shotText = (gameManager.currentShot - 1) + "/" + gameInfo.shotsPerLevel;
        shot.text = shotText;
    }

    public void ShowCatchText(int amount)
    {
        string text = "+" + amount.ToString() + " health!";

        healAmountText.text = text;

        catchText.SetActive(true);

        StartCoroutine(RemoveCatchText());
    }

    IEnumerator RemoveCatchText()
    {
        yield return new WaitForSeconds(catchTextTime);

        catchText.SetActive(false);
    }
}