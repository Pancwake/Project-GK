using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    [Header("BaseStats")]
    [SerializeField] int baseHealth;
    [SerializeField] int baseGoalHealthPenalty;
    [SerializeField] int baseRepelPoints;
    [SerializeField] int baseCatchPoints;
    [SerializeField] float basePointsMultiplierIncrease;

    [Header("GameStats")]
    public int goalHealthPenalty;
    public int repelPoints;
    public int catchPoints;
    public float pointsMultiplierIncrease;

    [Header("Player Stats")]
    public int maxHealth;
    public int currentHealth;
    public int points;
    public int combo;
    public float pointsMultiplier;

    public void CalculateMultiplier()
    {
        if (combo <= 0) //Dont do anything if no combo
            return;

        //1.2 = 1 + (0.1 * (3 - 1)) 
        pointsMultiplier = 1 + (pointsMultiplierIncrease * (combo - 1)); //combo -1 so it starts at the second hit
    }

    public void ResetStats()
    {
        maxHealth = baseHealth;
        goalHealthPenalty = baseGoalHealthPenalty;
        repelPoints = baseRepelPoints;
        catchPoints = baseCatchPoints;
        pointsMultiplierIncrease = basePointsMultiplierIncrease;

        currentHealth = maxHealth;
        points = 0;
        combo = 0;
        pointsMultiplier = 1;
    }
}