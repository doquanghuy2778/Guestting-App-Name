using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetName : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    private int _isGetName;
   
    public void GetNameUser()
    {
        string namePlayer = inputField.text;
        PlayerPrefs.SetString("name", namePlayer);
        _isGetName = 1;
        PlayerPrefs.SetInt("IsGetName", _isGetName);
        PlayerPrefs.Save();
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
