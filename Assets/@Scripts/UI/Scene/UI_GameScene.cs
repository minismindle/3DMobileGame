using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Events;

public class UI_GameScene : UI_Base
{
    enum GameObjects
    {
        ExpSliderObject,
        HpSliderObject,
    }
    enum Texts
    {
        WaveText,
        TimeLimitValueText,
        KillValueText,
        CharacterLevelValueText,
    }
    enum Images
    {
        MeleeWeaponImage,
        ManualWeaponImage,
        AutoWeaponImage,
        GrenadeImage,
    }
    enum Buttons
    {
        AttackButton,
        JumpButton,
        InvenButton,
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
        GetButton((int)Buttons.InvenButton).gameObject.BindEvent(OnClickEquipButton);
        GetImage((int)Images.MeleeWeaponImage).gameObject.BindEvent(OnClickMeleeWeaponSlotImage);
        GetImage((int)Images.ManualWeaponImage).gameObject.BindEvent(OnClickManualWeaponSlotImage);
        GetImage((int)Images.AutoWeaponImage).gameObject.BindEvent(OnClickAutoWeaponSlotImage);
        GetImage((int)Images.GrenadeImage).gameObject.BindEvent(OnClickGrenadeSlotImage);
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
    public void SetGemCountRatio(float ratio)
    {
        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = ratio;
    }

    public void SetKillCount(int killCount)
    {
        GetText((int)Texts.KillValueText).text = $"{killCount}";
    }

    public void SetHpBar(float ratio)
    {
        GetObject((int)GameObjects.HpSliderObject).GetComponent<Slider>().value = ratio;
    }

    public void SetStageInfo(int stage)
    {
        GetText((int)Texts.WaveText).text = $"Wave {stage}"; 
    }

    public void SetTimeInfo(string time)
    {
        GetText((int)Texts.TimeLimitValueText).text = time;
    }

    public void SetCharaterLevelInfo(int charaterLevel)
    {
        GetText((int)Texts.CharacterLevelValueText).text = $"{charaterLevel}";
    }
}
