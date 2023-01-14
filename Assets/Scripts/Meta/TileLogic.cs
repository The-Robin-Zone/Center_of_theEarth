using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{

    public GameObject bulletTile;
    public GameObject regTile;
    public float xOffset;
    public float yOffset;
    public bool flag = true;

    void OnCollisionEnter2D(Collision2D col)
    {

        if ((col.gameObject.tag == "Ground" || col.gameObject.tag == "StaticGround") && flag == true)
        {
            flag = false;
            xOffset = (float)(Math.Round(bulletTile.transform.position.x * 2) / 2);
            yOffset = (float)(Math.Round(bulletTile.transform.position.y * 2) / 2);

            bulletTile.transform.position= new Vector3(xOffset, yOffset, 0);
            GameObject tile = Instantiate(regTile, bulletTile.transform.position, Quaternion.identity);

            //Vector3 _scale = tile.transform.localScale;
            //float _mult = Global_Variables.shotTileMultiplier;
            //tile.transform.localScale = new Vector3(_scale.x * _mult, _scale.y * _mult, _scale.z);
            Destroy(bulletTile);
        }
        

    }
}
