using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string _nameUser;
    private int _stages;
    private int _retry;
    private float _time;
    private static Player _instance;
    public static Player Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
    }

    public UserData DataUser()
    {
        _nameUser = PlayerPrefs.GetString("name");
        _stages = PlayerPrefs.GetInt("stage");
        _retry = PlayerPrefs.GetInt("retry");
        _time = PlayerPrefs.GetFloat("timer");

        UserData user = new UserData(); 
        user.Name = _nameUser;
        user.Stages = _stages;  
        user.RetryAttempt = _retry; 
        user.TotalTime = _time;
        
        return user;
    }
}
