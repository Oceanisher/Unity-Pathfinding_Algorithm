using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 寻路算法
/// </summary>
public abstract class IAlgorithm
{
    //地图
    protected SlotController[][] slots;
    //起点
    protected SlotController startSlot;
    //终点
    protected SlotController endSlot;
    //本次寻路是否已经结束
    protected bool isFinish;
    //格子已处理标识
    protected bool[][] flagArray;

    protected NavigationInvoker invoker = NavigationInvoker.Instance;

    /// <summary>
    /// 进行寻路
    /// </summary>
    /// <param name="slots"></param>
    /// <param name="startSlot"></param>
    /// <param name="endSlot"></param>
    public abstract void Pathfinding(SlotController[][] slots, SlotController startSlot, SlotController endSlot);

    /// <summary>
    /// 重置算法
    /// </summary>
    protected void Reset()
    {
        slots = null;
        startSlot = null;
        endSlot = null;
        isFinish = false;
    }

    /// <summary>
    /// 寻找邻接点，按照上、左、下、右的顺序
    /// </summary>
    /// <param name="slots"></param>
    /// <param name="searchSlot"></param>
    /// <returns></returns>
    protected SlotController[] FindNeighbor(SlotController[][] slots, SlotController searchSlot)
    {
        SlotController up;
        IsExist(slots, searchSlot, MapEnums.PathfindingDirectionEnum.UP, out up);
        SlotController left;
        IsExist(slots, searchSlot, MapEnums.PathfindingDirectionEnum.LEFT, out left);
        SlotController down;
        IsExist(slots, searchSlot, MapEnums.PathfindingDirectionEnum.DOWN, out down);
        SlotController right;
        IsExist(slots, searchSlot, MapEnums.PathfindingDirectionEnum.RIGHT, out right);
        return new SlotController[] { up, left, down, right };
    }

    /// <summary>
    /// 是否存在某个方向的格子，如果存在，返回
    /// </summary>
    /// <param name="slots"></param>
    /// <param name="searchSlot"></param>
    /// <param name="direction"></param>
    /// <param name="findSlot"></param>
    /// <returns></returns>
    protected bool IsExist(SlotController[][] slots, SlotController searchSlot, MapEnums.PathfindingDirectionEnum direction, out SlotController findSlot)
    {
        int x = (int)searchSlot.Pos.x, y = (int)searchSlot.Pos.y;
        
        switch (direction)
        {
            case MapEnums.PathfindingDirectionEnum.UP:
                x -= 1;
                break;
            case MapEnums.PathfindingDirectionEnum.DOWN:
                x += 1;
                break;
            case MapEnums.PathfindingDirectionEnum.LEFT:
                y -= 1;
                break;
            case MapEnums.PathfindingDirectionEnum.RIGHT:
                y += 1;
                break;
            default:
                Debug.LogError("上下左右不存在");
                break;
        }
        
        //判断对应的slot是否存在
        if (x < 0 || x >= slots.Length 
            || y < 0 || y >= slots[0].Length)
        {
            findSlot = null;
            return false;
        }
        //存在则返回
        else
        {
            findSlot = slots[x][y];
            return true;
        }
    }

    /// <summary>
    /// 返回寻路顺序
    /// </summary>
    /// <param name="directionEnum"></param>
    /// <returns></returns>
    protected MapEnums.PathfindingDirectionEnum Next(MapEnums.PathfindingDirectionEnum direction)
    {
        switch (direction)
        {
            case MapEnums.PathfindingDirectionEnum.UP:
                return MapEnums.PathfindingDirectionEnum.LEFT;
            case MapEnums.PathfindingDirectionEnum.DOWN:
                return MapEnums.PathfindingDirectionEnum.RIGHT;
            case MapEnums.PathfindingDirectionEnum.LEFT:
                return MapEnums.PathfindingDirectionEnum.DOWN;
            case MapEnums.PathfindingDirectionEnum.RIGHT:
                return MapEnums.PathfindingDirectionEnum.UP;
            default:
                Debug.LogError("上下左右不存在");
                return 0;
        }
    }

    /// <summary>
    /// 发送格子变更命令
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="pathfindingEnum"></param>
    protected void SendNavigationCommand(SlotController slot, MapEnums.PathfindingEnum pathfindingEnum)
    {
        if (slot == startSlot || slot == endSlot)
        {
            return;
        }
        else
        {
            NavigationCommand command = new NavigationCommand();
            command.slot = slot;
            command.pathfinding = pathfindingEnum;
            invoker.AddCommond(command);
        }
    }

    /// <summary>
    /// 发送寻路结束命令
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="pathfindingEnum"></param>
    protected void SendEndCommand()
    {
        PathEndingCommond command = new PathEndingCommond();
        NavigationInvoker.Instance.AddCommond(command);
    }

    /// <summary>
    /// 初始化标识位
    /// </summary>
    /// <param name="slots"></param>
    protected void InitFlag()
    {
        this.flagArray = new bool[slots.Length][];
        for (int i = 0; i < slots.Length; i++)
        {
            this.flagArray[i] = new bool[slots[i].Length];
        }
    }

    /// <summary>
    /// 填充标识
    /// </summary>
    protected void FillFlag(SlotController slot)
    {
        flagArray[(int)slot.Pos.x][(int)slot.Pos.y] = true;
    }

    /// <summary>
    /// 查看标识是否已经是处理过的格子
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    protected bool IsFlagTrue(SlotController slot)
    {
        return flagArray[(int)slot.Pos.x][(int)slot.Pos.y];
    }
}
