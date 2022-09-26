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
        //BrokenCode();
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
    private void BrokenCode()
    {
        Camera cam = Camera.main;

        Vector2 topRight = cam.ViewportToWorldPoint(Vector2.one);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(Vector2.zero);

        var left_x_edge = transform.position.x - collider.bounds.size.x / 2;
        var right_x_edge = transform.position.x + collider.bounds.size.x / 2;

        var top_y_edge = transform.position.y + collider.bounds.size.y / 2;
        var bottom_y_edge = transform.position.y - collider.bounds.size.y / 2;

        print("topRight: " + topRight);
        print("bottomLeft: " + bottomLeft);

        print("left_x_edge: " + left_x_edge);
        print("right_x_edge: " + right_x_edge);
        print("top_y_edge: " + top_y_edge);
        print("bottom_y_edge: " + bottom_y_edge);

        if (left_x_edge <= bottomLeft.x || right_x_edge >= topRight.x)
        {
            velocity.x *= -1;
        }
        if (bottom_y_edge <= bottomLeft.y || top_y_edge >= topRight.y)
        {
            velocity.y *= -1;
        }
        gameObject.transform.position += (Vector3)velocity * Time.deltaTime;
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
