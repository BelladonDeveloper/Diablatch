using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public GameObject[] MenuPanels;
    public Image ButtonActiveImage;
    public Image MenuTransform;

    private RectTransform rectTransform;
    private Vector2 moveDir;

    public static bool lift = true;
    private bool fall = false;
    private bool[] isActiveMenu = { false, false, true, false, false };
    private float speed = 3200f;
    private float widthCenter;
    private float heightCenter;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        moveDir = Vector2.up;
        widthCenter = Screen.width / 2;
        heightCenter = Screen.height / 2;
    }

    private void Update()
    {
        if (lift)
        {
            rectTransform.anchoredPosition += moveDir * speed * Time.deltaTime;

            if (rectTransform.position.y >= heightCenter)
            {
                rectTransform.position = new Vector3(widthCenter, heightCenter, 0);
                lift = false;
            }
        }

        if (fall)
        {
            moveDir = Vector2.down;
            rectTransform.anchoredPosition += moveDir * speed * Time.deltaTime;

            if (rectTransform.position.y <= -heightCenter)
            {
                rectTransform.position = new Vector3(widthCenter, -heightCenter, 0);
                fall = false;
                moveDir = Vector2.up;
            }
        }
    }

    public void OnPlayClick()
    {
        fall = true;
    }

    public void OnNavigatorButtonClick()
    {
        var thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int index = int.Parse(thisButton.name);
        //float offset = MenuTransform.preferredHeight; Debug.Log(offset);
        ButtonActiveImage.transform.position = 
            new Vector2(thisButton.transform.position.x, ButtonActiveImage.transform.position.y);

        for (int i = 0; i < MenuPanels.Length; i++)
        {
            if (isActiveMenu[i])
            {
                if (index != i)
                {
                    RectTransform rectCurrent = MenuPanels[i].GetComponent<RectTransform>();
                    RectTransform rectNext = MenuPanels[index].GetComponent<RectTransform>();
                    float startPossition = rectNext.position.x;
                    StartCoroutine(MovePanel(index, i, Vector2.left, rectCurrent, rectNext, startPossition));
                    isActiveMenu[i] = false;
                }
            }
        }
    }

    IEnumerator MovePanel(int index, int i, Vector2 direction, RectTransform rectCurrent, RectTransform rectNext, float startPossition)
    {
        while (rectCurrent.position.x >= widthCenter * direction.x)
        {
            rectCurrent.anchoredPosition += direction * speed / 100;
            rectNext.anchoredPosition += direction * speed / 100;
            yield return null;
        }

        if (rectCurrent.position.x <= widthCenter * direction.x)
        {
            rectCurrent.position = new Vector3(startPossition, heightCenter, 0);
            rectNext.position = new Vector3(widthCenter, heightCenter, 0);
            isActiveMenu[index] = true;
            StopCoroutine("MovePanel");
            yield return null;
        }
    }
}
