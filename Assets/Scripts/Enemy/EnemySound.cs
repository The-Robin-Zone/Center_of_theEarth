using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource enemyTakeDamageSound;
    public float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer == 3)
        {
            Destroy(this);
        }
    }

    void Awake()
    {
        enemyTakeDamageSound.Play();
    }

}
