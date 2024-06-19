using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public GameObject LoadScenePanel;
    public void ButtonDelData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ButtonPlay()
    {
        LoadScenePanel.SetActive(true);
    }
}
