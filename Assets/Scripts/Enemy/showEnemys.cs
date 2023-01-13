using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showEnemys : MonoBehaviour
{
    public GameObject enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.SetActive(true);

        }
    }
}
