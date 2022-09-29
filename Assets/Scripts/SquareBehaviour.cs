using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SquareBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 velocity;

    private Rigidbody2D rigidBody;
    private new Collider2D collider;

    private int touchingPlatforms;
    private void Start()
    {
        touchingPlatforms = 0;

        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        velocity = Vector2.one * 1.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.DrawRay(transform.position, Vector2.down * (collider.bounds.size.y / 2), Color.red);
        Debug.DrawRay(transform.position, Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        Debug.DrawRay(new Vector2(transform.position.x - 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        Debug.DrawRay(new Vector2(transform.position.x + 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        ReadKeysAndMove();
        print(touchingPlatforms);
    }
    private void ReadKeysAndMove()
    {
        // 5 metres per second
        float velocity = 5f;

        Vector2 currentVelocity = rigidBody.velocity;
        Vector2 addedVelocity = new();

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            addedVelocity.x = -velocity;
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            addedVelocity.x = velocity;
        }


        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Vector2 rayPos = transform.position;
            rayPos.y += GetComponent<Collider2D>().transform.localScale.y + 0.01f;

            List<RaycastHit2D> res = new();
            res.AddRange(Physics2D.RaycastAll(transform.position, Vector2.down, collider.bounds.size.y / 2 + 0.1f));
            res.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x - 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down, collider.bounds.size.y / 2 + 0.1f));
            res.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x + 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down, collider.bounds.size.y / 2 + 0.1f));
            print(collider.bounds.size.y);
            
            if (touchingPlatforms != 0)
            {
                rigidBody.AddForce(Vector2.up * 1000, ForceMode2D.Impulse);
            }
        }

        rigidBody.AddForce(addedVelocity * 50000 * Time.deltaTime);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            touchingPlatforms = 1;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            touchingPlatforms = 0;
    }
}
