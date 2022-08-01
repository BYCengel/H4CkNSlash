using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AOEStunAbility : AbilitySO
{
    public float skillRange = 2f;
    public override void UseSkill()
    {
        if(player == null)
            player = FindObjectOfType<Player>();
        List<Enemy> enemiesInArea = Enemy.GetEnemiesInRange(player.GetAOEIndicator().transform.position, skillRange);

        foreach(Enemy enemy in enemiesInArea)
        {
            enemy.SetIsStunned(true, activeTime);
        }

        if (combatManager == null)
            combatManager = FindObjectOfType<CombatManager>();
        combatManager.SetSkillTimer("AOEStun");

        if (hudController == null)
            hudController = FindObjectOfType<HudController>();
        hudController.AddToHackBar(hackBarValue);
    }
}
