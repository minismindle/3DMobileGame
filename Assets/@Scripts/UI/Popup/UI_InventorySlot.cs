using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;


public class UI_InventorySlot : UI_Base
{
    private ItemData _itemData;
    private bool _empty;
    private bool _using;
    private int _count;
    public virtual ItemData ItemData {  get { return _itemData; } set { _itemData = value; } }
    public virtual bool Empty {  get { return _empty; } set { _empty = value; } }
    public virtual bool Using { get { return _using; } set { _using = value; } }
    public virtual int Count {  get { return _count; } set { _count = value; SetCountText(_count); } }    
    enum Images
    {
        SlotItemImage,
    }
    enum Texts
    {
        SlotCountText,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return base.Init();
        _empty = true;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindEvents();
        return true;
    }
    protected override void BindEvents()
    {
        this.gameObject.BindEvent(SelectSlot);
    }
    public void SelectSlot()
    {
        if (_empty)
            return;
        if (_itemData.ItemType == ItemType.Ammo)
            return;
        this.gameObject.transform.root.GetChild(0).GetComponent<UI_Inventory>().selectedSlot 
            = this.gameObject.GetComponent<UI_InventorySlot>();
        ReturnEquipItem();
        Managers.Game.Player.EquipItem(_itemData, Count);
        ClearSlot();
        Managers.Game.Player.Inventory.SortingSlot();
        Managers.Game.Player.Inventory.BubbleSort();
    }
    public void ReturnEquipItem()
    {
        switch (_itemData.ItemType)
        {
            case ItemType.Weapon:
                switch (_itemData.WeaponType)
                {
                    case WeaponType.Melee:
                        Managers.Game.Player.ReturnMeleeWeaponToInventory();
                        break;
                    case WeaponType.Manual:
                        Managers.Game.Player.ReturnManualWeaponToInventory();
                        break;
                    case WeaponType.Auto:
                        Managers.Game.Player.ReturnAutoWeaponToInventory(); 
                        break;
                }
                break;
            case ItemType.Consumable:
                Managers.Game.Player.ReturnConsumableToInventory();
                break;
            case ItemType.Grenade:
                Managers.Game.Player.ReturnGrenadeToInventory();
                break;
        }
    }
    public void SetSlot(ItemData itemData,int count)
    {
        _itemData = itemData;
        _count = count;
        GetImage((int)Images.SlotItemImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(_itemData.Image);
        SetCountText(count);
        _empty = false; 
    }
    public void SetCountText(int count)
    {
        _count = count;
        if (this.ItemData.Consumable)
        {
            GetText((int)Texts.SlotCountText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();
        }
        else
        {
            GetText((int)Texts.SlotCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
        }
    }
    public void ClearSlot()
    {
        _empty = true;
        _count = 0;
        _itemData = null;
        GetImage((int)Images.SlotItemImage).gameObject.GetComponent<Image>().sprite = null;
        GetText((int)Texts.SlotCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
}
