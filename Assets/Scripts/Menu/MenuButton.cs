using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject menuPanel;

    private void OnMouseUp()
    {
        MenuNavigator.lift = true;
    }
}
