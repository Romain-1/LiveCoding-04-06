using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool isTriggered;

    private void OnTriggerStay2D(Collider2D other)
    {
        isTriggered = true;
    }
}
