using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : UI_Base
{
    public ItemData itemData;
    public bool _empty;
    public int _count;
    enum Images
    {
        SlotItemImage,
    }
    enum Texts
    {
        SlotCountText,
    }
    
    private void Awake()
    {
        Init();
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
    public void BindEvents()
    {
        //GetImage((int)Images.SlotItemImage).gameObject.BindEvent(SelectSlot);
        this.gameObject.BindEvent(SelectSlot);
    }
    public void SelectSlot()
    {
        if (_empty)
            return;

        this.gameObject.transform.root.GetChild(0).GetComponent<UI_Inventory>().selectedSlot 
            = this.gameObject.GetComponent<UI_InventorySlot>();

        switch (itemData.ItemType)
        {
            case Define.ItemType.Weapon:
                Managers.Game.Player.EquipWeapon(itemData);
                ClearSlot();
                this.gameObject.transform.root.GetChild(0).GetComponent<UI_Inventory>().SortingSlot();
                break;
            case Define.ItemType.Grenade:
                Managers.Game.Player.EquipGrenade(itemData.Name,itemData.Image);
                break;
            case Define.ItemType.Potion:
                Managers.Game.Player.EquipPotion(itemData.Name, itemData.Image);
                break;
        }
    }
    public void SetSlot(ItemData itemData,int count)
    {
        this.itemData = itemData;
        _count = count;
        GetImage((int)Images.SlotItemImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(this.itemData.Image);
        SetCountText(count);
        _empty = false; 
    }
    public void SetCountText(int count)
    {
        _count = count;
        if (this.itemData.Consumable)
                GetText((int)Texts.SlotCountText).gameObject.GetComponent<TextMeshProUGUI>().text = count.ToString();
    }
    public void ClearSlot()
    {
        _empty = true;
        _count = 0;
        itemData = null;
        GetImage((int)Images.SlotItemImage).gameObject.GetComponent<Image>().sprite = null;
        GetText((int)Texts.SlotCountText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }
}
