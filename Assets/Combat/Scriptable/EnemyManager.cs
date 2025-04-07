using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;

    private const float LEVEL_MODIFIER = 0.5f;

    private void Awake()
    {


        GenerateEnemyByName("Lemon", 1);
        GenerateEnemyByName("Lemon", 2);
        GenerateEnemyByName("Lemon", 3);
    }

    private void GenerateEnemyByName(string enemyName, int level)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (enemyName == allEnemies[i].EnemyName)
            {
                Enemy newEnemy = new Enemy();
                newEnemy.EnemyName = allEnemies[i].EnemyName;
                newEnemy.Level = level;

                float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);

                // Health & core stats with scaling
                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth + (allEnemies[i].BaseHealth * levelModifier));
                newEnemy.CurrHealth = newEnemy.MaxHealth;

                newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStr + (allEnemies[i].BaseStr * levelModifier));
                newEnemy.Intelligence = Mathf.RoundToInt(allEnemies[i].BaseIntelligence + (allEnemies[i].BaseIntelligence * levelModifier));
                newEnemy.Defense = Mathf.RoundToInt(allEnemies[i].BaseDefense + (allEnemies[i].BaseDefense * levelModifier));
                newEnemy.Resistance = Mathf.RoundToInt(allEnemies[i].BaseResistance + (allEnemies[i].BaseResistance * levelModifier));

                newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative + (allEnemies[i].BaseInitiative * levelModifier));

                // SP (could be left flat or scaled if needed)
                newEnemy.CurrSP = allEnemies[i].BaseSP;
                newEnemy.MaxSP = allEnemies[i].BaseSP;

                // Skills & visuals
                newEnemy.BasicSkill = allEnemies[i].BasicSkill;
                newEnemy.SpecialSkill = allEnemies[i].SpecialSkill;
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;

                currentEnemies.Add(newEnemy);
                return;
            }
        }
    }


    public List<Enemy> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}
[System.Serializable]
public class Enemy
{
    public string EnemyName;

    public int CurrHealth;
    public int MaxHealth;

    public int Strength;
    public int Intelligence;
    public int Defense;
    public int Resistance;

    public int Initiative;
    public int Level;

    public int CurrSP;
    public int MaxSP;

    public GameObject EnemyVisualPrefab;

    public Skill BasicSkill;
    public Skill SpecialSkill;
}


