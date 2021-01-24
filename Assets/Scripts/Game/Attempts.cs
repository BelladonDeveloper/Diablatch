using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attempts : MonoBehaviour
{
    public delegate void MissingAttempts();
    public static event MissingAttempts AttemptsIsOver;

    private Text textCounter;
    private int attemptsCounter = 9;

    void Start()
    {
        attemptsCounter = PlayerPrefs.GetInt("AttemptsCounter", 9);
        textCounter = GetComponentInChildren<Text>();
        textCounter.text = attemptsCounter.ToString();
    }

    private void OnEnable()
    {
        Match3.LooseMove += SubtractHearts;
    }
    private void OnDisable()
    {
        Match3.LooseMove -= SubtractHearts;
    }

    private void SubtractHearts()
    {
        Debug.Log(attemptsCounter);
        attemptsCounter--;

        if (attemptsCounter <= 0)
        {
            AttemptsIsOver();
        }
        else
        {
            PlayerPrefs.SetInt("AttemptsCounter", attemptsCounter);
            textCounter.text = attemptsCounter.ToString();
        }
    }
}
