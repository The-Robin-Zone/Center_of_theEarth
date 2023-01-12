using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Global_Variables : MonoBehaviour
{
    public static int ammo;
    public int life = 3;
    public TextMeshProUGUI ammoAmount;
    public TextMeshProUGUI lifeAmount;

    public void Update()
    {
        ammoAmount.text = ammo.ToString();
        lifeAmount.text = life.ToString();

        Debug.Log("ammo: " + ammoAmount.text);
        Debug.Log("life: " + lifeAmount.text);
    }
}