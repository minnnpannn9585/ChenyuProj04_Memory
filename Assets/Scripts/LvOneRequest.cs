using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvOneRequest : MonoBehaviour
{
    private float timer = 5f;
    public GameObject requestUi;
    bool isShowing = false;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !isShowing)
        {
            requestUi.SetActive(true);
            isShowing = true;
        }
    }
}
