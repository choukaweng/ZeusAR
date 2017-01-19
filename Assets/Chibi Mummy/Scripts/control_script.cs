using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class control_script : MonoBehaviour
{

    Animator anim;
    bool boolper, boolper2, boolper3;
    //Transform target;

    EnemyManager enemyManager;
    GameObject target;
    NavMeshAgent nav;
    bool attacking, targetPosDetected;
    float attackTimer, attackTimerPeriod;
    Vector3 targetPos;
    bool death = false;
    bool doneKill = false;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        attacking = false;
        targetPosDetected = false;
        attackTimer = 0;
        attackTimerPeriod = 2;
        target = GameObject.FindGameObjectWithTag("EnemyTarget");
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        targetPos = target.transform.position;
    }
    public void Walk()
    {

        boolper = anim.GetBool("isWalk");
        anim.SetBool("isWalk", !boolper);
        anim.SetBool("isRun", false);
        anim.SetBool("isAnother", false);
        anim.SetBool("Attack", false);
        anim.SetBool("LowKick", false);
        anim.SetBool("isDeath", false);
        anim.SetBool("isDeath2", false);
        anim.SetBool("HitStrike", false);




    }

    public void Run()
    {

        anim.SetTrigger("Run");
    }

    public void OtherIdle()
    {

        boolper3 = anim.GetBool("isAnother");
        anim.SetBool("isAnother", !boolper3);
        anim.SetBool("isWalk", false);
        anim.SetBool("isRun", false);
        anim.SetBool("Attack", false);
        anim.SetBool("LowKick", false);
        anim.SetBool("isDeath", false);
        anim.SetBool("isDeath2", false);
        anim.SetBool("HitStrike", false);




    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void LowKick()
    {
        anim.SetBool("LowKick", true);
    }

    public void Death()
    {
        anim.Play("Death");
         
    }
    public void Death2()
    {
       
        anim.SetBool("isDeath2", true);
    }
    public void Strike()
    {
        anim.SetBool("HitStrike", true);
    }

    public void Damage()
    {
        anim.SetBool("isDamage", true);
    }

    public void Kill()
    {
        death = true;
    }

    void Update()
    {
        
        if(!death)
        {
            if (target != null)
            {
                Run();
                //nav.SetDestination(target.transform.position);

                nav.SetDestination(targetPos);
                Vector3 direction = target.transform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 2f * Time.deltaTime, 0);
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (attacking)
                {

                    if (attackTimer > attackTimerPeriod)
                    {
                        //Debug.Log(targetPos);
                        anim.Play("Attack");
                        target.gameObject.GetComponent<SphereBarrier>().BarrierAttacked();
                        attackTimer = 0;
                    }
                    else
                    {
                        attackTimer += Time.deltaTime;
                    }

                }
            }
        }
        else
        {
            Death();
            Invoke("KillEnemy", 2f);
        }
       
          

    }

    void OnTriggerEnter(Collider c)
    {
        
        if (c.gameObject.tag == "EnemyTarget" && !death)
        {
            attacking = true;
            if(!targetPosDetected)
            {
                targetPos = transform.position;
                targetPosDetected = true;
            }
            
        }
    }
    

    void KillEnemy()
    {
        enemyManager.setCollectables(transform.position);
        Destroy(gameObject);  
    }

}
