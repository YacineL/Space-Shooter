﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadScene", 2f);
    }

    // Update is called once per frame
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
