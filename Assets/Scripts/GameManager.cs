using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _mainMenuScene;
    
    public GameObject DefenderCastle, EnemyCastle;

    [Header("Events")]
    public UnityEvent OnWin;
    public UnityEvent OnLose;

    /// <summary>
    /// chose a winner based on the gameobject in the parameter
    /// </summary>
    /// <param name="go"></param>
    public void OnChoseWinner(GameObject go)
    {
        if (go == DefenderCastle)   //When you lose
            OnLose?.Invoke();
        else if (go == EnemyCastle) //When you win
            OnWin?.Invoke();

        StartCoroutine(MainMenu());
    }

    /// <summary>
    /// Wait a bit and then go back to the main menu
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(_mainMenuScene);
    }
}
