using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _Explosion_PlayerPrefab;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldGameObject;
    [SerializeField]
    private GameObject[] _engines;
    [SerializeField]
    private int _life = 3;
    [SerializeField]
    private float _fireRate = 0.32f;
    [SerializeField]
    private float _canFire = 0.0f;
    [SerializeField]
    private float _speed = 5.0f;

    private SpanwManager _spanwManager;
    private AudioSource _audioSource;
    private UiManager _uiManager;
    private GameManager _gameManager;

    private int hitCount = 0;

    public bool isPlayerAlive = true;
    public bool canTripleShot = false;
    public bool speedBoostPowerUp = false;
    public bool shieldPowerUp = false;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        if(_uiManager != null)
        {
            _uiManager.UpdateLives(_life);
        }
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spanwManager = GameObject.Find("Spanw_Manager").GetComponent<SpanwManager>();
        if (_spanwManager != null)
        {
            _spanwManager.StartSpawnRoutine();
        }
        _audioSource = GetComponent<AudioSource>();
        hitCount = 0;
    }
    private void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();

        }

    }
    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (canTripleShot == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.79f, 0), Quaternion.identity);
            }
            _canFire = Time.time + _fireRate;
        }
    }
    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (speedBoostPowerUp == true)
        {
            transform.Translate(Vector3.right * 2.0f * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * 2.0f * _speed * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        if (transform.position.x > 8.6f)
        {
            transform.position = new Vector3(-8.6f, transform.position.y, 0);
        }
        else if (transform.position.x < -8.6f)
        {
            transform.position = new Vector3(8.6f, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        if(shieldPowerUp == true)
        {
            shieldPowerUp = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        hitCount++;
        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }
        _life--;
        _uiManager.UpdateLives(_life);


        if (_life < 1)
        {
            Instantiate(_Explosion_PlayerPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            isPlayerAlive = false;
            Destroy(this.gameObject);
        }
    }
    public void EnableShields()
    {
        shieldPowerUp = true;
        _shieldGameObject.SetActive(true);
    }
    public void TipleShootPowerUpOn()
    {
        canTripleShot = true;

        StartCoroutine(TripleShotPowerDownRoutine());


    }
    public void SpeedBoostPowerUpOn()
    {
        speedBoostPowerUp = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        canTripleShot = false;
    }
    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speedBoostPowerUp = false;
    }
    // when the game is not started show game title 
    // when the game started isntantiate player 
    //when the player dies clear score and show title leting the player restart the game
    //make player a prefab
    
}


