using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.U2D;
using System.Linq;

public class SquareBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 velocity;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;
    private Animator a;

    private GameObject attackCollider;

    private List<Collider2D> collisions = new();

    enum AnimState
    {
        None,
        Idle,
        Run,
        Jump,
        Fall,
        Attack
    }
    
    private AnimState animState;

    private int touchingPlatforms;
    private void Start()
    {
        touchingPlatforms = 0;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        a = GetComponent<Animator>();
        velocity = Vector2.one * 1.5f;
        attackCollider = transform.Find("AttackCollider").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.DrawRay(transform.position, Vector2.down * (collider.bounds.size.y / 2), Color.red);
        Debug.DrawRay(transform.position, Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        Debug.DrawRay(new Vector2(transform.position.x - 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        Debug.DrawRay(new Vector2(transform.position.x + 0.9f * collider.bounds.size.x / 2, transform.position.y), Vector2.down * (collider.bounds.size.y / 2 + 0.1f));
        ReadKeysAndMove();
        // print(touchingPlatforms);
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
            transform.localScale = new Vector3(-1, 1);
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            addedVelocity.x = velocity;
            transform.localScale = new Vector3(1, 1);
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Vector2 rayPos = transform.position;
            rayPos.y += collider.transform.localScale.y + 0.01f;

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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animState = AnimState.Attack;
            a.SetTrigger("Attack");
            print("Attack");

            foreach (var x in collisions)
            {
                print(x);
            }

            var toDestroy = collisions.FindAll(x => x.gameObject.CompareTag("Enemy"));
            foreach (var x in toDestroy)
            {
                print("Destroying " + x);
                Destroy(x.gameObject);
            }
        }
        else if (animState != AnimState.Attack || a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (touchingPlatforms == 0)
            {
                if (rigidBody.velocity.y > 0)
                {
                    if (animState != AnimState.Jump)
                    {
                        animState = AnimState.Jump;
                        a.SetTrigger("Jump");
                    }
                }
                else if (rigidBody.velocity.y < 0)
                {
                    if (animState != AnimState.Fall)
                    {
                        animState = AnimState.Fall;
                        a.SetTrigger("Fall");
                    }
                }
            }
            else if (Math.Abs(rigidBody.velocity.x) > 0.1)
            {
                if (animState != AnimState.Run)
                {
                    animState = AnimState.Run;
                    a.SetTrigger("Run");
                }
            }
            else if (animState != AnimState.Idle)
            {
                animState = AnimState.Idle;
                a.SetTrigger("Idle");
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            touchingPlatforms = 1;
    }

    public void AttackHitboxEnter(Collider2D collider)
    {
        collisions.Add(collider);
    }
    public void AttackHitboxExit(Collider2D collider)
    {
        collisions.Remove(collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            touchingPlatforms = 0;
    }
}
