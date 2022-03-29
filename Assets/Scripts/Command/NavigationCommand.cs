using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图导航指令
/// </summary>
public class NavigationCommand : ICommand
{
    //被操作的slot
    public SlotController slot;
    //操作slot的类型
    public MapEnums.PathfindingEnum pathfinding;

    public void Execute()
    {
        slot.SetToPathfindingShow(pathfinding);
    }
}
