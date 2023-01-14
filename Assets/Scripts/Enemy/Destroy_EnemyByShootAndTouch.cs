using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_EnemyByShootAndTouch : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ShootingTile"))
        {
            Destroy(transform.parent.gameObject);
            Debug.Log("Enemy distroyed");

        }
    }
}
