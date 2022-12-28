using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float min_distance;
    private float distance;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Awake()
    {

    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        if (distance > min_distance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            //attacking
        }
    }
}
