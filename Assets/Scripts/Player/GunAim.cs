using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunAim : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject laserGun;
    private Transform InnerHand;
    private Transform OuterHand;
    private Transform Gun;

    public bool IsBeamActive = true;
    public TextMeshProUGUI ShootType;

    //for shooting gun
    public float speed = 10;
    public Transform target;
    public GameObject bullet;
    public Transform bulletPos;

    // Start is called before the first frame update
    void Awake()
    {
        ShootType = GameObject.Find("HUDShootType").GetComponent<TextMeshProUGUI>();

        laserGun.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        InnerHand = transform.Find("InnerHand");
        OuterHand = transform.Find("OuterHand");
        Gun = transform.Find("Gun").Find("Sprite");
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        // if aiming left, flip the pieces vertically so they aren't upside-down
        int yScale = ((rotZ > 90 && rotZ <= 270) || (rotZ < -90 && rotZ >= -270)) ? -1 : 1;

        Quaternion rotQuaternion = Quaternion.Euler(0, 0, rotZ);
        transform.rotation = rotQuaternion;

        Transform[] hands = {OuterHand, InnerHand, Gun};
        foreach (Transform hand in hands) {
            SetHandOrientation(hand, rotQuaternion, yScale);
        }
         
        if (Input.GetMouseButtonDown(0))
        {
            if (ShootType.text == "Beam")
            {
                ShootBeam();
            }
            if (ShootType.text == "Shoot")
            {
                ShootTiles();
            }
        }

        if (Input.GetMouseButtonDown(1) && laserGun.activeSelf == false)
        {
            IsBeamActive = !IsBeamActive;

            if (IsBeamActive)
            {
                ShootType.text = "Beam";
            }
            else
            {
                ShootType.text = "Shoot";
            }
        }
    }

    private void SetHandOrientation (Transform obj, Quaternion quaternion, int yScale) {
        obj.rotation = quaternion;
        // maintain its size but alter direction
        Vector3 scale = obj.localScale;
        scale.y = Mathf.Abs(scale.y) * Mathf.Sign(yScale);
        obj.localScale = scale;
    }

    public void ShootBeam()
    {
        if (laserGun.activeSelf == false)
        {
            laserGun.SetActive(true);
        }
        else
        {
            laserGun.SetActive(false);
        }
    }

    public void ShootTiles()
    {

        if (Global_Variables.ammo > 0)
        {
            Vector2 AimVector = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 shotForce = AimVector - (Vector2)laserGun.transform.position;

            GameObject CurrBullet;

            //if aim is directly down enter if
            if ((AimVector.y < transform.position.y) && (Math.Abs(AimVector.x - transform.position.x) < 1))
            {
                CurrBullet = Instantiate(bullet, transform.position - new Vector3(0,1,0), Quaternion.identity);
            }
            else
            {
                CurrBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            }

            CurrBullet.GetComponent<Rigidbody2D>().AddForce(shotForce.normalized * 300);

            Global_Variables.ammo--;

        }
    }
}
