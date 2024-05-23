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
    enum Buttons
    {
        AttackButton,
        JumpButton,
        SwapButton,
        ReloadButton,
        ThrowButton,
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
        BindEvents();
        return true;
    }
    public void BindEvents()
    {
        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnClickAttackButton);
        GetButton((int)Buttons.JumpButton).gameObject.BindEvent(OnClickJumpButton);
        GetButton((int)Buttons.SwapButton).gameObject.BindEvent(OnClickSwapButton);
        GetButton((int)Buttons.ReloadButton).gameObject.BindEvent(OnClickReloadButton);
        GetButton((int)Buttons.ThrowButton).gameObject.BindEvent(OnClickThrowButton);
        
    }
    public void OnClickJumpButton()
    {
        Managers.Game.Player.ClickJumpButton();
    }
    public void OnClickAttackButton()
    {
        Managers.Game.Player.ClickAttackButton();
    }
    public void OnClickSwapButton()
    {
        Managers.Game.Player.ClickSwapButton();
    }
    public void OnClickThrowButton()
    {
        Managers.Game.Player.ThrowPlayer();
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
