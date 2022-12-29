using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    Transform m_transform;

    [SerializeField] float distance = 500f;
    RaycastHit2D hit;
    private int ammo = 0;
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        ShootLaser();
        ammoText.text = "  X " + ammo;
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

        //If Ray hit something, than disable it
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.point, Color.white);
            if (hit.transform.gameObject.tag != "Player")
            {
                hit.transform.gameObject.SetActive(false);
                ammo++;
            }
           
        }
    }
}
