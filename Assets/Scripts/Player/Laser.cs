using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Laser : MonoBehaviour
{
    Transform m_transform;
    RaycastHit2D hit;
    public TextMeshProUGUI ammoAmount;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        ShootLaser();
    }

    void ShootLaser()
    {

        Vector2 startPos = laserFirePoint.position;
        Vector2 endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Change Last Value for beam distance (only beam, not raycast)
        endPos = (endPos-startPos).normalized * 5f;

        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, startPos + endPos);

        //Change Last Value for ray distance (only the raycast, not beam)
        hit = Physics2D.Raycast(transform.position, transform.right, 5f);

        //If Ray hits a ground tile, disable it
        if (hit.collider != null && (hit.transform.gameObject.tag == "Ground"))
        {
            Debug.DrawRay(transform.position, hit.point, Color.white);
            hit.transform.gameObject.SetActive(false);

            Global_Variables.ammo++;

        }
    }
}
