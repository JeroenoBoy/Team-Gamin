using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public HealthController DefenderCastle, EnemyCastle;

    public UnityEvent OnWin;

    public UnityEvent OnLose;

    public void OnChoseWinner(GameObject go)
    {
        if (go == DefenderCastle)
            OnLose?.Invoke();
        else if (go == EnemyCastle)
            OnWin?.Invoke();
    }
}
