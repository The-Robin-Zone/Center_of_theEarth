using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject laserGun;

    // Start is called before the first frame update
    void Start()
    {
        laserGun.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {

        Debug.Log("enter shot function");

        if (laserGun.activeSelf == false)
        {
            Debug.Log("shoots fired");
            laserGun.SetActive(true);
        }
        else
        {
            Debug.Log("shoots cancelled");
            laserGun.SetActive(false);
        }
    }
}
