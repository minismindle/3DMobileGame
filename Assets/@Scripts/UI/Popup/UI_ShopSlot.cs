using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopSlot : UI_Base
{
    ItemData _itemdata;

    int _count;
    int _price;
    bool _isGet;
    public virtual ItemData ItemData {  get { return _itemdata; } set { _itemdata = value; } }
    public virtual int Count { get { return _count; } set { _count = value; } } 
    public virtual int Price { get { return _price; } set { _price = value; } }
    enum Images
    {
        ItemImage,
    }
    enum Buttons
    {
    }
    enum Texts
    {
        PriceText,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindEvents();
        return true;
    }
    protected override void BindEvents()
    {
        GetImage((int)Images.ItemImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(_itemdata.Image);
        GetText((int)Texts.PriceText).gameObject.GetComponent<TextMeshProUGUI>().text = _price.ToString();
        this.gameObject.BindEvent(BuyItem);
    }
    public void BuyItem()
    {
        if (_isGet)
            return;
        if (Managers.Game.Gold < _price)
            return;
        Managers.Game.Player.Inventory.InsertItem(_itemdata, _count);
        Managers.Game.Gold -= _price;
        if(!_itemdata.Consumable)
            _isGet = true;
    }
}
