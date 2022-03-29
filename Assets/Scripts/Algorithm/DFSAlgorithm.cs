using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 深度优先算法
/// </summary>
public class DFSAlgorithm : IAlgorithm
{
    public override void Pathfinding(SlotController[][] slots, SlotController startSlot, SlotController endSlot)
    {
        this.slots = slots;
        this.startSlot = startSlot;
        this.endSlot = endSlot;
        this.isFinish = false;
        InitFlag();
        Debug.Log("开始进行DFS寻路");
        NavigationStepByStep(startSlot);
        //结束寻路
        SendEndCommand();
    }

    /// <summary>
    /// 递归循环查询
    /// </summary>
    /// <param name="nowSlot"></param>
    void NavigationStepByStep(SlotController nowSlot)
    {
        //如果其他操作已经找到了终点，结束
        if (isFinish)
        {
            return;
        }
        //如果当前格子已经是终点格子，结束
        if (nowSlot == endSlot)
        {
            isFinish = true;
            return;
        }

        //循环四个方向查询查询是否到达终点，按照深度查询
        SlotController[] neighbers = FindNeighbor(slots, nowSlot);
        foreach (SlotController slot in neighbers)
        {
            if (slot == endSlot)
            {
                isFinish = true;
                return;
            }
            if (null != slot && slot.CanPathfinding() && !IsFlagTrue(slot) && !isFinish)
            {
                FillFlag(slot);
                SendNavigationCommand(slot, MapEnums.PathfindingEnum.PICK);
                NavigationStepByStep(slot);
            }
        }
    }
}
