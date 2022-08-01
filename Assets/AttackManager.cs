using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public bool canReceiveInput;
    public bool inputReceived;

    public static AttackManager instance;

    private void Awake()
    {
        instance = this;
    }


    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }


    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}
