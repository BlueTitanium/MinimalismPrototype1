using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    //public Transform player;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerNecessities/Player/GameObject/Body").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // get the angle btw player and enemy
        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        dir.Normalize();
        movement = dir;
    }

    private void FixedUpdate() {
        moveTowardsPlayer(movement);
    }

    void moveTowardsPlayer(Vector2 dir) {
        rb.MovePosition((Vector2)transform.position + (dir * moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            //player.TakeDamage(10);
            GameObject.FindObjectOfType<PlayerController>().TakeDamage(1f);
            print("pewpewpew");
            Destroy(gameObject);
        }
    }

}
