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
using System.Runtime.InteropServices;

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
        ConsumableImage
    }
    enum Texts
    {
        GoldText,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));  
        BindText(typeof(Texts)); 
        slots = GetObject((int)GameObjects.Slots).gameObject.GetComponentsInChildren<UI_InventorySlot>();
        Managers.Game.Player.Inventory = this.gameObject.GetComponent<UI_Inventory>();
        BindEvents();
        return true;
    }
    protected override void BindEvents()
    {
        GetButton((int)Buttons.ExitInventoryButton).gameObject.BindEvent(OnClickExitInventoryButton);
        GetImage((int)Images.MeleeWeaponImage).gameObject.BindEvent(OnClickMeleeWeaponImage);
        GetImage((int)Images.ManualWeaponImage).gameObject.BindEvent(OnClickManualWeaponImage);
        GetImage((int)Images.AutoWeaponImage).gameObject.BindEvent(OnClickAutoWeaponImage);
        GetImage((int)Images.GrenadeImage).gameObject.BindEvent(OnClickGrenadeImage);
        GetImage((int)Images.ConsumableImage).gameObject.BindEvent(OnClickConsumableImage);
    }
    protected override void Subscribe()
    {
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
    }
    public void InsertItem(ItemData itemData,int count)
    {
        switch(itemData.ItemType)
        {
            case ItemType.Ammo:
                InsertAmmo(itemData, count);
                break;
            case ItemType.Consumable:
                InsertConsumable(itemData,count);
                break;
            case ItemType.Grenade:
                InsertGrenade(itemData, count); 
                break;
            case ItemType.Weapon:
                GetEmptySlot(itemData, count);  
                break;
        }
        SortingSlot();
        BubbleSort();
    }
    void InsertAmmo(ItemData itemData, int count)
    {
        Managers.Game.Player.Ammo.Count += count;
        GetEmptySlot(itemData, count);
    }
    void InsertConsumable(ItemData itemData, int count)
    {
        if (Managers.Game.Player.Consumable.Equip)
        {
            Managers.Game.Player.Consumable.Count += count;
        }
        else if (!Managers.Game.Player.Consumable.Equip)
        {
            GetEmptySlot(itemData, count);
        }
    }
    void InsertGrenade(ItemData itemData,int count)
    {
        if (Managers.Game.Player.Grenade.Equip)
        {
            Managers.Game.Player.Grenade.Count += count;
        }
        else if (!Managers.Game.Player.Grenade.Equip)
        {
            GetEmptySlot(itemData, count);
        }
    }
    public void GetEmptySlot(ItemData itemData, int count) 
    {
        if (itemData.Consumable) //한칸에 여러개를 넣을 수 있을 때 
        {
            foreach (UI_InventorySlot slot in slots)
            {
                if (slot.Empty)
                {
                    slot.SetSlot(itemData, count);
                    return;
                }
                else if(!slot.Empty)
                {
                    if (itemData.Name != slot.ItemData.Name)
                        continue;
                    if(slot.Count == MaxSlotCount)
                    {
                        continue;
                    }
                    else if(slot.Count < MaxSlotCount)
                    {
                        int _sumCount = slot.Count + count;
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
        else if (!itemData.Consumable) //한칸에 한개의 아이템만 넣을 수 있을 때
        {
            foreach(UI_InventorySlot slot in slots)
            {
                if(slot.Empty)
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
            if (slots[i].Empty)
            {
                for (int j = i+1; j < slots.Length; j++) 
                {
                    if (slots[j].Empty)
                        return;
                    slots[j-1].SetSlot(slots[j].ItemData, slots[j].Count);
                    slots[j].ClearSlot();   
                }
                return;
            }
        }
        return;
    }
    public void BubbleSort()
    {
        for (int i = 0; i < slots.Length - 1; i++)
        {
            if (slots[i].Empty)
                continue;
            for (int j = 0; j < slots.Length - 1; j++) 
            {
                if(slots[j+1].Empty)
                    continue;
                if (slots[j].ItemData.ItemType > slots[j + 1].ItemData.ItemType)
                {
                    ItemData itemData = slots[j + 1].ItemData;
                    int count = slots[j + 1].Count;
                   
                    slots[j + 1].SetSlot(slots[j].ItemData, slots[j].Count);
                    slots[j].SetSlot(itemData, count); 
                }
            }
        }
        return;
    }
    public void UseItem(string itemName, int count)
    {
        int idx = FindSlotIdx(itemName);

        if (idx == -1) 
            return;

        slots[idx].Count -= count;

        if(slots[idx].Count == 0 )
        {
            slots[idx].ClearSlot();
        }
    }
    public void ReturnItem(ItemData itemData, int count)
    {
        if (count == 0) return;

        int idx = FindSlotIdx(itemData.Name);

        if (idx == -1)
        {
            Managers.Game.Player.Ammo.Count -= count;
            InsertItem(itemData, count);    
            return;
        }

        slots[idx].Count += count;
    }
    int FindSlotIdx(string itemName )
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i].Empty)
                continue;
            if (slots[i].ItemData.Name == itemName)
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
    public void SetGrenadeSlot(CreatureController owner, ItemData itemData,int count)
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetConsumableSlot(CreatureController owner, ItemData itemData, int count)
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemData.Image);
    }
    public void SetGold(int gold)
    {
        GetText((int)Texts.GoldText).gameObject.GetComponent<TextMeshProUGUI>().text = gold.ToString();
    }
    public void ClearMeleeWeaponSlot()
    {
        GetImage((int)Images.MeleeWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void ClearAutoWeaponSlot()
    {
        GetImage((int)Images.AutoWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void ClearManualWeaponSlot()
    {
        GetImage((int)Images.ManualWeaponImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void ClearGrenadeSlot()
    {
        GetImage((int)Images.GrenadeImage).gameObject.GetComponent<Image>().sprite = null;
    }
    public void ClearConsumableSlot()
    {
        GetImage((int)Images.ConsumableImage).gameObject.GetComponent<Image>().sprite = null;
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
    public void OnClickGrenadeImage()
    {
        Managers.Game.Player.ReturnGrenadeToInventory();    
    }
    public void OnClickConsumableImage() 
    { 
        Managers.Game.Player.ReturnConsumableToInventory();
    }
}
