using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float controlSpeed = 4f;
    [Tooltip("In m")] [SerializeField] float xRange = 5f;
    [Tooltip("In m")] [SerializeField] float yRange = 5f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw Based")]
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float controlRollFactor = -20f;

    [Header("Pause / Game Over screen ")]
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] PlayableDirector playableDirector;

    float xThrow, yThrow;
    bool isControlEnabled = true;
    bool isPaused = false;

    private void Start()
    {
        playableDirector.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessPausing();
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    private void ProcessPausing()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused || playableDirector.time == playableDirector.duration)
            {
                gameOverCanvas.SetActive(true);
                //Time.timeScale = 0;
                isPaused = true;
                //AudioListener.pause = true;
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
            }
            else 
            {
                gameOverCanvas.SetActive(false);
                //Time.timeScale = 1;
                isPaused = false;
                //AudioListener.pause = false;
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
        }
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float rawYPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            ActivateGuns();
        }
        else
        {
            DeactivateGuns();
        }
    }

   

    void OnPlayerDeath()
    {
        print("Control's frozen");
        isControlEnabled = false;
    }

    private void ActivateGuns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(true);
        }
    }

    private void DeactivateGuns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
    }
}
