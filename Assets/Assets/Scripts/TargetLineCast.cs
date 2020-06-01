using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLineCast : MonoBehaviour
{
    private LineRenderer targetLine;
    public GameObject greenIndicator;
    public GameObject redIndicator;

    private bool onTarget;


    private void Start()
    {
        greenIndicator.SetActive(true);
        targetLine = GetComponent<LineRenderer>();
    }   



private void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit))
        {
            if (hit.collider)
            {
                targetLine.SetPosition(1, hit.point - transform.position);
                greenIndicator.SetActive(false);
                redIndicator.SetActive(true);

                if (hit.collider.CompareTag("Platform") || hit.collider.CompareTag("Blue Platform"))
                {
                    targetLine.SetPosition(1, hit.point - transform.position);
                    redIndicator.SetActive(false);
                    greenIndicator.SetActive(true);
                }
            }


        }
        else
        {
            targetLine.SetPosition(1, -transform.up * 5000);
            greenIndicator.SetActive(false);
            redIndicator.SetActive(true);
        }
    }
}
