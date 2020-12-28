using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{
    public Task[] tasks;

    public int nextLevel;

    private int muchTasks;

    void Start()
    {
        muchTasks = tasks.Length;
    }

    private void OnEnable()
    {
        Task.EmptyTask += FinishedLevel;
    }

    private void OnDisable()
    {
        Task.EmptyTask -= FinishedLevel;
    }

    private void FinishedLevel()
    {
        muchTasks--;
        if (muchTasks <= 0)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
