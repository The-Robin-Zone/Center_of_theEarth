using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    Transform m_transform;
    RaycastHit2D hit;
    public TextMeshProUGUI ammoAmount;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    public GameObject cooldownObj;
    public Slider slider;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        //cooldownObj = GameObject.FindGameObjectWithTag("Cooldown");
        //slider = cooldownObj.GetComponent<Slider>();

    }
    private void FixedUpdate()
    {
        ShootLaser();
    }

    void ShootLaser()
    {

        Vector2 startPos = laserFirePoint.position;
        Vector2 endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // if mouse is close to player while shooting
        if (!(Math.Abs(startPos.x - endPos.x) < 1f && Math.Abs(startPos.y - endPos.y) < 1f))
        {

            //Change Last Value for beam distance (only beam, not raycast)
            endPos = (endPos - startPos).normalized * 5f;
            //Change Last Value for ray distance (only the raycast, not beam)
            hit = Physics2D.Raycast(transform.position, transform.right, 5f);


            //Set the start and end position of the Beam so it doesnt go through walls
            m_lineRenderer.SetPosition(0, startPos);

            if (hit.collider != null)
            {
                //endPos = (Vector2)hit.transform.position;
                m_lineRenderer.SetPosition(1, (Vector2)hit.transform.position);
            }
            else
            {
                m_lineRenderer.SetPosition(1, startPos + endPos);
            }



            //If Ray hits a ground tile, disable it
            if (hit.collider != null && hit.transform.gameObject.tag == "Ground") //&& slider.value == 0)
            {
                Debug.DrawRay(transform.position, hit.point, Color.white);
                hit.transform.gameObject.SetActive(false);
                //slider.value = 3;
                Global_Variables.ammo++;

            }
            else if (hit.collider != null && hit.transform.gameObject.tag == "Ground")
            {
                //slider.value = slider.value - 0.1f;
            }
            else if (hit.collider == null || hit.transform.gameObject.tag != "Ground")
            {
                //slider.value = 3;
            }
        }
        else
        {
            m_lineRenderer.SetPosition(0, startPos);
            m_lineRenderer.SetPosition(1, startPos);
        }
    }
}
