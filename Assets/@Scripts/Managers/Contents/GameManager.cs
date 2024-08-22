using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.Playables;

public class GameManager
{
	public PlayerController Player { get { return Managers.Object?.Player; } }
	#region 재화

	int _gold = 0;

	public event Action<int> OnGoldCountChanged;
	public int Gold
    { 
		get { return _gold; }
		set 
		{
            _gold = value;
			OnGoldCountChanged?.Invoke(value);
		}
	}

	int _manualAmmo = 0;

    public event Action<int> OnManualAmmoCountChanged;
    public int ManualAmmo
	{
		get { return _manualAmmo; }
		set
		{
            _manualAmmo = value;
			OnManualAmmoCountChanged?.Invoke(value);	
		}
	}

	int _autoAmmo = 0;
	public event  Action<int> OnAutoAmmoCountChanged;	
	public int AutoAmmo
	{
		get { return _autoAmmo; }
		set
		{
			_autoAmmo = value;
			OnAutoAmmoCountChanged?.Invoke(value);	
		}
	}

	int _totalAmmo = 0;

	public event Action<int> OnTotalAmmoCountChanged;

	public int TotalAmmo
	{
		get { return _totalAmmo; }
		set
		{
            _totalAmmo = value;
            OnTotalAmmoCountChanged?.Invoke(value);
		}
	}

	int _potion = 0;

	public event Action<int> OnPotionCountChanged;
	public int Potion
	{
		get { return _potion; }
		set
		{
			_potion = value;
			OnPotionCountChanged?.Invoke(value);
		}
	}

	int _grenade = 0;

	public event Action<int> OnGrenadeCountChanged;
	public int Grenade
	{
		get { return _grenade; }
		set
		{
			_grenade = value;
			OnGrenadeCountChanged?.Invoke(value);	
		}
	}
    #endregion

    #region 전투
    int _killCount;
	public event Action<int> OnKillCountChanged;

	public int KillCount
	{
		get { return _killCount; }
		set
		{
			_killCount = value; OnKillCountChanged?.Invoke(value);
		}
	}
	#endregion

	#region 이동
	Vector3 _moveDir;

	public event Action<Vector3> OnMoveDirChanged;

	public Vector3 MoveDir
	{
		get { return _moveDir; }
		set
		{
			_moveDir = value;
            _moveDir.z = _moveDir.y;
            _moveDir.y = 0;
            OnMoveDirChanged?.Invoke(_moveDir);
		}
	}
    #endregion
}
