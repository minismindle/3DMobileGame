using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
    public Dictionary<int, ItemData> ItemDataDic { get; private set; } = new Dictionary<int, ItemData>();
    public Dictionary<int, ShopData> ShopDataDic { get; private set; } = new Dictionary<int, ShopData>();
    public Dictionary<int, MonsterData> MonsterDataDic { get; private set; } = new Dictionary<int, MonsterData>();
    public Dictionary<int, PlayerData> PlayerDataDic { get; private set; } = new Dictionary<int, PlayerData>();
    public Dictionary<int, WaveData> WaveDataDic { get; private set; } = new Dictionary<int, WaveData>();
    public Dictionary<int, ProjectileData> ProjectileDataDic { get; private set; } = new Dictionary<int, ProjectileData>();
    public void Init()
    {
        ItemDataDic = LoadJson<Data.ItemDataLoader,int,Data.ItemData>("ItemData").MakeDict();
        ShopDataDic = LoadJson<Data.ShopDataLoader, int,Data.ShopData>("ShopData").MakeDict();
        MonsterDataDic = LoadJson<Data.MonsterDataLoader, int,Data.MonsterData>("MonsterData").MakeDict();
        PlayerDataDic = LoadJson<Data.PlayerDataLoader, int,Data.PlayerData>("PlayerData").MakeDict();
        WaveDataDic = LoadJson<Data.WaveDataLoader, int,Data.WaveData>("WaveData").MakeDict();
        ProjectileDataDic = LoadJson<Data.ProjectileDataLoader, int,Data.ProjectileData>("ProjectileData").MakeDict();
    }
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }


}
