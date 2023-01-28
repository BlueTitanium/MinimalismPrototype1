using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    public GameObject bodyObject;
    public GameObject innerRange;
    public GameObject outerRange;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {

        }
    }
}
