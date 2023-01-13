using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Enemy : MonoBehaviour
{
    private PatrollingEnemyBehavier enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<PatrollingEnemyBehavier>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(enemyParent.gameObject);
            
            Debug.Log("Enemy distroyed");

        }
    }
}
