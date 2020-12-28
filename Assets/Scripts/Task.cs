using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public delegate void TaskIsEmpty();
    public static event TaskIsEmpty EmptyTask;

    private Text KilledPiece;
    private SpriteRenderer taskImage;

    private int muchToKill;

    private void Start()
    {
        KilledPiece = GetComponent<Text>();
        muchToKill = int.Parse(KilledPiece.text);
        taskImage = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Match3.AddDeathPiece += AddPiece;
    }

    private void OnDisable()
    {
        Match3.AddDeathPiece -= AddPiece;
    }

    private void AddPiece(NodePiece piece)
    {
        Debug.Log(taskImage.sprite.name);

        if (muchToKill > 0)
        {
            if (piece.GetSprite() == taskImage.sprite)
            {
                muchToKill--;
                KilledPiece.text = muchToKill.ToString();
            }
            if (muchToKill <= 0)
            {
                muchToKill = 0;
                EmptyTask();
            }
        }
    }
}
