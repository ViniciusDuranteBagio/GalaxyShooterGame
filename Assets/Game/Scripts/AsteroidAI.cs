using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAI : MonoBehaviour, IMovable
{
    public float _speed;
    public int health;
    private GameObject player;
    private float playerXPosition;
    private float moveXposition;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 2.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        moveXposition = CalculateXPositionToMove();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    public void Move()
    {
        transform.Translate(moveXposition * _speed * Time.deltaTime, -1 * _speed * Time.deltaTime, 0);
        if (transform.position.y < -6.22f)
        {
            Destroy(this.gameObject);
        }
    }

    private float CalculateXPositionToMove()
    {
        playerXPosition = player.transform.position.x;
       
        if (playerXPosition > transform.position.x)
        {
            return 1;
        }
        else if (playerXPosition < transform.position.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }

        else if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            Damage();
        }
    }

    public void Damage()
    {
        health--;
        if (health < 1)
        {
            //Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
            //_uiManager.UpdateScore();
            //AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }

}
