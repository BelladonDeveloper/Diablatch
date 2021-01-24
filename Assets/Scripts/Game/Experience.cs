using System;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public Image fillImage;

    private Text textAmount;
    private Vector3 fillPosition;

    private float ScreenWidthIndex;
    private int countExperience;
    private int levelExperience;

    void Start()
    {
        textAmount = GetComponent<Text>();
        countExperience = PlayerPrefs.GetInt("ExpAmount", 0);
        textAmount.text = (Math.Truncate((double)countExperience / 50)).ToString();

        fillPosition = fillImage.transform.position;
        ScreenWidthIndex = fillPosition.x / 3.66f;
    }

    private void OnEnable()
    {
        Match3.AddDeathPiece += AddExperience;
    }

    private void OnDisable()
    {
        Match3.AddDeathPiece -= AddExperience;
    }

    private void AddExperience(NodePiece nodePiece)
    {
        countExperience++;

        if (countExperience <= 50)
        {
            levelExperience = 1;
        }
        else
        {
            levelExperience = (int)Math.Truncate((double)countExperience / 50);
        }

        textAmount.text = levelExperience.ToString();
        PlayerPrefs.SetInt("ExpAmount", countExperience);

        UpdateFillImage();
    }

    private void UpdateFillImage()
    {
        float fillPercent = countExperience % 50 / 100f;

        fillImage.rectTransform.localScale = new Vector3(fillPercent * 2, 1, 1);

        fillImage.transform.position = new Vector3
            (fillPosition.x + (ScreenWidthIndex * fillPercent),
                fillPosition.y, fillPosition.z);

        //Debug.Log(fillPercent);
        Debug.Log("Experience: " + countExperience);
    }
}
