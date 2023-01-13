using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;
    [HideInInspector] public Animator anim;


    public GameObject bullet;
    public Transform bulletPos;
    public float timer;
    public float minDistance;
    public float timeBetweenShots;
    private float nextShotTime;
    private bool attacking;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (Time.time > nextShotTime)
        {
            anim.SetBool("Attack", true);
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }
        
        

        if (Vector2.Distance(transform.position, target.position) < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
