using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBg : MonoBehaviour
{
    public float speed;
    [SerializeField]private Renderer bgRenderer;
    private bool isMoving = false;
    void Update()
    {
        if (isMoving) return;
        
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
