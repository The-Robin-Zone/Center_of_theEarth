using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Appear : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetChild(1).gameObject.SetActive(true);
        Destroy(transform.gameObject);
    }
}
