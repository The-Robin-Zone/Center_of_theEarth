using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;

    public GameObject bullet;
    public Transform bulletPos;
    public float timer;
    public float minDistance;
    public float timeBetweenShots;
    private float nextShotTime;


    private void Update()
    {
        timer += Time.deltaTime;
        if(Time.time > nextShotTime)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }
        

        if (Vector2.Distance(transform.position, target.position) < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void shoot()
    {
        
    }
}
