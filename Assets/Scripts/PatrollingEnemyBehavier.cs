using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemyBehavier : MonoBehaviour
{

    #region Public Variables
    public float minAttackDistance; // min attack distance
    public float moveSpeed;
    public float timer; // time between attacks
    public Transform rightLimit;
    public Transform leftLimit;
    [HideInInspector]public Transform target;
    [HideInInspector]public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    [HideInInspector] public Animator anim;
    #endregion

    #region Private Variables

    private float Timer;
    private float distance; // stores the distance between the enemy and player
    private bool attackMode; // enemy enters attack mode
    private bool cooling;//checks if enemy is cooling after attack
    #endregion

    private void Awake()
    {
        SelectTarget();
        Timer = timer;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!attackMode)
        {
            Move();
        }

        if(!InsideLimits() && !inRange)
        {
            SelectTarget();    
        }

        if (inRange)
        {
           
            EnemyLogic();
        }
    }
    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > minAttackDistance)
        {
            stopAttack();
        }
        else if( minAttackDistance > distance && !cooling)
        {
            Attack();
        }

        if (cooling)
        {
            cooldown();
            anim.SetBool("Attack", false);
        }
    }

    private void Move()
    {
         anim.SetBool("Walk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){

            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        }
    }

    void Attack()
    {
        timer = Timer;//reset timer when player enter attack range
        attackMode = true; // to check if enemy is still attacking or not
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", true);
    }

    private void stopAttack()
    {
        cooling = false;
        attackMode = false;
       anim.SetBool("Attack", false);
    }

    private bool InsideLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

        void cooldown()
        {
            timer -= Time.deltaTime;
        
            if(timer<=0 && cooling && attackMode)
            {
                cooling = false;
                timer = Timer;
            }
        }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else if (distanceToLeft < distanceToRight)
        {
            target = rightLimit;
        }
        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }

    public void TriggerCooling()
    {
        cooling = true;
    }
}
