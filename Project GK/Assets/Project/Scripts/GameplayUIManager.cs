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
        string healthText = gameInfo.currentHealth + "/" + gameInfo.maxHealth;
        health.text = healthText;

        money.text = gameInfo.money.ToString();

        string levelText = gameInfo.currentStadiumLevel + "/" + gameInfo.levelsPerStadium;
        level.text = levelText;

        string shotText = gameManager.currentShot + "/" + gameInfo.shotsPerLevel;
        shot.text = shotText;
    }
}