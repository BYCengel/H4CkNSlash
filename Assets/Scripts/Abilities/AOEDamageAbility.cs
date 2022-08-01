using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AOEDamageAbility : AbilitySO
{
    public float skillRange = 2f;
    public int damage = 30;
    public override void UseSkill()
    {
        if(player == null)
            player = FindObjectOfType<Player>();

        List<Enemy> enemiesInArea = Enemy.GetEnemiesInRange(player.GetAOEIndicator().transform.position, skillRange);

        foreach (Enemy enemy in enemiesInArea)
        {
            enemy.Damage(damage);
        }

        if (combatManager == null)
            combatManager = FindObjectOfType<CombatManager>();
        combatManager.SetSkillTimer("AOEDamage");

        if(hudController == null)
            hudController = FindObjectOfType<HudController>();
        hudController.AddToHackBar(hackBarValue);
    }
}
