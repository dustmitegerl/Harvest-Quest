using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//ref https://www.gamedev.tv/courses/unity-2-5d-turn-based-combat/returning-to-the-overworld/5932
public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI LevelText;

    private int currHealth;
    private int maxHealth;
    private int level;

    private const string LEVEL_ABB = "Lvl: ";
    
    // Start is called before the first frame update
    void Start()
    {
        //SetStartingValues(10,10,5);
    }

    // Update is called once per frame
    public void SetStartingValues(int maxHealth, int currHealth, int level)
    {
        this.maxHealth = maxHealth;
        this.currHealth = currHealth;
        this.level = level;
        LevelText.text = LEVEL_ABB + this.level.ToString();
        UpdateHealthBar();
    }

    public void ChangeHealth(int currHealth)
    {
        this.currHealth = currHealth;
        if(currHealth<=0)
        {
            //death animation
            Destroy(gameObject,1f);
        }
        UpdateHealthBar();
        //if health is 0 :death animation + destroy battle visual
    }

    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;
    }
}
