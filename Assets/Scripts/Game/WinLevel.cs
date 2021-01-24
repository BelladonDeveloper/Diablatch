using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{
    public Task[] tasks;
    public GameObject LooseScreen;

    public int nextLevel;

    private int muchTasks;

    void Start()
    {
        muchTasks = tasks.Length;
    }

    private void OnEnable()
    {
        Task.EmptyTask += FinishedLevelByTasks;
        Attempts.AttemptsIsOver += FinishedLevelByAttempts;
    }

    private void OnDisable()
    {
        Task.EmptyTask -= FinishedLevelByTasks;
        Attempts.AttemptsIsOver -= FinishedLevelByAttempts;
    }

    private void FinishedLevelByTasks()
    {
        muchTasks--;
        if (muchTasks <= 0)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    private void FinishedLevelByAttempts()
    {
        // TODO Створити екран програшу
        // Продумати блокування гри через подію Attempts.AttemptsIsOver
        LooseScreen.SetActive(true);
    }
}
