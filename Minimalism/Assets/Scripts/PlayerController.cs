using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxHP;
    public float curHP;

    public bool Unsheathed = false;
    private bool moving = false;
    private bool invincible = false;
    private bool dead = false;
    private float timer = 0f;

    public Rigidbody2D body;
    public GameObject bodyObject;
    public Animator sword;
    public LineRenderer line;

    public GameObject innerRange;
    public InnerRange innerRangeController;
    private float maxInnerSize = 3f;
    public float innerSize = 3f;
    public bool projectileInInnerRange = false;
    public GameObject ranges;
    public GameObject outerRange;
    public OuterRange outerRangeController;
    public float outerSize = 5f;
    public bool enemyInOuterRange = false;
    public float rangeDecrement = 0.01f;
    public float rangeIncrement = 0.3f;

    private Vector3 startLine;
    private bool canRestart = false;

    // Start is called before the first frame update
    void Start()
    {
        maxInnerSize = innerSize;
        innerRange.transform.localScale = new Vector3(innerSize, innerSize);
        outerRange.transform.localScale = new Vector3(outerSize, outerSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.Space) && !dead)
        {
            //unsheathe
            if (!Unsheathed)
            {
                sword.SetTrigger("Unsheathe");
            }
            Unsheathed = true;
        }
        else if(sword.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Sword_Unsheathed" && !dead)
        {
            print("sheathing");
            sword.SetTrigger("Sheathe");
        }
        if (Input.GetKeyUp(KeyCode.Space) && !dead)
        {
            //slice or put back
            if (enemyInOuterRange)
            {
                //slice
                timer = .3f;
                var pos = outerRangeController.enemies[0].transform.position;
                StartCoroutine(moveBodyToLocation(body.transform.position,pos,.3f, outerRangeController.enemies[0]));
                outerRangeController.enemies.RemoveAt(0);
                if (outerRangeController.enemies.Count == 0)
                {
                    enemyInOuterRange = false;
                }
                sword.SetTrigger("Slash1");
                IncreaseRange(rangeIncrement);

            }
            else if (projectileInInnerRange)
            {
                //deflect
                timer = .2f;
                var obj = innerRangeController.projectiles[0];
                Vector3 targetPosition = obj.transform.position;
                Vector3 dir = targetPosition - body.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                body.transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
                innerRangeController.projectiles.RemoveAt(0);
                Destroy(obj, .1f);
                if (innerRangeController.projectiles.Count == 0)
                {
                    projectileInInnerRange = false;
                }
                if (sword.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Sword_Defense")
                {
                    sword.SetTrigger("Defense");
                    IncreaseRange(rangeIncrement);
                }
                GameManager.gm.addScore(800);
            }
            else
            {
                //punish
                DecreaseRange(rangeIncrement / 1.3f);
            }
            //put back
            Unsheathed = false;
        }

        if (Unsheathed && !dead)
        {
            if (projectileInInnerRange)
            {
                //deflect
                timer = .2f;
                var obj = innerRangeController.projectiles[0];
                Destroy(obj, .1f);
                Vector3 targetPosition = obj.transform.position;
                Vector3 dir = targetPosition - body.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                body.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                innerRangeController.projectiles.RemoveAt(0);
                if (innerRangeController.projectiles.Count == 0)
                {
                    projectileInInnerRange = false;
                }
                if (sword.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Sword_Defense")
                {
                    sword.SetTrigger("Defense");
                    IncreaseRange(rangeIncrement);
                }
                GameManager.gm.addScore(250);
            }
            DecreaseRange(rangeDecrement * Time.deltaTime);
        } else
        {
            if(innerSize+(rangeDecrement * Time.deltaTime) < maxInnerSize)
            {
                innerSize += rangeDecrement * Time.deltaTime;
                outerSize = innerSize;
                innerRange.transform.localScale = new Vector3(innerSize, innerSize);
                outerRange.transform.localScale = new Vector3(outerSize, outerSize);
            }
        }

        ranges.transform.position = body.transform.position;
        if (moving)
        {
            line.SetPosition(0, startLine);
            line.SetPosition(1, body.transform.position);
        }else
        {
            line.SetPosition(0, startLine);
            line.SetPosition(1, startLine);
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            invincible = true;
        } else
        {
            invincible = false;
        }
    }

    private void FixedUpdate()
    {
        
    }

    public IEnumerator moveBodyToLocation(Vector3 start, Vector3 end, float time, GameObject pointObject)
    {
        Vector3 targetPosition = end;
        Vector3 dir = targetPosition - body.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetPosition += dir.normalized;
        body.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        float elapsedTime = 0;
        startLine = start;
        line.SetPosition(0, start);
        line.SetPosition(1, targetPosition);
        while (elapsedTime < time)
        {
            moving = true;
            body.transform.position = Vector3.Lerp(start, targetPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moving = false;
        Destroy(pointObject);
        GameManager.gm.addScore(1000);
    }
    public IEnumerator moveRangesToLocation(float delay, float time)
    {
        var start = body.transform.position;
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0;
        if (!moving || Vector3.Distance(ranges.transform.position, body.transform.position) <= .2f)
        {
            moving = true;
            while (elapsedTime < time)
            {
                ranges.transform.position = Vector3.Lerp(start, body.transform.position, (elapsedTime / time));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            moving = false;
        }
        line.SetPosition(0, start);
        line.SetPosition(1, start);
    }



    public void TakeDamage(float damage)
    {
        if (!invincible)
            curHP -= damage;
        if (curHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        dead = true;
        GameManager.gm.uiAnim.SetTrigger("Death");
        yield return new WaitForSeconds(.2f);
        canRestart = true;
    }

    public void IncreaseRange(float amount)
    {
        if(outerSize < maxInnerSize)
        {
            outerSize = maxInnerSize+amount/2;
        } else
        {
            outerSize += amount;
        }
        innerRange.transform.localScale = new Vector3(innerSize, innerSize);
        outerRange.transform.localScale = new Vector3(outerSize, outerSize);
    }
    public void DecreaseRange(float amount)
    {
        if (outerSize > maxInnerSize)
        {
            outerSize -= amount;
        }
        else if (outerSize <= maxInnerSize && outerSize > 0)
        {
            outerSize -= amount;
            innerSize -= amount;
        }
        else if (outerSize <= 0)
        {
            outerSize = 0;
            innerSize = 0;
        }
        innerRange.transform.localScale = new Vector3(innerSize, innerSize);
        outerRange.transform.localScale = new Vector3(outerSize, outerSize);
    }
}
