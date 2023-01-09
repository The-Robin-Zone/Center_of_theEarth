using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemyBehavier : MonoBehaviour
{

    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float minAttackDistance; // min attack distance
    public float speed;
    public float timer; // time between attacks
    public Transform rightLimit;
    public Transform leftLimit;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private Transform target;
    private Animation anim;
    private float Timer;
    private float distance; // stores the distance between the enemy and player
    private bool attackMode = false; // enemy enters attack mode
    private bool inRange;//checks if player is in range
    private bool cooling;//checks if enemy is cooling after attack
    #endregion

    private void Awake()
    {
        SelectTarget();
        Timer = timer;
     //   anim = GetComponent<Animator>();

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
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        //when player is detected
        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (!inRange)
        {
            stopAttack();
        }
    }
    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > minAttackDistance)
        {
            Move();
            stopAttack();
        }
        else if( minAttackDistance >= distance && !cooling)
        {
            Attack();
        }

        if (cooling)
        {
            //anim.setBool("Attack", false);
        }
    }

    private void Move()
    {
       // anim.SetBool("canWalk", true);
        // if(!anim.GetCurrentAnimationStateInfo(0).IsName("name of attack animation"){}

        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        timer = Timer;//reset timer when player enter attack range
        attackMode = true; // to check if enemy is still attacking or not

        //anim.SetBool("canWalk", false);
        //anim.SetBool("Attack", true);
    }

    private void stopAttack()
    {
        cooling = false;
        attackMode = false;
       // anim.SetBool("", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.transform;
            inRange = true;
            Flip();
        }
    }
    void RaycastDebugger()
    {
        if(distance > minAttackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if(minAttackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    private bool InsideLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }
}
