using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : BaseController
{
    public virtual Define.ItemType ItemType { get; set; } = Define.ItemType.None;


}
