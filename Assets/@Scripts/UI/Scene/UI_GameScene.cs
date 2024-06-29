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
        MeleeWeaponSlotImage,
        RangeWeaponSlotImage,
        GrenadeSlotImage,
    }
    enum Buttons
    {
        AttackButton,
        JumpButton,
        ReloadButton,
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
        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnClickAttackButton);
        GetButton((int)Buttons.JumpButton).gameObject.BindEvent(OnClickJumpButton);
        GetButton((int)Buttons.ReloadButton).gameObject.BindEvent(OnClickReloadButton);
        GetImage((int)Images.MeleeWeaponSlotImage).gameObject.BindEvent(OnClickMeleeWeaponSlotImage);
        GetImage((int)Images.RangeWeaponSlotImage).gameObject.BindEvent(OnClickRangeWeaponSlotImage);
        GetImage((int)Images.GrenadeSlotImage).gameObject.BindEvent(OnClickGrenadeSlotImage);
    }
    public void OnClickMeleeWeaponSlotImage()
    {
        Managers.Game.Player.SwapToMeleeWeapon();
    }
    public void OnClickRangeWeaponSlotImage()
    {
        Managers.Game.Player.SwapToRangeWeapon();
    }
    public void OnClickGrenadeSlotImage()
    {
        Managers.Game.Player.SwapToGrenade();
    }
    public void OnClickJumpButton()
    {
        Managers.Game.Player.ClickJumpButton();
    }
    public void OnClickAttackButton()
    {
        Managers.Game.Player.ClickAttackButton();
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
