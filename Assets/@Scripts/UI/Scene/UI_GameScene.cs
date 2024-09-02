using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Events;
using Data;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

public class UI_GameScene : UI_Base
{
    public virtual int GrenadeCount {  get; set; }  
    public virtual int ConsumableCount {  get; set; }   

    enum GameObjects
    {
        GuageFront,
        ExpSliderObject,
        HpSliderObject,
        BossStageGroup,
        TimeGroup,
    }
    enum Texts
    {
        HPText,
        MaxHPText,
        WaveText,
        TimeText,
        GoldText,
        MaxAmmoText,
        TotalAmmoText,
        GrenadeCountText,
        ConsumableCountText,
    }
    enum Images
    {
        MeleeWeaponImage,
        ManualWeaponImage,
        AutoWeaponImage,
        GrenadeImage,
        ConsumableImage,
    }
    enum Buttons
    {
        AttackButton,
        JumpButton,
        InvenButton,
        ReloadButton,
        DiveButton
    }
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindEvents();
        return true;
    }
    public void BindEvents()
    {
        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnPointerDownAttackButton, null,Define.UIEvent.PointerDown);
        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnPointerUpAttackButton,null,Define.UIEvent.PointerUp);
        GetButton((int)Buttons.JumpButton).gameObject.BindEvent(OnClickJumpButton);
        GetButton((int)Buttons.ReloadButton).gameObject.BindEvent(OnClickReloadButton);
        GetButton((int)Buttons.InvenButton).gameObject.BindEvent(OnClickEquipButton);
        GetImage((int)Images.MeleeWeaponImage).gameObject.BindEvent(OnClickMeleeWeaponSlotImage);
        GetImage((int)Images.ManualWeaponImage).gameObject.BindEvent(OnClickManualWeaponSlotImage);
        GetImage((int)Images.AutoWeaponImage).gameObject.BindEvent(OnClickAutoWeaponSlotImage);
        GetImage((int)Images.GrenadeImage).gameObject.BindEvent(OnClickGrenadeSlotImage);
        GetImage((int)Images.ConsumableImage).gameObject.BindEvent(OnClickConsumableImage);
    }
    public void OnClickEquipButton()
    {
        Managers.UI.ShowPopup<UI_Inventory>();
    }
    public void OnClickMeleeWeaponSlotImage()
    {
        Managers.Game.Player.SwapToMeleeWeapon();
    }
    public void OnClickManualWeaponSlotImage()
    {
        Managers.Game.Player.SwapToManualWeapon();
    }
    public void OnClickAutoWeaponSlotImage()
    {
        Managers.Game.Player.SwapToAutoWeapon();
    }
    public void OnClickGrenadeSlotImage()
    {
        Managers.Game.Player.SwapToGrenade();
    }
    public void OnClickConsumableImage() 
    {
        Managers.Game.Player.Consumable.Use();
    }
    public void OnClickJumpButton()
    {
        Managers.Game.Player.ClickJumpButton();
    }
    public void OnPointerDownAttackButton()
    {
        Managers.Game.Player.ClickAttackButton();
    }
    public void OnPointerUpAttackButton()
    {
        Managers.Game.Player.StopAttack();
    }
    public void OnClickReloadButton()
    {
        Managers.Game.Player.ReloadPlayer();
    }
    public void SetMeleeWeaponSlot(string imageName)
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void ClearMeleeWeaponSlot()
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void SetAutoWeaponSlot(string imageName) 
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void ClearAutoWeaponSlot()
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void SetManualWeaponSlot(string imageName)
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void ClearManualWeaponSlot()
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void SetGrenadeSlot(string imageName) 
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void ClearGrenadeSlot()
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void SetConsumableSlot(string imageName) 
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void ClearConsumableSlot()
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void SetGold(int gold)
    {
        GetText((int)Texts.GoldText).gameObject.GetComponent<TextMeshProUGUI>().text = gold.ToString();
    }
    public void SetGrenadeCount(int count)
    {
        GrenadeCount = count;

        if(GrenadeCount > 0)
            GetText((int)Texts.GrenadeCountText).gameObject.GetComponent<TextMeshProUGUI>().text = GrenadeCount.ToString();
        else
            GetText((int)Texts.GrenadeCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
    public void SetConsumableCount(int count)
    {
        ConsumableCount = count;

        if(ConsumableCount > 0)
            GetText((int)Texts.ConsumableCountText).gameObject.GetComponent<TextMeshProUGUI>().text = ConsumableCount.ToString();
        else
            GetText((int)Texts.ConsumableCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
    public void SetAmmoCount(int count)
    {
        GetText((int)Texts.MaxAmmoText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString(); 
    }
    public void SetNoneWeaponAmmoCount()
    {
        GetText((int)Texts.MaxAmmoText).gameObject.GetComponent<TextMeshProUGUI>().text = "-";
    }
    public void SetTotalAmmoCount(int count)
    {
        GetText((int)Texts.TotalAmmoText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();
    }
    public void SetPlayerMaxHP(int maxHP)
    {
        GetText((int)Texts.MaxHPText).gameObject.GetComponent<TextMeshProUGUI>().text = maxHP.ToString();   
    }
    public void SetPlayerHP(int HP)
    {
        GetText((int)Texts.HPText).gameObject.GetComponent<TextMeshProUGUI>().text = HP.ToString();   
    }
    public void SetGemCountRatio(float ratio)
    {
        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = ratio;
    }
    public void SetHpBar(float ratio)
    {
        GetObject((int)GameObjects.HpSliderObject).GetComponent<Slider>().value = ratio;
    }
    public void SetStageInfo(int stage)
    {
        GetText((int)Texts.WaveText).text = $"Wave {stage}"; 
    }
    public void SetTimeInfo(float time)
    {
        GetText((int)Texts.TimeText).text = SetTimeText((int)time);
    }
    public void SetBossHpBar(float ratio)
    {
        GetObject((int)GameObjects.GuageFront).gameObject.GetComponent<RectTransform>();
    }
    string SetTimeText(int time)
    {
        string timeText = "";
        if (time < 10)
        {
            timeText = $"00:0{time}";
        }
        else if (time < 60)
        {
            timeText = $"00:{time}";
        }
        else if (time < 600)
        {
            if (time % 60 < 10)
                timeText = $"0{time / 60}:0{time % 60}";
            else
                timeText = $"0{time / 60}:{time % 60}";
        }
        else if (time == 10)
        {
            timeText = "10:00";
        }
        return timeText;
    }
}
