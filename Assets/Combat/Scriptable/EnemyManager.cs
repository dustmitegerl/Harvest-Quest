using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public struct DemoEnemy { public EnemyInfo enemyInfo; public int level; }
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    public List<Enemy> currentEnemies;
    [HeaderAttribute("For setting up demo")]
    public DemoEnemy[] demoEnemies;
    private const float LEVEL_MODIFIER = 0.5f;
    #region making it a singleton
    private static EnemyManager _instance;
    public static EnemyManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower().Contains("battle") && currentEnemies.Count == 0)
        {
            GenerateDemo();
        }
        else { Destroy(gameObject); }
    }
    // for demo
    void GenerateDemo()
    {
        Debug.Log("Current enemies list is empty. Generating demo.");
        foreach (DemoEnemy enemy in demoEnemies)
        {
            GenerateEnemyByName(enemy.enemyInfo.EnemyName, enemy.level);
        }
    }

    public void GenerateEnemyByName(string enemyName, int level)
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

    public string spotID;

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


