using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    public static StatsDisplayer sd;

    public GameObject pointPrefab;
    public GameObject statusPrefab;
    public GameObject wavePrefab;
    public bool showStatusLine = true;

    // Start is called before the first frame update
    void Start()
    {
        sd = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPoints(int n)
    {
        var v = Instantiate(pointPrefab,transform);
        v.GetComponent<Points>().SetPoints(n);
    }

    public void showStatus(string s)
    {
        if (showStatusLine) {
            var v = Instantiate(statusPrefab, transform);
            v.GetComponent<Status>().SetStatus(s);
        }
        
    }

    public void showWave(string s)
    {
         var v = Instantiate(wavePrefab, transform);
         v.GetComponent<WaveLine>().SetWave(s);
    }

}
