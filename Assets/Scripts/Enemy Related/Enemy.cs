using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    //TODO Quaternion.identitiy yerine particle'lara yön verecek kod lazım Damage() kodunda
    
    private static List<Enemy> enemyList;
    private static List<Enemy> enemiesInRange;

    private CombatManager combatManager;
    public GameObject blood;
    public int currentHP;
    public int maxHP = 20;
    private bool isStunned = false;
    private float stunDuration;
    private float stunTimer;

    private float bloodVFXDuration = 1f;
    private Animator anim;

    private void Start()
    {
        currentHP = maxHP;
        if (enemyList == null) enemyList = new List<Enemy>();
        enemyList.Add(this);
        combatManager = FindObjectOfType<CombatManager>();
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    public static Enemy GetClosestEnemy(Vector3 position, float range)// static because we only need one of this function
    {//basic list search algorithm O(n)
        if (enemyList == null)
            return null;
        Enemy closestEnemy = null;
        Enemy testEnemy = null;
        for(int i = 0; i < enemyList.Count; i++)
        {
            testEnemy = enemyList[i];
            if(Vector3.Distance(position, testEnemy.transform.position) > range)
            {
                // enemy too far, skip
                continue;
            }
            if(closestEnemy == null)
            {
                // no closest enemy
                closestEnemy = testEnemy;
            }
            else
            {
                //already has a closest enemy, get which is closer
                if(Vector3.Distance(position, testEnemy.transform.position) <
                    Vector3.Distance(position, closestEnemy.transform.position))
                {
                    // test enemy is closer than closest enemy
                    closestEnemy = testEnemy;
                }
            }
        }
        return closestEnemy;
    }
    public static List<Enemy> GetEnemiesInRange(Vector3 position, float range)
    {
        if (enemiesInRange == null)
            enemiesInRange = new List<Enemy>();
        else
            enemiesInRange.Clear();
        for(int i = 0; i < enemyList.Count; i++)
        {
            Enemy testEnemy = enemyList[i];
            if (Vector3.Distance(position, testEnemy.transform.position) > range)
                continue; //fazla uzak
            enemiesInRange.Add(testEnemy);
        }
        if (enemiesInRange == null)
            return null;
        return enemiesInRange;
    }

    public void Damage(int dmg) // geri sekmece mevzusunu buraya ekleyeceğiz.
    {
        currentHP -= dmg;//TODO Quaternion.identitiy yerine particle'lara yön verecek kod lazım
        anim.SetTrigger("GetHit");
        
          Vector3 relative = transform.position - PlayerManager.instance.player.transform.position;
          float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            // BİTTİ ____ Player pozisyonuna erişilmeye başlandığı zaman Quaternion.identity yerine yazılacak ve üst iki satır açılacak ---> Quaternion.Euler(0, 0, angle);
         
        GameObject bloodVFX = Instantiate (blood, transform.position, Quaternion.Euler(0,0,angle));
        if(currentHP <= 0)
        {
            Debug.Log(name + " is dead!");
            LevelManager.instance.level.GetComponent<LevelGeneration>().roomKill += 1;
            Die();
        }
        Destroy(bloodVFX, bloodVFXDuration);
    }

    private void Die()
    {
        //enemy dead animation here
        enemyList.Remove(this);
        Destroy(gameObject);
    }

    public void SetIsStunned(bool value, float st)
    {
        if (value)
        {
            isStunned = value;
            stunDuration = st;
            stunTimer = Time.time;
            combatManager.AddStunnedEnemy(this);
            anim.SetBool("Stunned", value);
        }
        else
        {
            isStunned = value;
            anim.SetBool("Stunned", value);
        }
        
    }
    public bool GetIsStunned()
    {
        return isStunned;
    }
    public float GetStunDuration()
    {
        Debug.Log("stunTime " + stunDuration);
        return stunDuration;
    }
    public float GetStunTimer()
    {
        Debug.Log("stunTimer " + stunTimer);
        return stunTimer;
    }
}
