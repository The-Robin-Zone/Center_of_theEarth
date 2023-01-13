using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerraycast : MonoBehaviour
{

    [SerializeField] float distance = 100f;

    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, distance);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.point, Color.white);
            hit.transform.gameObject.SetActive(false);
            Debug.Log("WAS HIT");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.position + transform.right * distance, Color.black);
            Debug.Log("WAS  NOT HIT");
        }
    }
}
