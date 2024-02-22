using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public delegate void SpeedBoostDelegate();
    public static event SpeedBoostDelegate OnSpeedBoostPowerUp;

    [SerializeField]
    private float _speed = 2.5f;

    [SerializeField]
    private int powerupID; // 0 = triplesoot 1 = speedboost 2 = shields
    [SerializeField]
    private AudioClip _clip;

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.71f)
        { 
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
          
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if (powerupID == 0)
                {
                    player.TripleShootPowerUpOn();
                }
                 
                else if (powerupID == 1)
                {
                    if (OnSpeedBoostPowerUp != null)
                    {
                        OnSpeedBoostPowerUp();
                    }
                }

                else if (powerupID == 2)
                {
                    player.EnableShields();           
                }

                
            }

            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            //Destroy ourself
            Destroy(this.gameObject);
        }
        
    }
}
