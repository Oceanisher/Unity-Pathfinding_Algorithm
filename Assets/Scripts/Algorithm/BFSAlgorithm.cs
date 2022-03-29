using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 宽度优先搜索算法
/// </summary>
public class BFSAlgorithm : IAlgorithm
{
    //每个点都保存一个指向前一个点的指针
    Dictionary<SlotController, SlotController> pointDic = new Dictionary<SlotController, SlotController>();

    public override void Pathfinding(SlotController[][] slots, SlotController startSlot, SlotController endSlot)
    {
        this.slots = slots;
        this.startSlot = startSlot;
        this.endSlot = endSlot;
        this.isFinish = false;
        InitFlag();
        Debug.Log("开始进行BFS寻路");
        NavigationStepByStepInit(new SlotController[] { startSlot });
        //结束寻路
        SendEndCommand();
    }

    /// <summary>
    /// 递归循环查询准备
    /// </summary>
    /// <param name="nowSlotArray"></param>
    void NavigationStepByStepInit(SlotController[] nowSlotArray)
    {
        NavigationStepByStep(nowSlotArray);

        //结束的路线放到集合
        List<SlotController> list = new List<SlotController>();
        SlotController searchSlot = endSlot;
        while(pointDic.ContainsKey(searchSlot))
        {
            SlotController temp;
            pointDic.TryGetValue(searchSlot, out temp);
            list.Add(temp);
            searchSlot = temp;
        }

        list.Reverse();
        foreach (SlotController slot in list)
        {
            SendNavigationCommand(slot, MapEnums.PathfindingEnum.PICK);
        }
    }

    /// <summary>
    /// 递归循环查询
    /// </summary>
    /// <param name="nowSlot"></param>
    void NavigationStepByStep(SlotController[] nowSlotArray)
    {
        //如果其他操作已经找到了终点，结束
        if (isFinish)
        {
            return;
        }
        //循环当前的数组，看是否有终点
        //如果当前格子的周围没发现，那么继续循环子节点周围的点
        List<SlotController> childrenList = new List<SlotController>();
        foreach (SlotController slot in nowSlotArray)
        {
            if (slot == endSlot)
            {
                isFinish = true;
                return;
            }
            if (null != slot && slot.CanPathfinding() && !IsFlagTrue(slot) && !isFinish)
            {
                FillFlag(slot);
                SendNavigationCommand(slot, MapEnums.PathfindingEnum.TEMP);

                SlotController[] neighbers = FindNeighbor(slots, slot);
                foreach (SlotController temp in neighbers)
                {
                    if (null != temp && temp.CanPathfinding() && !IsFlagTrue(temp) && !childrenList.Contains(temp))
                    {
                        childrenList.Add(temp);
                        SendNavigationCommand(temp, MapEnums.PathfindingEnum.OPTIONAL);
                        pointDic.Add(temp, slot);
                    }
                }
            }
        }
        
        //递归子节点
        if (childrenList.Count > 0)
        {
            NavigationStepByStep(childrenList.ToArray());
        }
    }
}
