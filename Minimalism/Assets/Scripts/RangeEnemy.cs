using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public GameObject projectile;
    public Transform shootSpot;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool shoot = true; //in shooting mode
    //private bool isShooting = false; //time to shoot projectile
    private float shootTimer;
    private float waitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("PlayerNecessities/Player/GameObject/Body").GetComponent<Transform>();
        waitTime = waitTime/((GameManager.gm.waveNum - 1)/5 + 1);
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
        shootTimer += Time.deltaTime;
        if (!GameManager.gm.paused) {
            moveTowardsPlayer(movement);
            if (shoot && shootTimer > waitTime) {
                shootProjectile();
                shootTimer = 0;
            }
        }
    }

    void moveTowardsPlayer(Vector2 dir) {
        if (shoot == false) {
            moveSpeed = 5f;
        }
        rb.MovePosition((Vector2)transform.position + (dir * moveSpeed * Time.deltaTime));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "OuterRange" )
        {
            shoot = false;
            print("collide");
        }
        if (collision.gameObject.CompareTag("Player") )
        {
            print("bamRange");
            Vector3 dir = -1 * (transform.position - collision.transform.position).normalized;
            GameObject.FindObjectOfType<PlayerController>().KnockBack(dir, 5f);
            GameObject.FindObjectOfType<PlayerController>().TakeDamage(1.5f);
            Destroy(gameObject);
        }
    }

    //create the projectile
    void shootProjectile() {
        Instantiate(projectile, shootSpot.position, transform.rotation);
        print("shoot");
    }
}
