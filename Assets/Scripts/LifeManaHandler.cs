using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeManaHandler : MonoBehaviour
{ // reference for the script: https://www.youtube.com/watch?v=WXUsJnupiPQ

    public Slider HPBar;
    public Slider MPBar;
    public Slider enemyHPBar;

    public TextMeshProUGUI HPText;
    public TextMeshProUGUI MPText;
    public TextMeshProUGUI enemyHPText;


    public float myHP;
    public float myMP;
    private float currentHP;
    private float currentMP;
    
    public float attackDamage;
    public float manaCost;


    public float enemyHP;
    private float currentenemyHP;

    public float magicAttackDamage;
  
    // Start is called before the first frame update
    void Start()
    {
        currentHP = myHP;
        currentMP = myMP;
        currentenemyHP = enemyHP;

        enemyHPBar.maxValue = enemyHP;
        HPBar.maxValue = myHP;
        MPBar.maxValue = myMP;
        
        enemyHPBar.value = enemyHP;
        HPBar.value = myHP;
        MPBar.value = myMP;
        
    }

    void Update()
    {
        enemyHPBar.value = currentenemyHP;
        //HPBar.value = currentHP;
        MPBar.value = currentMP;
        
        enemyHPText.text = "" + (int)currentenemyHP;
        HPText.text = "" + (int)currentHP;
        MPText.text = "" + Mathf.FloorToInt(currentMP);
        
        if (currentMP < myMP) 
        {
            currentMP = Mathf.MoveTowards(currentMP, myMP, Time.deltaTime * 0.01f);
        }

        if (currentMP < 0) 
        {   
          currentMP = 0;
        }
        
        
    }

    public IEnumerator AttackEnemy() 
    {
        UpdateEnemyUI();
        Damage(attackDamage);

        yield return new WaitForSeconds(2f);
        
    }

    public void UseSkill() 
    {
        
           if (currentMP >= manaCost)
           {
               UpdateEnemyUI();
               Damage(magicAttackDamage);
               ReduceMP(manaCost);
           }
           else 
           {
               Debug.Log("You don't have enough SP to attack!");
           }

    }

    public bool TakeDamage(float damage) 
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, myHP);

        HPBar.value = currentHP;
        HPText.text = "" + (int)currentHP;

        if (currentHP <= 0) 
        {
            return true;
        }
            return false;

        if (currentenemyHP <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
        /*{
            Debug.Log("Game Over");
        }*/
    }

    public void Damage(float damage) 
    {
        currentenemyHP -= damage;
        currentenemyHP = Mathf.Clamp(currentenemyHP, 0, enemyHP);

        UpdateEnemyUI();
    }
  
    public void ReduceMP(float mp) 
    {
        currentMP -= mp;
        currentMP = Mathf.Clamp(currentMP, 0, myMP);

        MPBar.value = currentMP;
        MPText.text = "" + Mathf.FloorToInt(currentMP);
    }

    private void UpdateEnemyUI() 
    {
        enemyHPBar.value = currentenemyHP;
        enemyHPText.text = "" + (int)currentenemyHP;
    }
}
