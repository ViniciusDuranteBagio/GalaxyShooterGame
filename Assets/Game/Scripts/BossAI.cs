using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    [SerializeField]
    public float _speed;
    [SerializeField]
    private GameObject _Enemy_ExplosionPrefab;
    UiManager _uiManager;
    [SerializeField]
    private AudioClip _clip;
    public int health;
    public bool ShouldMoveToLeft = true;
    [SerializeField]
    private float _fireRate = 0.40f;
    [SerializeField]
    private float _canFire = 0.0f;
    public GameObject _bossLaserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AIMovements();
    }
    private void AIMovements()
    {
        if (IsOnStartPosition())
        {
            _uiManager.SetDangerScreenUnactive();
            MovementBoss();
            Shoot();
        }
        else
        {
            if (!_uiManager.IsDangerScreenActive())
            {
                _uiManager.SetDangerScreenActive();
            }
            MoveToInitialPosition();
        }
    }
    private bool IsOnStartPosition()
    {
        return transform.position.y <= 3.92f;
    }
    private void MoveToInitialPosition()
    {
        float sliderValue = _uiManager.slider.value;
        if (sliderValue < 100f)
        {
            _uiManager.slider.value++;
        }
            transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
    }
    private void MovementBoss()
    {
        if (transform.position.x > 6.10f)
        {
            ShouldMoveToLeft = true;
        }
        if (transform.position.x < -6.30f)
        {
            ShouldMoveToLeft = false;
        }

        Move(ShouldMoveToLeft);
    }
    private void Move(bool shouldGoToLeft)
    {
        if (shouldGoToLeft)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
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
        //fazer anima��o dele perdendo motor quando for chegar perto de 
        if (!IsOnStartPosition())
        {
            return;
        }
        health--;
        _uiManager.UpdateHealthSlider(health);
        if (health < 1)
        {
            Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            _uiManager.KilledTheBoss();
            Destroy(this.gameObject);
        }
    }
    private void Shoot()
    {
        if (Time.time > _canFire)
        {
           Instantiate(_bossLaserPrefab, transform.position - new Vector3(0, 1.4f, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }

}
