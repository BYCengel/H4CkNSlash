using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySO : ScriptableObject
{
    public string name;
    public float cooldownTime;
    public float activeTime;
    public int hackBarValue;
    protected static HudController hudController;
    protected static CombatManager combatManager;
    protected static Player player;

    public virtual void UseSkill()
    {
        throw new System.Exception("Didn't specify skill properly.");
    }
    public virtual float GetCooldown()
    {
        return cooldownTime;
    }
}
