using UnityEngine;
using UnityEngine.UI;
using System;

public class PlatformLogic : MonoBehaviour
{
    public Material[] material;

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
        /* 3 conditions: first: player tag
                         second: jumpcounts
                         third:  location of collision on platform*/

        
        
    }

    private void UpdatePlatform(Collision _collider)
    {
        if (jumpCount == 0) // first hit
        {
            _renderer.material = material[0]; // first crack

            jumpCount++;

            LocateCollision(_collider);



        }
        else if (jumpCount >= 1) // second hit
        {
            _renderer.material = material[1]; // second crack

            jumpCount++;
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
}
