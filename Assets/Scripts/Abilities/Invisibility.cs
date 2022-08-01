using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Invisibility : AbilitySO
{
    Transform cyberPolat;
    public float transparencyModifier = 0.2f;
    public override void UseSkill()
    {
        if (combatManager == null)
            combatManager = FindObjectOfType<CombatManager>();
        if (hudController == null)
            hudController = FindObjectOfType<HudController>();
        if (player == null)
            player = FindObjectOfType<Player>();
        if (!player.GetIsInvisible())
        {
            player.SetInvisibilityDuration(activeTime);
            cyberPolat = player.transform.Find("CyberPolat_Torso");
            /*foreach(Transform child in cyberPolat.transform)
            {
                Debug.Log("invisibility");
                Color childColor = child.GetComponent<SpriteRenderer>().color;
                Debug.Log(childColor);
                childColor = new Color(childColor.r, childColor.g, childColor.b, transparencyModifier);
                Debug.Log("after " + childColor);
            }*/
            Color color = cyberPolat.GetComponent<SpriteRenderer>().color;
            cyberPolat.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, transparencyModifier);
            RecursiveColorHandler(cyberPolat.transform);

            player.SetInvisibility(true);
            combatManager.SetSkillTimer("invisibility");
            hudController.AddToHackBar(hackBarValue);
        }
        else
        {
            ColorFixer();
        }
    }
    private void RecursiveColorHandler(Transform transformOfObject)
    {
        foreach(Transform child in transformOfObject)
        {
            if (child.GetComponent<SpriteRenderer>() == null)
                continue;
            Color childColor = child.GetComponent<SpriteRenderer>().color;
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color(childColor.r, childColor.g, childColor.b, transparencyModifier);
            if (child.childCount > 0)
                RecursiveColorHandler(child);
        }
    }
    public void ColorFixer()
    {
        Color color = cyberPolat.GetComponent<SpriteRenderer>().color;
        cyberPolat.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f);
        RecursiveColorFixer(cyberPolat.transform);
        player.SetInvisibility(true);
    }
    private void RecursiveColorFixer(Transform transformOfObject)
    {
        foreach (Transform child in transformOfObject)
        {
            if (child.GetComponent<SpriteRenderer>() == null)
                continue;
            Color childColor = child.GetComponent<SpriteRenderer>().color;
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color(childColor.r, childColor.g, childColor.b, 1f);
            if (child.childCount > 0)
                RecursiveColorHandler(child);
        }
    }
}
