using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{ // reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();

    }

    void SetupBattle() 
    { 
      Instantiate(playerPrefab, playerPosition);
      Instantiate(enemyPrefab, enemyPosition);

    }
}
