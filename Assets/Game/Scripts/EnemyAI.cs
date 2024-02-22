 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IMovable
{
    [SerializeField]
    public float _speed;
    [SerializeField]
    private GameObject _Enemy_ExplosionPrefab;
    [SerializeField]
    private AudioClip _clip;
    public float health;

    public delegate void EnemyDeadHandler();
    public static event EnemyDeadHandler EnemyDied;
    // fazer padrão singleton para ver se podemos mudar esse evento de statico para não statico
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }

        else if (other.CompareTag("Laser"))
        {
            float damage = other.GetComponent<Laser>().GetLaserDamage();

            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            
            Damage(damage);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health < 1) 
        {
            Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            if (EnemyDied != null)
            {
                EnemyDied();
            }
            Destroy(this.gameObject);
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.22f)
        {
            float randomX = Random.Range(-8.03f, 8.3f);
            transform.position = new Vector3(randomX, 6.22f, 0);
        }
    }
}
