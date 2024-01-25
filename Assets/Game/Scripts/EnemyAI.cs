 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IMovable
{
    [SerializeField]
    public float _speed;
    [SerializeField]
    private GameObject _Enemy_ExplosionPrefab;
    UiManager _uiManager;
    [SerializeField]
    private AudioClip _clip;
    public float health;


    public delegate void EnemyDeath();
    public static event EnemyDeath OnEnemyDeath;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    void Update()
    {
        Move();
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
        health = health - damage;
        if (health < 1)
        {
            Death();
            if (OnEnemyDeath != null)
            {
                OnEnemyDeath();
            }
        }
    }

    public void Death() 
    {
        Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
        Destroy(this.gameObject);
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
