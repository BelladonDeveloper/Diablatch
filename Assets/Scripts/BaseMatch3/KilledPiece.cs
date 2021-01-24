using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    public bool falling;

    //int todo = 64; // TODO
    float speed = 32f;
    float gravity = 64f;
    Vector2 moveDir;
    RectTransform rect;
    Image img;

    private readonly float blockSize = Match3.blockSize;

    public void Initialize(Sprite piece, Vector2 start)
    {
        falling = true;

        moveDir = Vector2.up;
        moveDir.x = Random.Range(-1.0f, 1.0f);
        moveDir *= speed / 2;

        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        img.sprite = piece;
        rect.anchoredPosition = start;
    }

    void Update()
    {
        if (!falling) return;
        moveDir.y -= Time.deltaTime * gravity;
        moveDir.x = Mathf.Lerp(moveDir.x, 0, Time.deltaTime);
        rect.anchoredPosition += moveDir * Time.deltaTime * speed;
        if (rect.position.x < -blockSize || rect.position.x > Screen.width + blockSize || 
            rect.position.y < -blockSize || rect.position.y > Screen.height + blockSize)
            falling = false;
    }
}
