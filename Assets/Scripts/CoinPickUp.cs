using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{

    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {

            wasCollected = true;
            //Add to score within the GameSession script, AddToScore Method, value is 100 (from this current coin script)
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);

            //PlayClipAtPoint allows the sound to continue after the object is destroyed
            //Play sound at camera position so the sound isn't played too far away
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            //Disable coin so it doesn't double pick up.
            gameObject.SetActive(false);
            //gameObject is the coin, collision.gameObject is the player
            Destroy(gameObject);

        }
    }

}
