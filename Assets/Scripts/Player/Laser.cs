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

    // Cooldown
    public GameObject CircleCoolDown;
    public Image CircleCoolDown_Fill_image;
    //public GameObject cooldownObj;
    //public Slider slider;

    private float shotForce = 5.0f;
    private float suctionTimeMult = 1.75f;
    private float minSuctionMouseDistance = 0.8f;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();

        //CircleCoolDown.SetActive(true);
        //cooldownObj = GameObject.FindGameObjectWithTag("Cooldown");
        //slider = cooldownObj.GetComponent<Slider>();
        //CircleCoolDown = GameObject.FindGameObjectWithTag("CircleCoolDown");
        CircleCoolDown = GameObject.FindGameObjectWithTag("CircleCoolDown");
        CircleCoolDown_Fill_image = GameObject.FindGameObjectWithTag("CircleCoolDown_Fill").GetComponent<Image>();
        //CircleCoolDown.SetActive(false);
    }
    private void FixedUpdate()
    {
        ShootLaser();
    }

    void ShootLaser()
    {

        Vector2 startPos = laserFirePoint.position;
        Vector2 endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // if mouse is not close to player while shooting
        if (!((endPos - startPos).magnitude < minSuctionMouseDistance))
        {

            //Change Last Value for beam distance (only beam, not raycast)
            endPos = (endPos - startPos).normalized * shotForce;
            //Change Last Value for ray distance (only the raycast, not beam)
            hit = Physics2D.Raycast(transform.position, transform.right, shotForce);


            //Set the start and end position of the Beam so it doesnt go through walls
            m_lineRenderer.SetPosition(0, startPos);

            if (hit.collider != null && hit.transform.gameObject.tag == "Ground")
            {
                m_lineRenderer.SetPosition(1, (Vector2)hit.transform.position);
            }
            else if (hit.collider != null && hit.transform.gameObject.tag == "StaticGround")
            {
                m_lineRenderer.SetPosition(1, (Vector2)hit.transform.position);
            }
            else
            {
                m_lineRenderer.SetPosition(1, startPos + endPos);
            }



            //If Ray hits a ground tile, disable it
            if (hit.collider != null && hit.transform.gameObject.tag == "Ground" && CircleCoolDown_Fill_image.fillAmount == 1)
            {
                Debug.DrawRay(transform.position, hit.point, Color.white);
                hit.transform.gameObject.SetActive(false);
                Global_Variables.ammo++;

                //slider.value = 3;
                CircleCoolDown_Fill_image.fillAmount = 0;
                CircleCoolDown.SetActive(false);

            }
            else if (hit.collider != null && hit.transform.gameObject.tag == "Ground")
            {
                //slider.value = slider.value - (0.1f * suctionTimeMult);
                CircleCoolDown.SetActive(true);
                CircleCoolDown_Fill_image.fillAmount = CircleCoolDown_Fill_image.fillAmount + (0.01f * suctionTimeMult);
            }
            else if (hit.collider == null || hit.transform.gameObject.tag != "Ground")
            {
                //slider.value = 3;
                CircleCoolDown_Fill_image.fillAmount = 0;
                CircleCoolDown.SetActive(false);
            }
        }
        else
        {
            m_lineRenderer.SetPosition(0, startPos);
            m_lineRenderer.SetPosition(1, startPos);
        }
    }
}
