using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public delegate void TaskIsEmpty();
    public static event TaskIsEmpty EmptyTask;

    public Image Done;
    public Image taskImage;

    private Text KilledPiece;

    private int muchToKill;

    private void Start()
    {
        KilledPiece = GetComponent<Text>();
        muchToKill = int.Parse(KilledPiece.text);
    }

    private void OnEnable()
    {
        Match3.AddDeathPiece += AddPiece;
    }

    private void OnDisable()
    {
        // TODO Перевірити, якщо Task вже відписався від події в методі AddPiece, \
        // то видалити цей метод
        Match3.AddDeathPiece -= AddPiece;
    }

    private void AddPiece(NodePiece piece)
    {
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
                Done.gameObject.SetActive(true);
                KilledPiece.text = "";
                EmptyTask();
                Match3.AddDeathPiece -= AddPiece;
            }
        }
    }
}
