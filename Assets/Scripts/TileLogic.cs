using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{

    public GameObject thisTile;
    public GameObject regTile;
    public float xOffset;
    public float yOffset;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Ground")
        {
            Debug.Log("collided with Ground");
            xOffset = (float)(Math.Round(thisTile.transform.position.x * 2) / 2);
            yOffset = (float)(Math.Round(thisTile.transform.position.y * 2) / 2);

            thisTile.transform.position= new Vector3(xOffset, yOffset, 0);
            Instantiate(regTile, thisTile.transform.position, Quaternion.identity);
            Destroy(thisTile);
        }
        

    }
}
