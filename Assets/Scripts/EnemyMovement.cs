using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private bool isFlattened = false; 

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
        Goomba.OnGoombaFlattened += StopMovement;
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        if (!isFlattened)
        {
            enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (!isFlattened && Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            Movegoomba();
        }
        else if (!isFlattened)
        {
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    void OnDestroy()
    {
        Goomba.OnGoombaFlattened -= StopMovement;
    }



    private void StopMovement(GameObject goomba)
    {
        if (goomba == gameObject) // Make sure it's the correct Goomba
        {
            isFlattened = true;
            enemyBody.linearVelocity = Vector2.zero; // Stop any movement immediately
        }
    }
}