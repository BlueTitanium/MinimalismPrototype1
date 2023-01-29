using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    public TextMeshProUGUI[] lines;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStatus(string s)
    {
        foreach(var v in lines)
        {
            v.text = s;
        }
    }
}
