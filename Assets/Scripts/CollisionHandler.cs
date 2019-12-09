using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] TextMeshProUGUI deathText;
    [SerializeField] float gameOverScreenDelay = 1f;
    void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
        deathFX.SetActive(true);
        StartCoroutine(ShowGameOver());
    }

    private void StartDeathSequence()
    {
        print("Player's dying");
        SendMessage("OnPlayerDeath");
    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(gameOverScreenDelay);
        deathText.text = "You Die";
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

}
