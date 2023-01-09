using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaPatrol : MonoBehaviour
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
            gameObject.SetActive(false);
            enemyParent.target = collision.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
