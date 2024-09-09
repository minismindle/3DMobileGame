using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Define;

namespace Data
{
    #region ItemData
    [Serializable]
    public class ItemData
    {
        public int DataId;
        public string Name;
        public string Image;
        public ItemType ItemType;
        public WeaponType WeaponType;
        public bool Consumable;
    }
    [Serializable]
    public class  ItemDataLoader : ILoader<int, ItemData>
    {
        public List<ItemData> items = new List<ItemData>();
        public Dictionary<int, ItemData> MakeDict()
        {
            Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();

            foreach(ItemData itemdata in items)
                dict.Add(itemdata.DataId, itemdata);

            return dict;
        }
    }
    #endregion
    #region ShopData
    [Serializable]
    public class ShopData
    {
        public int DataId;
        public int Price;
        public int Count;
    }
    [Serializable]
    public class ShopDataLoader : ILoader<int, ShopData>
    {
        public List<ShopData> shopitems = new List<ShopData>();
        public Dictionary<int, ShopData> MakeDict()
        {
            Dictionary<int, ShopData> dict = new Dictionary<int, ShopData>();

            foreach (ShopData shopdata in shopitems)
                dict.Add(shopdata.DataId, shopdata);

            return dict;
        }
    }
    #endregion
    #region MonsterData
    [Serializable]
    public class MonsterData
    {
        public int DataId;
        public int PrefabName;
        public int MaxHP;
        public float AttackRange;
        public float ScanRange;
        public int Attack;
    }
    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach(MonsterData monsterdata in monsters)
                dict.Add(monsterdata.DataId, monsterdata);
            return dict;
        }
    }
    #endregion

    #region PlayerData
    [Serializable]
    public class PlayerData
    {
        public int DataId;
        public string PrefabName;
        public int MaxHP;
        public int MaxAmmo;
        public int MaxGrenade;
        public int MaxPotion;
        public float Speed;
    }
    [Serializable]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        public List<PlayerData> player = new List<PlayerData>();
        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach (PlayerData playerdata in player)
                dict.Add(playerdata.DataId, playerdata);
            return dict;
        }
    }
    #endregion

    #region ProjectileData
    [Serializable]
    public class ProjectileData
    {
        public int DataId;
        public string PrefabName;
        public float Speed;
        public int Damage;
    }
    [Serializable]
    public class ProjectileDataLoader : ILoader<int, ProjectileData>
    {
        public List<ProjectileData> projectiles = new List<ProjectileData>();
        public Dictionary<int, ProjectileData> MakeDict()
        {
            Dictionary<int, ProjectileData> dict = new Dictionary<int, ProjectileData>();
            foreach (ProjectileData projectiledata in projectiles)
                dict.Add(projectiledata.DataId, projectiledata);
            return dict;
        }
    }
    #endregion

    #region StageData
    [Serializable]
    public class StageData
    {
        public int DataId;
        public int BossSpawnCount;
        public int MaxCount;
        public float SpawnInterval;
        public List<string> MonsterNames;
        public string BossName;
    }
    [Serializable]

    public class StageDataLoader : ILoader<int,StageData>
    {
        public List<StageData> stages = new List<StageData>();
        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int,StageData> dict = new Dictionary<int,StageData>();
            foreach (StageData stagedata in stages)
                dict.Add(stagedata.DataId, stagedata);
            return dict;    
        }
    }
    #endregion
}