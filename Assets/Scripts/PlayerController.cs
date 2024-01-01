using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Setup Setting")]
    [Tooltip("How fast ship moves up and down based upon player input")][SerializeField] private float cotrolSpeed = 10f;
    [Tooltip("Haw fast player moves horizontally")][SerializeField] private float xRange = 10f;
    [Tooltip("Haw fast player moves vertically")][SerializeField] private float yRange = 7f;

    [Header("Laser gun array")]
    [Tooltip("Add all player lsers here")][SerializeField] private GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] private float positionYawFactor = 2f;
    [SerializeField] float positonPitchFactor = -2f;


    [Header("Player input based tuning")]
    [SerializeField] private float contraolPitchFactor = -10f;
    [SerializeField] private float controlRollFactor = 5f;


    float xThrow, yThrow;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.elapsedTime > 35f) return;
        ProcessTranslation();
        ProcessRotation();
        ProcessFireing();
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positonPitchFactor;
        float pitchDueToControlThrow = yThrow * contraolPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * cotrolSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * cotrolSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }


    private void ProcessFireing()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActivate)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActivate;
        }
    }

}
