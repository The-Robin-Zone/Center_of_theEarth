using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_EnemyByShootAndTouch : MonoBehaviour
{
    public GameObject enemyTakeDamageSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ShootingTile"))
        {
            enemyTakeDamageSound = Instantiate(enemyTakeDamageSound, transform.parent.gameObject.transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
            Debug.Log("Enemy distroyed");
        }
    }
}
