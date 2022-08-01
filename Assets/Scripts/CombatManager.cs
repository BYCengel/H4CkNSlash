using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    Player player;

    private static List<Enemy> StunnedEnemies;
    private static List<Enemy> tempStunnedEnemies;
    private static bool isStunnedEnemiesNull = true;
    private static float timer;
    private static float stunSkillTimer;
    private static int stunnedEnemyCounter;

    private static float stunSkillCooldown;
    private static float damageSkillCooldown;
    private static float invisibilitySkillCooldown;
    private static float assasinationSkillCooldown;

    private static bool isStunSkillInCooldown = false;
    private static float damageSkillTimer;
    private static bool isDamageSkillInCooldown = false;
    private static float invisibilitySkillTimer;
    private static bool isInvisibilityInCooldown = false;
    private static float assasinationSkillTimer;
    private static bool isAssasinationInCooldown = false;
    private static bool isAnySkillInCooldown = false;
    private static int cooldownCounter = 0;

    private static float lastHitTakenTimer;
    private static bool isPlayerHPFull = true;
    public static float timeBetweenHitAndRegen = 5f;

    private static bool isHackBarFull = false;
    public int boostedDamage = 30;
    public int baseDamage = 10;


    private HudController hudController;
    void Start()
    {
        StunnedEnemies = new List<Enemy>();
        hudController = FindObjectOfType<HudController>();
        player = FindObjectOfType<Player>();
        SetCooldowns();
    }
    void Update()
    {
        if (!isStunnedEnemiesNull)
        {
            foreach(Enemy enemy in StunnedEnemies)
            {
                if (enemy == null)
                    continue;
                if (Time.time - enemy.GetStunTimer() >= enemy.GetStunDuration())
                {
                    RemoveStunnedEnemy(enemy);
                }
            }
        }
        else
        {
            StunnedEnemies.Clear();
        }
        hudController.HackBarDecay();

        if (isAnySkillInCooldown)
        {
            if (isStunSkillInCooldown)
            {
                if(Time.time - stunSkillTimer >= stunSkillCooldown)
                {
                    isStunSkillInCooldown = false;
                    cooldownCounter--;
                    player.SetCanUseSkills("AOEStun");
                }
            }
            if (isDamageSkillInCooldown)
            {
                if (Time.time - damageSkillTimer >= damageSkillCooldown)
                {
                    isDamageSkillInCooldown = false;
                    cooldownCounter--;
                    player.SetCanUseSkills("AOEDamage");
                }
            }
            if (isInvisibilityInCooldown)
            {
                if (Time.time - invisibilitySkillTimer >= invisibilitySkillCooldown)
                {
                    isInvisibilityInCooldown = false;
                    cooldownCounter--;
                    player.SetCanUseSkills("invisibility");
                }
            }
            if (isAssasinationInCooldown)
            {
                if (Time.time - assasinationSkillTimer >= assasinationSkillCooldown)
                {
                    isAssasinationInCooldown = false;
                    cooldownCounter--;
                    player.SetCanUseSkills("assasination");
                }
            }
            if(cooldownCounter <= 0)
            {
                cooldownCounter = 0;
                isAnySkillInCooldown = false;
            }
        }
        if (!isPlayerHPFull)
        {
            if(Time.time - lastHitTakenTimer > timeBetweenHitAndRegen)
            {
                hudController.HPAutoRegen();
            }
        }
    }
    public void AddStunnedEnemy(Enemy newStunnedEnemy)
    {
        StunnedEnemies.Add(newStunnedEnemy);
        stunnedEnemyCounter++;
        isStunnedEnemiesNull = false;
        Debug.Log("Stunned" + newStunnedEnemy.name);
    }
    private void RemoveStunnedEnemy(Enemy stunnedEnemyToRemove)
    {
        stunnedEnemyToRemove.SetIsStunned(false, 0);
        stunnedEnemyCounter--;
        if (stunnedEnemyCounter == 0)
            isStunnedEnemiesNull = true;
        Debug.Log("Stun ended" + stunnedEnemyToRemove.name);
    }

    public void SetSkillTimer(string skill)
    {
        isAnySkillInCooldown = true;
        switch (skill)
        {
            case "AOEStun":
                stunSkillTimer = Time.time;
                cooldownCounter++;
                isStunSkillInCooldown = true;
                break;
            case "AOEDamage":
                damageSkillTimer = Time.time;
                cooldownCounter++;
                isDamageSkillInCooldown = true;
                break;
            case "invisibility":
                invisibilitySkillTimer = Time.time;
                cooldownCounter++;
                isInvisibilityInCooldown = true;
                break;
            case "assasination":
                assasinationSkillTimer = Time.time;
                cooldownCounter++;
                isAssasinationInCooldown = true;
                break;
        }
    }
    private void SetCooldowns()
    {
        stunSkillCooldown = player.GetStunSkillCooldown();
        damageSkillCooldown = player.GetDamageSkillCooldown();
        invisibilitySkillCooldown = player.GetInvisibilitySkillCooldown();
        assasinationSkillCooldown = player.GetAssasinationSkillCooldown();
    }

    public static void PlayerTookDamage()
    {
        lastHitTakenTimer = Time.time;
        isPlayerHPFull = false;
    }
    public void HackBarReadyForCops()
    {
        LevelManager.instance.level.GetComponent<LevelGeneration>().copCanSpawn = true;
    }
    public static void SetIsPlayerHPFull(bool value)
    {
        isPlayerHPFull = value;
    }
    public void SetIsHackBarFull(bool value)
    {
        isHackBarFull = value;
        if (value)
        {
            isHackBarFull = value;
            player.SetLightAttackDamage(boostedDamage);
        }
        else
        {
            isHackBarFull = value;
            player.SetLightAttackDamage(baseDamage);
        }
    }
}

