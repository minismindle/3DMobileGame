using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{
    public ObjectType ObjectType { get; protected set; }

	void Awake()
    {
        Init();
    }
    private void Start()
    {
        SubScribe();
    }
    bool _init = false;
    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    public virtual void UpdateController()
    {

    }
    public virtual void SetInfoInit(int templateID)
    {

    }
    public virtual void SetInfo(int templateID)
    {

    }
    protected virtual void SubScribe()
    {

    }
}
