using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI LevelText;

    private int currHealth;
    private int maxHealth;
    private int level;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private const string LEVEL_ABB = "Lvl: ";

    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DEAD_PARAM = "IsDead";
    private const string IS_SPECIAL_PARAM = "IsSpecial";

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

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

        if (currHealth <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1f);
        }
        else
        {
            PlayHitAnimation();
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;
    }

    public void PlayAttackAnimation()
    {
        if (anim != null)
        {
            DebugTrigger(IS_ATTACK_PARAM);
            anim.SetTrigger(IS_ATTACK_PARAM);
        }
    }


    public void PlayHitAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger(IS_HIT_PARAM);
        }
    }

    public void PlayDeathAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger(IS_DEAD_PARAM);
        }
    }

    public void PlaySpecialAttackAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger(IS_SPECIAL_PARAM);
        }
    }

    public void DebugTrigger(string trigger)
    {
        Debug.Log($"[Animator Triggered] {trigger}");
    }

    public void Highlight(bool on)
    {
        if (spriteRenderer != null)
        {
            if (on)
                spriteRenderer.color = Color.yellow;
            else
                spriteRenderer.color = originalColor;
        }
    }

}
