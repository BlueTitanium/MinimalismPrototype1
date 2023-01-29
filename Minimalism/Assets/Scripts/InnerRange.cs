using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerRange : MonoBehaviour
{
    public List<GameObject> projectiles;
    public PlayerController p;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(projectiles.Count > 0 && projectiles[0] == null)
        {
            projectiles.RemoveAt(0);
        }
        if(projectiles.Count == 0)
        {
            p.projectileInInnerRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            projectiles.Add(collision.gameObject);
            p.projectileInInnerRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (projectiles.Contains(collision.gameObject))
        {
            projectiles.Remove(collision.gameObject);
        }
        if (projectiles.Count == 0)
        {
            p.projectileInInnerRange = false;
        }
    }
}
