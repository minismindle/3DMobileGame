using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Define
{

    #region Enum
    public enum ItemType
    {
        None,
        Weapon,
        Grenade,
        Consumable,
        Ammo,
    }
    public enum CoinType
    {
        Bronze,
        Silver,
        Gold,
    }
    public enum WeaponName
    {
        None,
        Hammer,
        HandGun,
        SubMachineGun,
    }
    public enum WeaponType
    {
        None,
        Melee,
        Manual,
        Auto,
        Grenade,
    }
    public enum PlayerWeaponType
    {
        None,
        Melee,
        Auto,
        Manual,
        Grenade,
    }
    
    public enum EquipmentType
    {
        Weapon,
        Gloves,
        Ring,
        Belt,
        Armor,
        Boots,
    }

    public enum EquipmentGrade
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
        Myth,
        Myth1,
        Myth2,
        Myth3
    }

    public enum EquipmentSortType
    {
        Level,
        Grade,
    }

    public enum MergeEquipmentType
    {
        None,
        ItemCode,
        Grade,
    }

    public enum CreatureState
    {
        Idle,
        Swing,
        Shot,
        Swap,
        Jump,
        Dodge,
        Throw,
        Skill1,
        Skill2,
        Moving,
        OnDamaged,
        Dead,
        Land,
        Reload,
        Attack,
        Tanut,
        Wait,
    }

    public enum ObjectType
    {
        Player,
        NPC,
        Monster,
        BossMonster,
        Weapon,
        Projectile,
        Coin,
        Grenade,
    }
    public enum NPCType
    {
        Shop,
        Quest,
    }
    public enum MonsterName
    {
        None,
        MonsterA,
        MonsterB,
        MonsterC,
        Boss,
    }
    public enum WaveType
    {
        None,
        RedZone,//자기장 축소
        Elete,// 엘리트몹 등장
        Boss,
        //
    }
    public enum Scene
    {
        None,
        TitleScene,
        GameScene,
        LoadScene,
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }
    #endregion

    public enum Layer
    {
        Wall = 11,

    }
    public enum CameraMode
    {
        QuarterView,
    }
    #region Tag
    public static string MonsterProjectile = "MonsterProjectile";
    public static string PlayerProjectile = "PlayerProjectile";
    #endregion

    #region Ammo
    public static int AmmoItemDataID = 20000;
    #endregion

    #region Projectile
    public static string BulletSubMachineGun = "Bullet_SubMachineGun";
    #endregion

    #region Inventory
    public static int MaxSlotCount = 99999999;
    #endregion
}