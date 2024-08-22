using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCController : CreatureController
{
    public override bool Init()
    {
        base.Init();
        _animator = GetComponent<Animator>();   
        return true;
    }
}
