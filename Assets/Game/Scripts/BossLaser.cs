using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour, IMovable
{
    private Vector2 _direction;
    private float _speed;

    private void OnEnable()
    {
        Invoke("Destroy", 3f);
    }
    void Start()
    {
        _speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    public void Move()
    {
       transform.Translate(_direction * _speed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag == "EnemyLaser" && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Destroy();
            }
        }
    }
}
