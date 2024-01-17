using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private bool _speedBoostPowerUp = false;

    private const float RIGHT_lIMIT = 8.6f;
    private const float LEFT_lIMIT = -8.6f;
    private const float TOP_LIMIT = 0;
    private const float BOTTOM_LIMIT = -4.2f;

    void Start()
    {
        PowerUp.OnSpeedBoostPowerUp += SpeedBoostPowerUpOn;
    }

    void Update()
    {
        Move();
    }

    public void IncreaseSpeed()
    {
        _speed += 1.25f;
    }

    public void Move()
    {
        float _multiplierSpeed = GenerateMutiplierSpeed();
        MoveHorizontaly(_multiplierSpeed);
        MoveVerticaly(_multiplierSpeed);
        BlockLimitScreen();
        MoveSideToSide();
    }

    private void MoveHorizontaly(float _multiplierSpeed) 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * _multiplierSpeed * _speed * horizontalInput * Time.deltaTime);
    }

    private void MoveVerticaly(float _multiplierSpeed)
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * _multiplierSpeed * _speed * verticalInput * Time.deltaTime);
    }

    private float GenerateMutiplierSpeed()
    {
        float multiplierSpeedBase = 1.0f;

        if (HasSpeedBoost())
        {
            multiplierSpeedBase = 2.0f;
        }

        return multiplierSpeedBase;
    }

    private bool HasSpeedBoost()
    {
       return _speedBoostPowerUp == true;
    }

    void BlockLimitScreen() 
    {
        if (transform.position.y > TOP_LIMIT)
        {
            transform.position = new Vector3(transform.position.x, TOP_LIMIT, 0);
        }
        else if (transform.position.y < BOTTOM_LIMIT)
        {
            transform.position = new Vector3(transform.position.x, BOTTOM_LIMIT, 0);
        }
    }

    void MoveSideToSide() 
    {
        if (transform.position.x > RIGHT_lIMIT)
        {
            transform.position = new Vector3(LEFT_lIMIT, transform.position.y, 0);
        }
        else if (transform.position.x < LEFT_lIMIT)
        {
            transform.position = new Vector3(RIGHT_lIMIT, transform.position.y, 0);
        }
    }

    public void SpeedBoostPowerUpOn()
    {
        _speedBoostPowerUp = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostPowerUp = false;
    }

}
