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
    public TextMeshProUGUI ammoAmount;

    //for shooting gun
    public float speed = 10;
    public Transform target;
    public GameObject bullet;
    public Transform bulletPos;
    

    // Start is called before the first frame update
    void Start()
    {
        laserGun.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        InnerHand = transform.Find("InnerHand");
        OuterHand = transform.Find("OuterHand");
        Gun = transform.Find("Gun");
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
        SetHandOrientation(OuterHand, rotQuaternion, yScale);
        SetHandOrientation(InnerHand, rotQuaternion, yScale);
        SetHandOrientation(Gun, rotQuaternion, yScale);

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
        // TO FIX
        // AIM isnt accurate
        // Ammo count is not working properly

        //GameObject som = GameObject.FindGameObjectWithTag("LaserGun");

        //int currAmmo = Int32.Parse(ammoAmount.text);
        
        if (Int32.Parse(ammoAmount.text) > 0)
        {
            ammoAmount.text = (Int32.Parse(ammoAmount.text) - 1).ToString();

            GameObject CurrBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            Vector2 AimVector = Input.mousePosition;

            AimVector = AimVector - new Vector2(963,534);

            //Debug.Log("Mouse position is : " + AimVector);

            CurrBullet.GetComponent<Rigidbody2D>().AddForce(AimVector);
        }


    }
}
