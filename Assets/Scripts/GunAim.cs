using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject laserGun;
    private Transform InnerHand;
    private Transform OuterHand;
    private Transform Gun;

    // Start is called before the first frame update
    void Awake()
    {
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
        
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }

    }

    private void SetHandOrientation (Transform obj, Quaternion quaternion, int yScale) {
        obj.rotation = quaternion;
        // maintain its size but alter direction
        Vector3 scale = obj.localScale;
        scale.y = Mathf.Abs(scale.y) * Mathf.Sign(yScale);
        obj.localScale = scale;
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
