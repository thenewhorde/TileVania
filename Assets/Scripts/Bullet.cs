using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 1f;
    
    Rigidbody2D myRigidbody;
    //Find the player movement component, set it to variable "player"
    PlayerMovement player;
    float xSpeed;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //find the component Player Movement on the player object
        player = FindObjectOfType<PlayerMovement>();
        //Figure out the player current facing direction, times that by bullet speed.
        xSpeed = player.transform.localScale.x * bulletSpeed;

        //change direction of bullet
        transform.localScale = new Vector2 (Mathf.Sign (xSpeed)* transform.localScale.x, transform.localScale.y);

    }


    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (myRigidbody.IsTouchingLayers(LayerMask.GetMask("Enemy")))

        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

}