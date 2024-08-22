using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    Image _background;

	[SerializeField]
	Image _handler;

	float _joystickRadius;
	Vector2 _touchPosition;
	Vector2 _moveDir;
	Vector3 _initPosition;
    void Start()
    {
		_joystickRadius = _background.gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
		_initPosition = _handler.transform.position;
    }
    void Update()
    {
        
    }
	public void OnPointerClick(PointerEventData eventData)
	{

	}
	public void OnPointerDown(PointerEventData eventData)
	{
        _touchPosition = eventData.position;
    }
	public void OnPointerUp(PointerEventData eventData)
    {
        _handler.transform.position = _initPosition;
		_moveDir = Vector2.zero;
		Managers.Game.MoveDir = _moveDir;
	}
	public void OnDrag(PointerEventData eventData)
	{
        Vector2 touchDir = (eventData.position - _touchPosition);
		float moveDist = Mathf.Min(touchDir.magnitude, _joystickRadius);

		_moveDir = touchDir.normalized;

		Vector2 newPosition = _touchPosition + _moveDir * moveDist;

		_handler.transform.position = newPosition;
        Managers.Game.MoveDir = _moveDir;
	}
}
