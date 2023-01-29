using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterRange : MonoBehaviour
{
    public List<GameObject> enemies;
    public PlayerController p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0 && enemies[0] == null)
        {
            enemies.RemoveAt(0);
        }
        if (enemies.Count == 0)
        {
            p.enemyInOuterRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Add(collision.gameObject);
            p.enemyInOuterRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemies.Contains(collision.gameObject))
        {
            enemies.Remove(collision.gameObject);
        }
        if(enemies.Count == 0)
        {
            p.enemyInOuterRange = false;
        }
    }

}
