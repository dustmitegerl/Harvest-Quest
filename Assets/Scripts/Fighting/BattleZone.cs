using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Get ready to battle!");
            StartBattle();
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("You left the battle!");
            EndBattle();
        }
    }

    void StartBattle() 
    {
        SceneManager.LoadScene("BattleArena");
        Debug.Log("Battle Started!");
    }

    void EndBattle() 
    {
        SceneManager.LoadScene("Harvest-Quest");
        Debug.Log("Battle Ended!");
    }
}
