using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public GameObject fireWorks;

    private AudioSource fireworksSound;


    private void Start()
    {
        fireworksSound = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            fireWorks.SetActive(true);
            fireworksSound.Play();
            GameController.SharedInstance.CompleteLevel();

        }
    }
}
