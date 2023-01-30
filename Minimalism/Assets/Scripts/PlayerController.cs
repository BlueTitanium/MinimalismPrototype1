using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHP;
    public float curHP;
    public Image hpBar;
    public SpriteRenderer ball;
    private Color original;

    public bool Unsheathed = false;
    private bool moving = false;
    private bool invincible = false;
    public bool dead = false;
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
    public float maxSize = 15f;
    public bool enemyInOuterRange = false;
    public float rangeDecrement = 0.01f;
    public float rangeIncrement = 0.3f;

    private Vector3 startLine;
    private bool canRestart = false;

    // Start is called before the first frame update
    void Start()
    {
        original = ball.color;
        curHP = maxHP;
        maxInnerSize = innerSize;
        innerRange.transform.localScale = new Vector3(innerSize, innerSize);
        outerRange.transform.localScale = new Vector3(outerSize, outerSize);
        hpBar.fillAmount = (curHP / maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
            Unsheathed = false;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !dead && !GameManager.gm.paused)
        {
            //slice or put back
            if (enemyInOuterRange)
            {
                //slice
                if (!moving)
                {
                    CameraShake.cs.cameraShake(.3f, 3f);
                    timer = .3f;
                    var pos = outerRangeController.enemies[0].transform.position;
                    StartCoroutine(moveBodyToLocation(body.transform.position, pos, .3f, outerRangeController.enemies[0]));
                    outerRangeController.enemies.RemoveAt(0);
                    if (outerRangeController.enemies.Count == 0)
                    {
                        enemyInOuterRange = false;
                    }
                    sword.SetTrigger("Slash1");
                    IncreaseRange(rangeIncrement);
                }
            }
            else if (projectileInInnerRange)
            {
                //deflect
                CameraShake.cs.cameraShake(.1f, 1.5f);
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
                StatsDisplayer.sd.showStatus("Parried");
            }
            else
            {
                //punish
                DecreaseRange(rangeIncrement / 1.3f);
                StatsDisplayer.sd.showStatus("Whiffed");
            }
            //put back
            Unsheathed = false;
        }

        if (Unsheathed && !dead)
        {
            if (projectileInInnerRange)
            {
                CameraShake.cs.cameraShake(.1f, 1.5f);
                //deflect
                timer = .2f;
                var obj = innerRangeController.projectiles[0];
                Vector3 targetPosition = obj.transform.position;
                Vector3 dir = targetPosition - body.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                body.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
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
                GameManager.gm.addScore(250);
                StatsDisplayer.sd.showStatus("Blocked");
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
        hpBar.fillAmount = (curHP/maxHP);
    }

    private void FixedUpdate()
    {
        
    }

    public IEnumerator moveBodyToLocation(Vector3 start, Vector3 end, float time, GameObject pointObject)
    {
        timer = time+.05f;
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
        StatsDisplayer.sd.showStatus("Sliced");
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
        if (!invincible && !dead)
        {
            CameraShake.cs.cameraShake(.1f, 2f);
            StartCoroutine(showDamage(.1f));
            curHP -= damage;
            GameManager.gm.addScore(-500);
            StatsDisplayer.sd.showStatus("Damaged");
        }
        if (curHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator showDamage(float t)
    {
        ball.color = new Color(.45f, 0, 0f);
        yield return new WaitForSeconds(t);
        ball.color = original;
    }

    public IEnumerator Die()
    {
        GameManager.gm.paused = true;
        dead = true;
        GameManager.gm.uiAnim.SetTrigger("Death");
        yield return new WaitForSeconds(.5f);
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
        if(outerSize > maxSize)
        {
            outerSize = maxSize;
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
