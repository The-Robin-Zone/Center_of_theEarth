using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_EnemyByShootAndTouch : MonoBehaviour
{
    public AudioSource enemyTakeDamageSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ShootingTile"))
        {
            //enemyTakeDamageSound.Play();
            Destroy(transform.parent.gameObject);
            Debug.Log("Enemy distroyed");
        }
    }
}
