using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    //TODO
    //skill butonları üzerine uğraşmak gerekecek. şimdilik sadece sliderlar var.
    public Player player;
    public Slider HPSlider;
    public Slider HackSlider;

    public Slider stunSkillSlider;
    public Slider damageSkillSlider;
    public Slider invisibilitySlider;
    public Slider assasinationSlider;

    public CombatManager combatManager;

    public static float decayModifier = 1.5f;
    public static float HPRegenModifier = 1.5f;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        combatManager = FindObjectOfType<CombatManager>();
        SetSliders(100, 100, 0, 100);
        SetSkillSliders();
    }

    private void Update()
    {
        if(stunSkillSlider.value < stunSkillSlider.maxValue)
            stunSkillSlider.value += Time.deltaTime;
        if (damageSkillSlider.value < damageSkillSlider.maxValue)
            damageSkillSlider.value += Time.deltaTime;
        if (invisibilitySlider.value < invisibilitySlider.maxValue)
            invisibilitySlider.value += Time.deltaTime;
        if (assasinationSlider.value < assasinationSlider.maxValue)
            assasinationSlider.value += Time.deltaTime;


    }

    public void SetSliders(int HPValue, int HPMaxValue, int hackValue, int hackMaxValue)
    {
        HPSlider.maxValue = HPMaxValue;
        HPSlider.value = HPValue;
        HackSlider.maxValue = hackMaxValue;
        HackSlider.value = hackValue;
    }
    public void SetSkillSliders()
    {
        stunSkillSlider.maxValue = player.GetStunSkillCooldown();
        damageSkillSlider.maxValue = player.GetDamageSkillCooldown();
        invisibilitySlider.maxValue = player.GetInvisibilitySkillCooldown();
        assasinationSlider.maxValue = player.GetAssasinationSkillCooldown();

        stunSkillSlider.value = stunSkillSlider.maxValue;
        damageSkillSlider.value = damageSkillSlider.maxValue;
        invisibilitySlider.value = invisibilitySlider.maxValue;
        assasinationSlider.value = assasinationSlider.maxValue;
    }
    public void SetHealthBarValue(float newValue)
    {
        HPSlider.value = newValue;
    }
    public void SetHackBarValue(float newValue)
    {
        HackSlider.value = newValue;
        if (HackSlider.value > HackSlider.maxValue)
            HackSlider.value = HackSlider.maxValue;
        if (HackSlider.value >= HackSlider.maxValue)
            combatManager.HackBarReadyForCops();
    }
    public void AddToHackBar(float value)
    {
        HackSlider.value += value;
        if (HackSlider.value > HackSlider.maxValue)
            HackSlider.value = HackSlider.maxValue;
        if (HackSlider.value > HackSlider.maxValue - 10f)
            combatManager.SetIsHackBarFull(true);
        if (HackSlider.value >= HackSlider.maxValue)
            combatManager.HackBarReadyForCops();
            
    }
    public void DecreaseFromHackBar(float value)
    {
        HackSlider.value += value;
        if (HackSlider.value < HackSlider.maxValue - 10f)
            combatManager.SetIsHackBarFull(false);
    }
    public void HackBarDecay()
    {
        if(HackSlider.value <= 0)
        {
            HackSlider.value = 0;
            return;
        }
        HackSlider.value -= (Time.deltaTime * decayModifier);
        if (HackSlider.value < HackSlider.maxValue - 10f)
            combatManager.SetIsHackBarFull(false);
    }
    public void HPAutoRegen()
    {
        if(HPSlider.value >= HPSlider.maxValue)
        {
            HPSlider.value = HPSlider.maxValue;
            CombatManager.SetIsPlayerHPFull(true);
            return;
        }
        HPSlider.value += (Time.deltaTime * HPRegenModifier);
    }
    public void SetSkillCooldownSlider(string slider, float value)
    {
        if (value < 0)
            value = 0;
        switch (slider)
        {
            case "stunSlider":
                stunSkillSlider.value = value;
                break;
            case "damageSlider":
                damageSkillSlider.value = value;
                break;
            case "invisibilitySlider":
                invisibilitySlider.value = value;
                break;
            case "assasinationSlider":
                assasinationSlider.value = value;
                break;
        }
    }
}
