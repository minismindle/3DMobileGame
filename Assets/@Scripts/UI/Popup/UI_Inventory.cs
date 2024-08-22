using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using static Define;
using TMPro;

public class UI_Inventory : UI_Base
{
    public UI_InventorySlot[] slots;
    public UI_InventorySlot selectedSlot;
    enum GameObjects
    {
        Slots,
    }
    
    enum Buttons
    {
        ExitInventoryButton
    }
    enum Images
    {
        MeleeWeaponImage,
        ManualWeaponImage,
        AutoWeaponImage,
        GrenadeImage,
        PotionImage
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
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));  
        slots = GetObject((int)GameObjects.Slots).gameObject.GetComponentsInChildren<UI_InventorySlot>();
        Managers.Game.Player.Inventory = this.gameObject.GetComponent<UI_Inventory>();
        BindEvents();
        return true;
    }
    public void BindEvents()
    {
        GetButton((int)Buttons.ExitInventoryButton).gameObject.BindEvent(OnClickExitInventoryButton);
        GetImage((int)Images.MeleeWeaponImage).gameObject.BindEvent(OnClickMeleeWeaponImage);
        GetImage((int)Images.ManualWeaponImage).gameObject.BindEvent(OnClickManualWeaponImage);
        GetImage((int)Images.AutoWeaponImage).gameObject.BindEvent(OnClickAutoWeaponImage);
    }
    public void InsertItem(ItemData itemData,int count)
    {
        switch(itemData.ItemType)
        {
            case ItemType.Ammo:
                Managers.Game.TotalAmmo += count;
                break;
            case ItemType.Potion:
                Managers.Game.Potion += count;  
                break;
            case ItemType.Grenade:
                Managers.Game.Grenade += count;
                break;
        }
        GetEmptySlot(itemData, count);
    }
    public void GetEmptySlot(ItemData itemData, int count) 
    {
        if (itemData.Consumable) //한칸에 여러개를 넣을 수 있을 때 ex) 물약,탄...
        {
            foreach (UI_InventorySlot slot in slots)
            {
                if (slot._empty)
                {
                    slot.SetSlot(itemData, count);
                    return;
                }
                else if(!slot._empty)
                {
                    if (itemData.DataId != slot.itemData.DataId)
                        continue;
                    if(slot._count == MaxSlotCount)
                    {
                        continue;
                    }
                    else if(slot._count < MaxSlotCount)
                    {
                        int _sumCount = slot._count + count;
                        if (_sumCount > MaxSlotCount)
                        {
                            slot.SetCountText(MaxSlotCount);
                            GetEmptySlot(itemData,_sumCount - MaxSlotCount);
                        }
                        else if(_sumCount <= MaxSlotCount)
                        {
                            slot.SetCountText(_sumCount);
                        }
                        return;
                    }
                }
            }
        }
        else if (!itemData.Consumable) //한칸에 한개의 아이템만 넣을 수 있을 때 ex) 장비
        {
            foreach(UI_InventorySlot slot in slots)
            {
                if(slot._empty)
                {
                    slot.SetSlot(itemData,count);
                    return;
                }
            }
        }
        return;
    }
    public void SortingSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i]._empty)
            {
                for (int j = i+1; j < slots.Length; j++) 
                {
                    if (slots[j]._empty)
                        return;
                    slots[j-1].SetSlot(slots[j].itemData, slots[j]._count);
                    slots[j].ClearSlot();   
                }
                return;
            }
        }
        return;
    }
    public void UseItem(ItemType itemType,int useCount)
    {
        int slotIdx = FindSlotIdx(itemType);
        
        if (slotIdx == -1)
            return;

        switch (itemType) 
        {
            case ItemType.Ammo:
                break;
            case ItemType.Grenade:
                break;
            case ItemType.Potion:
                break;
        }
    }
    int FindSlotIdx(ItemType itemType)
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i]._empty)
                continue;
            if (slots[i].itemData.ItemType == itemType)
            {
                return i;
            }
        }
        return -1;
    }
    public void OnClickExitInventoryButton()
    {
        Managers.UI.ClosePopup();
    }
    public void SetMeleeWeaponSlot(string imageName)
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void SetAutoWeaponSlot(string imageName)
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void SetManualWeaponSlot(string imageName)
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void SetGrenadeSlot(string imageName, int count)
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void SetPotionSlot(string imageName, int count)
    {
        GetImage((int)Images.PotionImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imageName);
    }
    public void OnClickMeleeWeaponImage()
    {
        Managers.Game.Player.ReturnMeleeWeaponToInventory();
    }
    public void OnClickManualWeaponImage()
    {
        Managers.Game.Player.ReturnManualWeaponToInventory();
    }
    public void OnClickAutoWeaponImage()
    {
        Managers.Game.Player.ReturnAutoWeaponToInventory();
    }
}
