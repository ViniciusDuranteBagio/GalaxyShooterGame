using System;
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

    private float damagePower;

    public int cristals;

    private SpanwManager _spanwManager;
    private AudioSource _audioSource;
    private UiManager _uiManager;
    private GameManager _gameManager;

    private int hitCount = 0;

    public bool isPlayerAlive = true;
    public bool canTripleShot = false;
    public bool shieldPowerUp = false;

    public delegate void DamagedPlayerHandler(int life);
    public static event DamagedPlayerHandler DamagedPlayer;
    
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler PlayerDied;
    

    private void Start()
    {

        damagePower = 1f;
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        if(_uiManager != null)
        {
            //passasr essa linha de baixo para enventp
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
            if (IsTripleShotActive())
            {
                GameObject laser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                float damagePower = GetDamagePower();
                laser.GetComponentInChildren<Laser>().SetLaserDamage(damagePower);
            }
            else
            {
                GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                float damagePower = GetDamagePower();
                laser.GetComponent<Laser>().SetLaserDamage(damagePower);
            }
            
            _canFire = Time.time + _fireRate;
        }
    }
    
    public bool IsTripleShotActive()
    {
        return canTripleShot;
    }
  
    public void Damage()
    {
        if(shieldPowerUp)
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
        if (DamagedPlayer != null)
        {
            DamagedPlayer(_life);
        }

        if (_life < 1)
        {
            Instantiate(_Explosion_PlayerPrefab, transform.position, Quaternion.identity);
            isPlayerAlive = false;
            Destroy(this.gameObject);
            
            if (PlayerDied != null)
            {
                PlayerDied();
            }
        }
    }
    public void EnableShields()
    {
        shieldPowerUp = true;
        _shieldGameObject.SetActive(true);
    }
    public void TripleShootPowerUpOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
   
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        canTripleShot = false;
    }

    private float GetDamagePower()
    {
        if (IsTripleShotActive())
        {
            return damagePower * 3;
        } 
        return damagePower;
    }

    public void addCristal()
    {
        cristals++;
    }

    public void SubCristal()
    {
        cristals--;
    }

    public void IncreaseDamage()
    {
        damagePower++;
        Debug.Log(damagePower);
    }

    public void IncreaseAttackSpeed()
    {
        _fireRate -= 0.16f;
    }

    public void IncreaseSpeed()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IncreaseSpeed();
    }

        


}


