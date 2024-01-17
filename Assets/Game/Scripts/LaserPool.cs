using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    public static LaserPool laserPullInstace;

    [SerializeField]
    private GameObject pooledLaser;
    private bool notEnoughLaserInPool = true;

    private List<GameObject> lasers;

    void Awake()
    {
        laserPullInstace = this;
    }

    void Start()
    {
        lasers = new List<GameObject>();
    }

    public GameObject GetLaser() 
    {
        if (lasers.Count > 0)
        {
            for (int i = 0; i < lasers.Count; i++)
            {
                if (!lasers[i].activeInHierarchy)
                {
                    return lasers[i];
                }
            }
        }

        if (notEnoughLaserInPool)
        {
            GameObject laser = Instantiate(pooledLaser);
            laser.SetActive(false);
            lasers.Add(laser);
            return laser;
        }

        return null;
    }
}
