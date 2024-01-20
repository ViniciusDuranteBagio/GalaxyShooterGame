using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaser : MonoBehaviour
{
    [SerializeField]
    private int laserAmount; // first = , second = 11, third = 7, forth = 9
    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;
    [SerializeField]
    private UiManager _uiManager;
    [SerializeField]
    private float _angle = 0f;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        BossDTO bossDTO = generateBossInfo(_uiManager.phase);

        laserAmount = bossDTO.laserAmount;

        InvokeRepeating(bossDTO.firePattern, 10f, bossDTO.fireRate);
    }

    private BossDTO generateBossInfo(int phase)
    {
        //criar o padrão 4 para o ultimo boss
        switch (phase)
        {
            case 1:
                return new BossDTO("FirstFirePattern", 0.2f, 3);
            case 2:
                return new BossDTO("SecondFirePattern", 2f, 11);
            case 3:
                return new BossDTO("ThirdFirePattern", 0.2f, 10);
            case 4:
                //in phase 4 the boss will have tha same attack pattern but it will have a shield for 4 seconds every 10 seconds
                return new BossDTO("ThirdFirePattern", 2f, 10);
            default:
                return new BossDTO("FirstFirePattern", 2f, 3);
        }
    }
    
    private void FirstFirePattern()
    {
        Vector2 laserDir = Vector2.down;

        GameObject laser = LaserPool.laserPullInstace.GetLaser();
        laser.transform.position = transform.position;
        laser.transform.rotation = transform.rotation;
        laser.SetActive(true);
        laser.GetComponent<BossLaser>().SetMoveDirection(laserDir);
    }
  
    private void SecondFirePattern()
    {
        float angleStep = (endAngle - startAngle) / laserAmount;
        float angle = startAngle;

        for (int i = 0; i < laserAmount; i++)
        {
            float laserXDir = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float laserYDir = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 laserMoveVector = new Vector3(laserXDir, laserYDir, 0f);
            Vector2 laserDir = (laserMoveVector - transform.position).normalized;

            GameObject laser = LaserPool.laserPullInstace.GetLaser();
            laser.transform.position = transform.position;
            laser.transform.rotation = transform.rotation;
            laser.SetActive(true);
            laser.GetComponent<BossLaser>().SetMoveDirection(laserDir);

            angle += angleStep;
        }
    }
    private void ThirdFirePattern()
    {
        for (int i = 0; i < laserAmount; i++)
        {
            float laserXDir = transform.position.x + Mathf.Sin(((_angle + 180f * i) * Mathf.PI) / 180f);
            float laserYDir = transform.position.y + Mathf.Cos(((_angle + 180f * i) * Mathf.PI) / 180f);

            Vector3 laserMoveVector = new Vector3(laserXDir, laserYDir, 0f);
            Vector2 laserDir = (laserMoveVector - transform.position).normalized;

            GameObject laser = LaserPool.laserPullInstace.GetLaser();
            laser.transform.position = transform.position;
            laser.transform.rotation = transform.rotation;
            laser.SetActive(true);
            laser.GetComponent<BossLaser>().SetMoveDirection(laserDir);
        }

        _angle += 10f;

        if (_angle >= 360f)
        {
            _angle = 0f;
        }
    }
}
