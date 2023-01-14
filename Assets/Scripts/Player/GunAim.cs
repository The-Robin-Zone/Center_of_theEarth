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

    public GameObject Player;
    public GameObject laserGun;
    private Transform InnerHand;
    private Transform OuterHand;
    private Transform Gun;

    public bool IsBeamActive = true;
    public string shootType = "Beam";

    //for shooting gun
    public float speed = 10;
    public Transform target;
    public GameObject bullet;
    public Transform bulletPos;

    public GameObject cooldownObj;
    public Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        laserGun.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        InnerHand = transform.Find("InnerHand");
        OuterHand = transform.Find("OuterHand");
        Gun = transform.Find("Gun").Find("Sprite");
        Player = GameObject.FindGameObjectWithTag("Player");

        cooldownObj = GameObject.FindGameObjectWithTag("Cooldown");
        slider = cooldownObj.GetComponent<Slider>();
        slider.value = 3;
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

        Transform[] hands = { OuterHand, InnerHand };//, Gun};
        foreach (Transform hand in hands) {
            SetHandOrientation(hand, rotQuaternion, yScale);
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            slider.value = 3;
           
            if (shootType.Equals("Beam"))
            {
                ShootBeam();
            }
            if (shootType.Equals("Shoot"))
            {
                ShootTiles();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            slider.value = 3;

            if (laserGun.activeSelf == true)
            {
                laserGun.SetActive(false);
            }

            IsBeamActive = !IsBeamActive;

            if (IsBeamActive)
            {
                shootType = "Beam";
            }
            else
            {
                shootType = "Shoot";
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


            //if aim is directly down and jump
            if ((AimVector.y < transform.position.y) && (Math.Abs(AimVector.x - transform.position.x) < 2) && IsPlayerInAir())
            {
                CurrBullet = Instantiate(bullet, transform.position - new Vector3(0,1,0), Quaternion.identity);
                CurrBullet.GetComponent<Rigidbody2D>().AddForce(shotForce.normalized * 300);
                //Vector3 _scale = CurrBullet.transform.localScale;
                //float _mult = Global_Variables.shotTileMultiplier;
                //CurrBullet.transform.localScale = new Vector3(_scale.x * _mult, _scale.y * _mult, _scale.z);
                Global_Variables.ammo--;
            }
            //if aim is directly down and didnt jump
            else if ((AimVector.y < transform.position.y) && (Math.Abs(AimVector.x - transform.position.x) < 2))
            {
                Debug.Log("player aimed down and shot, but not in the air so dont do anything!");

            }
            //if aim is not down
            else
            {
                CurrBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                CurrBullet.GetComponent<Rigidbody2D>().AddForce(shotForce.normalized * 300);
                //Vector3 _scale = CurrBullet.transform.localScale;
                //float _mult = Global_Variables.shotTileMultiplier;
                //CurrBullet.transform.localScale = new Vector3(_scale.x * _mult, _scale.y * _mult, _scale.z);
                Global_Variables.ammo--;
            }

        }
    }

    public bool IsPlayerInAir()
    {
        Vector3 playerFeet = Player.transform.position - new Vector3(0, 0.31f, 0);

        RaycastHit2D hit = Physics2D.Raycast(playerFeet, playerFeet - new Vector3(0,1,0), 0.1f);
        Debug.DrawRay(playerFeet, new Vector3(0, -1, 0), Color.white);

        //If Ray hits a ground tile, enter loop
        if (hit.collider != null && (hit.transform.gameObject.tag != "Player"))
        {
            return false;

        }
        else
        {
            return true;
        }

    }

}
