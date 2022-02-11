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

    public UnityEvent OnWin;
    public UnityEvent OnLose;

    public void OnChoseWinner(GameObject go)
    {
        if (go == DefenderCastle)
            OnLose?.Invoke();
        else if (go == EnemyCastle)
            OnWin?.Invoke();

        StartCoroutine(MainMenu());
    }


    private IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(_mainMenuScene);
    }
}
