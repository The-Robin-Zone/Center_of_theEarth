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
    private float minShootingMouseDistance = 1.6f;

    public AudioSource beamSound;
    //public AudioSource shootSound;
    public AudioSource noAmmoSound;

    // Cooldown
    public GameObject CircleCoolDown;
    public Image CircleCoolDown_Fill_image;

    // Start is called before the first frame update
    void Awake()
    {
        laserGun.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        InnerHand = transform.Find("InnerHand");
        OuterHand = transform.Find("OuterHand");
        Gun = transform.Find("Gun").Find("Sprite");
        Player = GameObject.FindGameObjectWithTag("Player");

        
        CircleCoolDown = GameObject.FindGameObjectWithTag("CircleCoolDown");
        CircleCoolDown_Fill_image = GameObject.FindGameObjectWithTag("CircleCoolDown_Fill").GetComponent<Image>();
        CircleCoolDown_Fill_image.fillAmount = 0;
        CircleCoolDown.SetActive(false);

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

        if (Input.GetMouseButtonDown(0) && !Global_Variables.gameFrozen)
        {
            CircleCoolDown_Fill_image.fillAmount = 0;
            ShootBeam();   
        }

        if (Input.GetMouseButtonDown(1) && !Global_Variables.gameFrozen)
        {
            CircleCoolDown_Fill_image.fillAmount = 0;
            beamSound.Stop();

            if (laserGun.activeSelf == true)
            {
                laserGun.SetActive(false);
            }

            ShootTiles();
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
            CircleCoolDown.SetActive(true);
            laserGun.SetActive(true);
            beamSound.Play();
        }
        else
        {
            CircleCoolDown.SetActive(false);
            laserGun.SetActive(false);
            beamSound.Stop();
        }
    }

    public void ShootTiles()
    {

        if (Global_Variables.ammo > 0)
        {
            //shootSound.Play();

            Vector2 AimVector = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 shotForce = AimVector - (Vector2)laserGun.transform.position;

            GameObject CurrBullet;

            // if mouse is not close to player while shooting
            if (!((AimVector - (Vector2)Player.transform.position).magnitude < minShootingMouseDistance))
            {

                //if aim is directly down and jump
                if ((AimVector.y < transform.position.y) && (Math.Abs(AimVector.x - transform.position.x) < 2) && IsPlayerInAir())
                {
                    CurrBullet = Instantiate(bullet, transform.position - new Vector3(0, 1, 0), Quaternion.identity);
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
                    CurrBullet = Instantiate(bullet, transform.position + (Vector3)shotForce.normalized, Quaternion.identity);
                    CurrBullet.GetComponent<Rigidbody2D>().AddForce(shotForce.normalized * 300);
                    //Vector3 _scale = CurrBullet.transform.localScale;
                    //float _mult = Global_Variables.shotTileMultiplier;
                    //CurrBullet.transform.localScale = new Vector3(_scale.x * _mult, _scale.y * _mult, _scale.z);
                    Global_Variables.ammo--;
                }
            }
            else
            {
                Debug.Log("You are aiming to close to the player");
            }

        }
        else
        {
            noAmmoSound.Play();
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
