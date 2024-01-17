using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDTO
{
    public string firePattern { get; }
    public float fireRate { get; }
    public int laserAmount { get; }


    public BossDTO(string firePattern, float fireRate, int laserAmount)
    {
        this.firePattern = firePattern;
        this.fireRate = fireRate;
        this.laserAmount = laserAmount;
    }

}
