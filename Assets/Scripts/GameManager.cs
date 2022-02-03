using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public HealthController defenderCastle, enemyCastle;

    public UnityEvent OnWin;

    public UnityEvent OnLose;

    public void OnChoseWinner(GameObject go)
    {
        if (go == defenderCastle)
            OnLose?.Invoke();
        else if (go == enemyCastle)
            OnWin?.Invoke();
    }
}
