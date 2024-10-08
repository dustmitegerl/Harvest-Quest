using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data

    public CharacterStats playerStatus;
    public CharacterStats enemyStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = new CharacterStats();
        enemyStatus = new CharacterStats ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (this.playerStatus.hp > 0) 
        {
            if (other.tag == "Enemy") 
            {
                if (!isAttacked) 
                { 
                 isAttacked = true;
                    setBattleData(other);
                    LevelLoader.instance.LoadLevel("BattleArena");
                }
            }
        }
    }

    private void setBattleData(Collider2D other)
    {
        // Player Data 
        playerStatus.position[0] = this.transform.position.x;
        playerStatus.position[1] = this.transform.position.y;

        // Enemy Data
        CharacterStats status = other.gameObject.GetComponent<EnemyStatus>().enemyStatus;
        enemyStatus.charName = status.charName;
        enemyStatus.characterGameObject = status.characterGameObject.transform.GetChild(0).gameObject;
        enemyStatus.hp = status.hp;
        enemyStatus.maxHP = status.maxHP;
        enemyStatus.mp = status.mp;
        enemyStatus.maxMP = status.maxMP;
    }
}
