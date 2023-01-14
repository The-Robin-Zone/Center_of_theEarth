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
    private bool facingRight = false;


    private void Update()
    {
        timer += Time.deltaTime;
        if(Time.time > nextShotTime)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }
        
        if(transform.position.x > target.position.x && facingRight){
            Flip();
            Debug.Log("Should Flip");
            
        }
        else if (transform.position.x <= target.position.x && !facingRight)
        {
            Flip();
        }

            if (Vector2.Distance(transform.position, target.position) < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            Flip();
        }
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
        facingRight = !facingRight;
    }
}
