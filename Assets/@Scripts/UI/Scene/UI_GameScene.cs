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
    GameObject bossStateGroup;
    public virtual int GrenadeCount {  get; set; }  
    public virtual int ConsumableCount {  get; set; }   

    enum GameObjects
    {
        GuageFront,
        BossStateGroup,
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
        ItemSummaryText
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
    protected override void BindEvents()
    {
        bossStateGroup = GetObject((int)GameObjects.BossStateGroup).gameObject;
        bossStateGroup.SetActive(false);
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
    protected override void Subscribe()
    {
        Managers.Game.Player.Consumable.OnConsumableCountChanged -= SetConsumableCount;
        Managers.Game.Player.Consumable.OnConsumableCountChanged += SetConsumableCount;
        Managers.Game.Player.Grenade.OnGrenadeCountChanged -= SetGrenadeCount;
        Managers.Game.Player.Grenade.OnGrenadeCountChanged += SetGrenadeCount;
        Managers.Game.Player.Ammo.OnAmmoCountChanged -= SetTotalAmmoCount;
        Managers.Game.Player.Ammo.OnAmmoCountChanged += SetTotalAmmoCount;
        Managers.Game.Player.Consumable.OnConsumableClear -= ClearConsumableSlot;
        Managers.Game.Player.Consumable.OnConsumableClear += ClearConsumableSlot;
        Managers.Game.Player.Grenade.OnGrenadeClear -= ClearGrenadeSlot;
        Managers.Game.Player.Grenade.OnGrenadeClear += ClearGrenadeSlot;
        Managers.Game.Player.AutoWeapon.OnAutoWeaponClear -= ClearAutoWeaponSlot;
        Managers.Game.Player.AutoWeapon.OnAutoWeaponClear += ClearAutoWeaponSlot;
        Managers.Game.Player.ManualWeapon.OnManualWeaponClear -= ClearManualWeaponSlot;
        Managers.Game.Player.ManualWeapon.OnManualWeaponClear += ClearManualWeaponSlot;
        Managers.Game.Player.MeleeWeapon.OnMeleeWeaponClear -= ClearMeleeWeaponSlot;
        Managers.Game.Player.MeleeWeapon.OnMeleeWeaponClear += ClearMeleeWeaponSlot;
        Managers.Game.Player.OnSetMeleeWeapon -= SetMeleeWeaponSlot;
        Managers.Game.Player.OnSetMeleeWeapon += SetMeleeWeaponSlot;
        Managers.Game.Player.OnSetManualWeapon -= SetManualWeaponSlot;
        Managers.Game.Player.OnSetManualWeapon += SetManualWeaponSlot;
        Managers.Game.Player.OnSetAutoWeapon -= SetAutoWeaponSlot;
        Managers.Game.Player.OnSetAutoWeapon += SetAutoWeaponSlot;
        Managers.Game.Player.OnSetGrenade -= SetGrenadeSlot;
        Managers.Game.Player.OnSetGrenade += SetGrenadeSlot;
        Managers.Game.Player.OnSetConsumable -= SetConsumableSlot;
        Managers.Game.Player.OnSetConsumable += SetConsumableSlot;
        Managers.Game.Player.OnSetAmmo -= SetAmmoText;
        Managers.Game.Player.OnSetAmmo += SetAmmoText;
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
        Managers.Game.Player.UsePotion();
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
    public void SetMeleeWeaponSlot(CreatureController owner,ItemData itemData)
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetAutoWeaponSlot(CreatureController owner, ItemData itemData)
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetManualWeaponSlot(CreatureController owner, ItemData itemData)
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetGrenadeSlot(CreatureController owner, ItemData itemData, int count)
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetConsumableSlot(CreatureController owner, ItemData itemData, int count)
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void ClearMeleeWeaponSlot()
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = null;
        SetNoneWeaponAmmoCount();
    }
    public void ClearAutoWeaponSlot()
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = null;
        SetNoneWeaponAmmoCount();
    }
    public void ClearManualWeaponSlot()
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = null;
        SetNoneWeaponAmmoCount();
    }
    public void ClearGrenadeSlot()
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = null;
        GetText((int)Texts.GrenadeCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
    public void ClearConsumableSlot()
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = null;
        GetText((int)Texts.ConsumableCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
    public void SetGold(int gold)
    {
        GetText((int)Texts.GoldText).gameObject.GetComponent<TextMeshProUGUI>().text = gold.ToString();
    }
    public void SetGrenadeCount(int count)
    {
        if (count == 0)
            return;
        GetText((int)Texts.GrenadeCountText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();            
    }
    public void SetConsumableCount(int count)
    {
        if (count == 0) 
            return; 
        GetText((int)Texts.ConsumableCountText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();
    }
    public void SetAmmoText(int count)
    {
        switch(Managers.Game.Player.PlayerWeaponType)
        {
            case Define.PlayerWeaponType.None:
                SetNoneWeaponAmmoCount();
                break;
            case Define.PlayerWeaponType.Melee:
                SetNoneWeaponAmmoCount();
                break;
            case Define.PlayerWeaponType.Manual:
                SetWeaponAmmoCount(count);
                break;
            case Define.PlayerWeaponType.Auto:
                SetWeaponAmmoCount(count);
                break;
            case Define.PlayerWeaponType.Grenade:
                SetNoneWeaponAmmoCount();
                break;
        }
    }
    public void SetWeaponAmmoCount(int count)
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
    public void SetStageInfo(int stage)
    {
        GetText((int)Texts.WaveText).text = $"Wave {stage}"; 
    }
    public void SetTimeInfo(int time)
    {
        GetText((int)Texts.TimeText).text = SetTimeText((int)time);
    }
    public void MonsterInfoUpdate(MonsterController boss)
    {
        if(boss.CreatureState != Define.CreatureState.Dead)
        {
            bossStateGroup.SetActive(true);
            GetObject((int)GameObjects.GuageFront).gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)boss.HP/(float)boss.MaxHP, 1, 1);
        }
        else if(boss.CreatureState == Define.CreatureState.Dead)
        {
            bossStateGroup.SetActive(false);
        }
    }
    public void PlayerInfoUpdate(PlayerController  player)
    {
        GetText((int)Texts.HPText).gameObject.GetComponent<TextMeshProUGUI>().text = player.HP.ToString();
        GetText((int)Texts.MaxHPText).gameObject.GetComponent<TextMeshProUGUI>().text = player.MaxHP.ToString();
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
