using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Renderer : MonoBehaviour
{

    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] textures;

    private int animationStep;

    [SerializeField]
    private float fps = 5f;

    private float fpsCounter;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        fpsCounter += Time.deltaTime;

        if (fpsCounter >= 1f/fps)
        {
            animationStep++;

            if (animationStep == textures.Length)
            {
                animationStep = 0;
            }

            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);

            fpsCounter = 0f;
        }
    }
}
