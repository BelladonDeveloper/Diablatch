using UnityEngine;

public class TransformToScreenCenter : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
}
