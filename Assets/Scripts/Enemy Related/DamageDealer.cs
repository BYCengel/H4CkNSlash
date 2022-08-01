using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 100;
    public int moveSpeed;
    public Vector3 target;
    private float timer;
    private Rigidbody2D rb;

    private float xSpeed;
    private float ySpeed;

    public GameObject player;

    private void Start()
    {
        player = PlayerManager.instance.player;
        timer = Time.time;
        rb = GetComponent<Rigidbody2D>();


        Vector3 relative = PlayerManager.instance.player.transform.position - transform.position;
        float angle = Mathf.Atan2(relative.y, relative.x);

        xSpeed = moveSpeed * Mathf.Cos(angle);
        ySpeed = moveSpeed * Mathf.Sin(angle);


        rb.velocity = new Vector2(xSpeed, ySpeed);

        //SetTarget(player.transform.position);
        //rb.AddForce(player.transform.position * 1f,ForceMode2D.Impulse);
        //MoveTo(target);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Die();
    }

    private void Update()
    {
        //MoveTo(target);
        /*if(target == transform.position)
        {
            Die();
        }*/
    }
    private void MoveTo(Vector2 targetPosition)
    {
       
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void Die()
    {
        //projectileın yok olma animasyonu buraya geliyor
        Destroy(gameObject);
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;//* 2
    }
}
