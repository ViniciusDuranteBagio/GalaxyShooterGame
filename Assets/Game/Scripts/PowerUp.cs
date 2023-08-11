using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
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
            //access the player
            // if the powerupID is 0
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if (powerupID == 0)
                {
                    player.TipleShootPowerUpOn();
                }
                 
                else if (powerupID == 1)
                {
                    player.SpeedBoostPowerUpOn();       
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
