using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using DG.Tweening;

public class CoinController : ItemController
{
    public Define.CoinType CoinType;
    
    Rigidbody _rigidbody;

    private int _gold;
    public override bool Init()
    {
        base.Init();
        SetInfo();
        return true;
    }
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,75f,0) * Time.deltaTime);
    }
    
    void SetInfo()
    {
        ObjectType = Define.ObjectType.Coin;
    }
    public int GetCoin()
    {
        switch(CoinType) 
        {
            case Define.CoinType.Bronze:
                _gold = 50;
                break;
            case Define.CoinType.Silver:
                _gold = 100; 
                break;
            case Define.CoinType.Gold:
                _gold = 200;
                break;
        }
        return _gold;
    }
}
