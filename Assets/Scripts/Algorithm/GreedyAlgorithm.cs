using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪心算法导航
/// </summary>
public class GreedyAlgorithm : IAlgorithm
{
    //当前的路线链表
    List<SlotController> slotLink = new List<SlotController>();

    public override void Pathfinding(SlotController[][] slots, SlotController startSlot, SlotController endSlot)
    {
        this.slots = slots;
        this.startSlot = startSlot;
        this.endSlot = endSlot;
        this.isFinish = false;
        InitFlag();
        Debug.Log("开始进行贪心寻路");
        NavigationStepByStepInit(startSlot);
        //结束寻路
        SendEndCommand();
    }

    /// <summary>
    /// 递归循环开始结束处理
    /// </summary>
    /// <param name="slot"></param>
    void NavigationStepByStepInit(SlotController slot)
    {
        //开始循环
        NavigationStepByStep(slot);

        //结束后确认最终路径
        foreach (SlotController temp in slotLink)
        {
            SendNavigationCommand(temp, MapEnums.PathfindingEnum.PICK);
        }
    }

    /// <summary>
    /// 递归循环查询
    /// </summary>
    /// <param name="nowSlot"></param>
    void NavigationStepByStep(SlotController nowSlot)
    {
        //如果当前点是已经被处理过的点、被回溯的点，那么不写flag
        if (!flagArray[(int)nowSlot.Pos.x][(int)nowSlot.Pos.y])
        {
            //将当前的点置为已处理、加入到路线链表中
            flagArray[(int)nowSlot.Pos.x][(int)nowSlot.Pos.y] = true;
            slotLink.Add(nowSlot);
            SendNavigationCommand(nowSlot, MapEnums.PathfindingEnum.TEMP);
        }
        
        if (nowSlot == endSlot)
        {
            isFinish = true;
            return;
        }

        //寻找该点周围的点，找出距离终点最短的点
        SlotController[] neighbors = FindNeighbor(slots, nowSlot);
        float distance = -1;
        SlotController nextSlot = null;
        foreach (SlotController slot in neighbors)
        {
            if (null == slot)
            {
                continue;
            }
            if (!slot.CanPathfinding() || IsFlagTrue(slot))
            {
                continue;
            }
            SendNavigationCommand(slot, MapEnums.PathfindingEnum.OPTIONAL);
            float tempDistance = CalculateDisToEnd(slot, endSlot);
            if (distance < 0 || tempDistance < distance)
            {
                distance = tempDistance;
                nextSlot = slot;
            }
        }

        if (null != nextSlot)
        {
            //如果当前选中的点与链表中的某个点相邻，那么重新连接这两个点，删除链表中那个点后面的点
            SlotController[] nextNeighbers = FindNeighbor(slots, nextSlot);
            List<SlotController> existNeightbers = new List<SlotController>();
            foreach (SlotController slot in nextNeighbers)
            {
                if (slotLink.Contains(slot) && slot != nowSlot)
                {
                    existNeightbers.Add(slot);
                }
            }

            if (existNeightbers.Count != 0)
            {
                int index = -1;
                int existNeightberIndex = -1;
                //int[] positionArray = new int[existNeightbers.Count];
                for (int i = 0; i < existNeightbers.Count; i++)
                {
                    if (index < 0 || slotLink.IndexOf(existNeightbers.ToArray()[i]) < index)
                    {
                        index = slotLink.IndexOf(existNeightbers.ToArray()[i]);
                        existNeightberIndex = i;
                    }
                }

                for (int i = slotLink.IndexOf(existNeightbers.ToArray()[existNeightberIndex]) + 1; i < slotLink.Count; i++)
                {
                    slotLink = slotLink.GetRange(0, i);
                }
            }

            NavigationStepByStepInit(nextSlot);
        }
        //如果周围已经没有需要处理的点了，但是还没有结束，试图回溯处理
        else
        {
            //如果当前点已经是起点、无法回溯
            if (nowSlot == startSlot)
            {
                return;
            }
            //如果是非起始点，那么向上回溯、并重写队列
            slotLink.Remove(nowSlot);
            //SendNavigationCommand(nowSlot, MapEnums.PathfindingEnum.OPTIONAL);
            NavigationStepByStepInit(slotLink.ToArray()[slotLink.Count - 1]);
        }
    }

    /// <summary>
    /// 计算某个点到终点的距离
    /// 这里按照上下左右计算，
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    float CalculateDisToEnd(SlotController nowSlot, SlotController endSlot)
    {
        float xPos = endSlot.Pos.x - nowSlot.Pos.x;
        float yPos = endSlot.Pos.y - nowSlot.Pos.y;

        return xPos * xPos + yPos * yPos;
    }
}
