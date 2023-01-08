using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    [SerializeField] float distance = 500f;
    Transform m_transform;
    RaycastHit2D hit;
    //public int ammo = 0;
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
        //ammoAmount.text = ammo.ToString();
    }

    void ShootLaser()
    {
        if (Physics2D.Raycast(m_transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
            Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * defDistanceRay);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);

        hit = Physics2D.Raycast(transform.position, transform.right, distance);

        //If Ray hits a ground tile, disable it
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.point, Color.white);
            if (hit.transform.gameObject.tag == "Ground")
            {
                hit.transform.gameObject.SetActive(false);
                ammoAmount.text = (Int32.Parse(ammoAmount.text)+1).ToString();
                //ammo++;
            }
           
        }
    }
}
