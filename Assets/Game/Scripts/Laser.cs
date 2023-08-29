using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    private Vector3 direction;



    void Start()
    {
        if (this.tag == "Laser")
        {
            direction = Vector3.up;
        }
        
        if(this.tag == "EnemyLaser") 
        { 
            direction = Vector3.down; 
        }
    }

   
    void Update()
    {
        MoveLaser(direction);
    }

    private void MoveLaser(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 6 || transform.position.y <= -6)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag == "EnemyLaser" && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
} 


