using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayClick : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        Debug.Log("Click_Play");

        GetComponent<AudioSource>().Play();
        transform.localScale = new Vector3(0.7f, 0.7f, 0);
        SceneManager.LoadScene(1);

        Time.timeScale = 1;
    }
}
