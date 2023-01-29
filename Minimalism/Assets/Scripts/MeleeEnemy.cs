using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("PlayerNecessities/Player/GameObject/Body").GetComponent<Transform>();
    }

    // Update is called once per frame
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
        if(!GameManager.gm.paused)
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
            print("bam");
            Destroy(gameObject);
        }
    }
}