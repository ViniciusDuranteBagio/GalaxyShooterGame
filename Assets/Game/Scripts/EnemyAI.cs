 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    public float _speed;
    [SerializeField]
    private GameObject _Enemy_ExplosionPrefab;
    UiManager _uiManager;
    [SerializeField]
    private AudioClip _clip;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.22f)
        {
            float randomX = Random.Range(-8.03f, 8.3f);
            transform.position = new Vector3(randomX, 6.22f, 0);
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
        // hitCount++;
        // if (hitCount == 1)
        // {
        //     _engines[0].SetActive(true);
        // }
        // else if (hitCount == 2)
        // {
        //     _engines[1].SetActive(true);
        // }
        health--;
        if (health < 1)
        {
            Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }




}
