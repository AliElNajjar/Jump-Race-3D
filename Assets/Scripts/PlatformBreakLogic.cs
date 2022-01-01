using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlatformBreakLogic : MonoBehaviour
{
    public Material[] material;
    
    public GameObject banner;
    public GameObject rubble;

    private Renderer _renderer;
    private float jumpCount = 0;
     private AudioSource smash;

    private Vector3 centerOfPlatform;
    private float distance;
    private Vector3 hitLocation;

    private float numberOfPlatforms;
    private int platformNumber;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        centerOfPlatform = transform.position;
        numberOfPlatforms = GameObject.FindGameObjectsWithTag("Platform").Length * 1.0f;
        platformNumber = Convert.ToInt32(transform.parent.GetComponentInChildren<Text>().text);
        smash = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            UpdatePlatform(collider);
            UpdateProgress();
            smash.Play();
        }
    }

    private void UpdatePlatform(Collision _collider)
    {
        if (jumpCount == 0) // first hit
        {
            _renderer.material = material[0]; // first crack
            //_renderer.material.mainTexture = change Texture.

            jumpCount++;

            LocateCollision(_collider);

        }
        else if (jumpCount == 1) // second hit
        {
            _renderer.material = material[1]; // second crack

            jumpCount++;
        }
        else if (jumpCount >= 2) // second hit
        {
            StartCoroutine(DestroyPlatform());
        }
    }

    private void UpdateProgress()
    {
        float progressFloat = (numberOfPlatforms - platformNumber) / numberOfPlatforms;
        GameController.SharedInstance.UpdateProgressUI(progressFloat);

    }

    private void LocateCollision(Collision collider)
    {
        hitLocation = collider.transform.position;
        distance = Vector3.Distance(hitLocation, centerOfPlatform);

        if (distance <= 0.4)
        {
            GameController.SharedInstance.JumpComment("bullsEye");
        }
        else if (distance > 0.4 && distance <= 0.75)
        {
            GameController.SharedInstance.JumpComment("normalJump");
        }

        //Debug.Log(distance);

    }

    IEnumerator DestroyPlatform()
    {
        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
        banner.SetActive(false);
        rubble.SetActive(true);
    }


}
