using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Global_Variables : MonoBehaviour
{
    public static int ammo;
    public static int max_life = 3;
    public static int life = max_life;
    public TextMeshProUGUI ammoAmount;
    public TextMeshProUGUI lifeAmount;
    public TextMeshProUGUI ShootType;

    public void Awake()
    {
        //ammoAmount = GameObject.Find("HUDAmmoAmount").GetComponent<TextMeshProUGUI>();
        //lifeAmount = GameObject.Find("HUDLivesAmount").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        ammoAmount.text = ammo.ToString();
        lifeAmount.text = life.ToString();
        GameObject scopeCenter = GameObject.Find("ScopeCenter");
        if (scopeCenter != null)
        {
            ShootType.text = scopeCenter.GetComponent<GunAim>().shootType;
        }
    }
}
