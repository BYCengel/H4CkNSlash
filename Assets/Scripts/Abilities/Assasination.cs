using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Assasination : AbilitySO
{
    public int damage = 50;
    public float skillRange = 7f;

    Enemy selectedEnemy;

    public override void UseSkill()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
        if (combatManager == null)
            combatManager = FindObjectOfType<CombatManager>();
        if (hudController == null)
            hudController = FindObjectOfType<HudController>();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //get mouse position
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
        selectedEnemy = Enemy.GetClosestEnemy(mousePosition, skillRange);
        if (selectedEnemy == null)
            return;

        player.transform.position = selectedEnemy.transform.position - selectedEnemy.transform.right.normalized;
        selectedEnemy.Damage(damage);
        combatManager.SetSkillTimer("assasination");
        hudController.AddToHackBar(hackBarValue);
    }
}
