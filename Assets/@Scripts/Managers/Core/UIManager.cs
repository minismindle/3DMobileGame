using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UIManager
{
	UI_Base _sceneUI;
	public Stack<UI_Base> _uiStack = new Stack<UI_Base>();

	public T GetSceneUI<T>() where T : UI_Base
	{
		return _sceneUI as T;
	}

	public T ShowSceneUI<T>() where T : UI_Base
	{
		if (_sceneUI != null)
			return GetSceneUI<T>();

		string key = typeof(T).Name + ".prefab";
		T ui = Managers.Resource.Instantiate(key, pooling: true).GetOrAddComponent<T>();
		_sceneUI = ui;

		return ui;
	}

    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"{name}", parent, pooling);
        go.transform.SetParent(parent);
        return Utils.GetOrAddComponent<T>(go);
    }

    public T ShowPopup<T>() where T : UI_Base
	{
		string key = typeof(T).Name + ".prefab";
		T ui = Managers.Resource.Instantiate(key, pooling: true).GetOrAddComponent<T>();
		_uiStack.Push(ui);

		RefreshTimeScale();

		return ui;
	}
	public void GetLastPopup()
	{

	}
	public void ClosePopup()
	{
		if (_uiStack.Count == 0)
			return;

		UI_Base ui = _uiStack.Pop();
		Managers.Resource.Destroy(ui.gameObject);
		Managers.Game.Player.CreatureState = Define.CreatureState.Idle;
		RefreshTimeScale();
	}
	public void CloseAllPopup()
	{
		while( _uiStack.Count > 0 ) 
			ClosePopup();
	}
    public void Clear()
    {
		CloseAllPopup();
        Time.timeScale = 1;
        _sceneUI = null;
    }
    public void RefreshTimeScale()
	{
		if (_uiStack.Count > 0)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
    }
}
