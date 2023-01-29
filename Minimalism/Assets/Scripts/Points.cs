using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI[] score;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPoints(int n)
    {
        if(n >= 0)
        {
            foreach(var v in score)
            {
                v.text = "+" + n;
            }
        } else
        {
            foreach (var v in score)
            {
                v.text = ""+n;
            }
        }
    }
}
