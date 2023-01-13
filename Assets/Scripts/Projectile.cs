using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float force;
    public LayerMask groundLayer;
    Vector3 targetPosition;
    public float speed;

    private GameObject player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPosition = player.transform.position;
        groundLayer = LayerMask.GetMask("Ground");

        Vector3 direction = targetPosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized* force;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        
        if(collision.gameObject.name == "Player")
        {
            //health System 
            Debug.Log("player hurt");
        }       
    }

    private void Update()
    {
    }
}
