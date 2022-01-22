using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    bool wasCoinCollected = false;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !wasCoinCollected)
        {
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().IncreaseScore();
            wasCoinCollected = true;
        }    
    }
}
