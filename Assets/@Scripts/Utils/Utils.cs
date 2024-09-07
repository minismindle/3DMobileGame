using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Utils
{
	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
			component = go.AddComponent<T>();
		return component;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform == null)
			return null;

		return transform.gameObject;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
						return component;
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}
		}

		return null;
	}
	public static Vector2 RandomPointInAnnulus(Vector2 origin,float minRadius = 6,float maxRadius = 12)
	{
		float randomDist= UnityEngine.Random.Range(minRadius,maxRadius);

		Vector2 randomDir = new Vector2(UnityEngine.Random.Range(-100,100),UnityEngine.Random.Range(-100,100)).normalized;

		var point = origin + randomDist * randomDir;

		return point;
	}
	public static Color HexToColor(string color)
	{
		Color parsedColor;
		ColorUtility.TryParseHtmlString("#" + color, out parsedColor);

		return parsedColor;
	}
	public static Vector2 GenerateMonsterSpawnPosition(Vector3 characterPosition, float minDistance = 10f, float maxDistance = 20f)
	{
		float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
		float distance = Random.Range(minDistance, maxDistance);

		float xDist = Mathf.Cos(angle) * distance;
		float zDist = Mathf.Sin(angle) * distance;

		Vector3 spawnPosition = characterPosition + new Vector3(xDist, 0,zDist);

		return spawnPosition;
	}
}
