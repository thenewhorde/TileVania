using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidbody;



    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);

    }


    void OnTriggerExit2D(Collider2D other)
    {

        //I want the monster to not turn when he touches the player.
        //Only to change direction when touching the wall.
       if (other.gameObject.layer == LayerMask.NameToLayer("Player"))

        {
            return;
        }

       else  
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();

        Debug.Log("Collided with something?");
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

}
