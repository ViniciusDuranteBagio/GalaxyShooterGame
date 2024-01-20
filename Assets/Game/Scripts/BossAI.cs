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
    private GameObject _shieldGameObject;
    [SerializeField]
    private AudioClip _clip;
    public float health;
    public bool ShouldMoveToLeft = true;
    public GameObject _bossLaserPrefab;
    private bool _shield;

    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        Debug.Log(_uiManager.phase);

        if (true)
        {
            InvokeRepeating(nameof(ShieldPowerOn), 10f, 14f);
        }
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
        if (other.tag == "Laser")
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
        if (!IsOnStartPosition() || _shield)
        {
            return;
        }

        health -= damage;
        _uiManager.UpdateHealthSlider(health);
        if (health < 1)
        {
            Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            _uiManager.KilledTheBoss();
            Destroy(this.gameObject);
        }
    }
    
    private void ShieldPowerOn()
    {
        _shield = true;
        _shieldGameObject.SetActive(true);
        Invoke(nameof(ShieldPowerDown), 4f);
    }

    private void ShieldPowerDown()
    {
        _shield = false;
        _shieldGameObject.SetActive(false);
    }
}
