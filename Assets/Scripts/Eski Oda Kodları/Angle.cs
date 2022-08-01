using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        float angles = Quaternion.Angle(transform.rotation, target.rotation);
        
        Debug.Log("açı -> " + angles);
    }
}
