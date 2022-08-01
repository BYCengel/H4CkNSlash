using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject projectile;
    private void Awake()
    {
        StartCoroutine(FireProjectile());
    }

    IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(projectile, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
