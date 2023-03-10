using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager wm;

    public bool initializing = true;

    public int wave = 0;
    public int numPerWave = 5;
    public List<GameObject> enemiesLeft;

    public GameObject rangeEnemy;
    public GameObject meleeEnemy;

    public PlayerController p;
    // Start is called before the first frame update
    void Start()
    {
        wm = this;
        initializing = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesLeft.Count > 0 && enemiesLeft[0] == null)
        {
            enemiesLeft.RemoveAt(0);
        }
        if(enemiesLeft.Count == 0 && !initializing)
        {
            StartCoroutine(startWave(wave + 1));
        }
    }

    public IEnumerator startWave(int n)
    {
        if (!p.dead)
        {
            wave = n;
            GameManager.gm.waveNum = n;
            GameManager.gm.SetWave();
            initializing = true;
            StatsDisplayer.sd.showWave("wave " + n);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < numPerWave * n - 1; i++)
            {
                // C#
                // get a random direction (360?) in radians
                float angle = Random.Range(0.0f, Mathf.PI * 2);

                // create a vector with length 1.0
                Vector3 position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

                // scale it to the desired length
                position *= 20f;
                if (i % 5 == 0)
                {
                    // range
                    var o = Instantiate(rangeEnemy, position, rangeEnemy.transform.rotation);
                    enemiesLeft.Add(o);
                }
                else
                {
                    //melee
                    var o = Instantiate(meleeEnemy, position, rangeEnemy.transform.rotation);
                    enemiesLeft.Add(o);
                }
                yield return new WaitForSeconds(.5f);
            }
            initializing = false;
        }
    }
}
