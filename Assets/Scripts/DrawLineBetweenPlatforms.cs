using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class DrawLineBetweenPlatforms : MonoBehaviour
{
    public LineRenderer lineRend;

    private GameObject[] platforms;

    private Vector3[] platformPositions;

    private void Awake()
    {
        platforms = GameObject.FindGameObjectsWithTag("Bundle").OrderBy(g => {
            return Convert.ToInt32(g.name.Split('-').Last());
        }).ToArray(); 
    }

    private void Start()
    {
        platformPositions = GetPositions(platforms);

        lineRend.positionCount = platformPositions.Length;

        lineRend.SetPositions(platformPositions); //vector 3 position array
    }

    private Vector3[] GetPositions(GameObject[] _platforms)
    {
        Vector3[] platformsPosition = new Vector3[_platforms.Length];

        for (int i = 0; i < _platforms.Length; i++)
        {
            platformsPosition[i] = _platforms[i].transform.position;
        }

        return platformsPosition;
    }
}















