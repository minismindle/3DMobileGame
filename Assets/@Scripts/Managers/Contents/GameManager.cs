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

	public int Gold { get; set; }

	int _gem = 0;

	public event Action<int> OnGemCountChanged;
	public int Gem 
	{ 
		get { return _gem; }
		set 
		{ 
			_gem = value;
			OnGemCountChanged?.Invoke(value);
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
