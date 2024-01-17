using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    private Vector3 direction;
    private float _laserDamage;
   
    void Update()
    {
        MoveLaser();
    }

    public float GetLaserDamage()
    {
        return _laserDamage;
    }

    public void SetLaserDamage(float damage)
    {
        _laserDamage = damage;
    }

    private void MoveLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 6 || transform.position.y <= -6)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
} 


