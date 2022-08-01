using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Sprite[] AttackLine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate<Sprite>(AttackLine[1], transform);
    }
}
