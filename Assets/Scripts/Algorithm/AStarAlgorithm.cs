using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*寻路算法
/// </summary>
public class AStarAlgorithm : IAlgorithm
{
    //各个点到起点的最短路径距离
    int[][] minDistanceArray;
    int[][] blendDistanceArray;
    //各个点到起点的最短路径链表
    Dictionary<SlotController, List<SlotController>> slotCollectionDic;

    public override void Pathfinding(SlotController[][] slots, SlotController startSlot, SlotController endSlot)
    {
        this.slots = slots;
        this.startSlot = startSlot;
        this.endSlot = endSlot;
        this.isFinish = false;
        InitFlag();
        Init();
        Debug.Log("开始进行A*寻路");
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
        List<SlotController> list;
        if (slotCollectionDic.TryGetValue(endSlot, out list))
        {
            foreach (SlotController temp in list)
            {
                SendNavigationCommand(temp, MapEnums.PathfindingEnum.PICK);
            }
        }
    }

    /// <summary>
    /// 递归循环查询
    /// </summary>
    /// <param name="nowSlot"></param>
    void NavigationStepByStep(SlotController nowSlot)
    {
        //查找周围格子、计算距离，
        SlotController[] neighbors = FindNeighbor(slots, nowSlot);
        //展示相关：当前格子置为选中
        SendNavigationCommand(nowSlot, MapEnums.PathfindingEnum.TEMP);
        foreach (SlotController slot in neighbors)
        {
            if (null == slot)
            {
                continue;
            }
            if (!slot.CanPathfinding() || IsFlagTrue(slot) || minDistanceArray[(int)slot.Pos.x][(int)slot.Pos.y] > -1)
            {
                continue;
            }

            //计算从当前点到邻居点的距离
            int distance = CalculateStartDistance(nowSlot, slot);
            //如果这个邻接点现在的距离，小于它之前计算的距离，那么就将它的距离、点链表替换
            int beforeDistance = minDistanceArray[(int)slot.Pos.x][(int)slot.Pos.y];
            if (beforeDistance < 0 || distance < beforeDistance)
            {
                minDistanceArray[(int)slot.Pos.x][(int)slot.Pos.y] = distance;
            }
                
            //计算当前点到起点、终点的距离和
            int blendDistance = distance + CalculateEndDistance(slot);
            int beforeBlendDistance = blendDistanceArray[(int)slot.Pos.x][(int)slot.Pos.y];
            if (beforeBlendDistance < 0 || blendDistance < beforeBlendDistance)
            {
                blendDistanceArray[(int)slot.Pos.x][(int)slot.Pos.y] = blendDistance;
                List<SlotController> tempList;
                slotCollectionDic.TryGetValue(nowSlot, out tempList);
                List<SlotController> list = new List<SlotController>(tempList);
                list.Add(slot);
                slotCollectionDic.Add(slot, list);
                //展示相关：当前格子置为潜在
                SendNavigationCommand(slot, MapEnums.PathfindingEnum.OPTIONAL);
            }

        }
        //完成之后，锁住当前的点
        flagArray[(int)nowSlot.Pos.x][(int)nowSlot.Pos.y] = true;
        //展示相关：当前格子置为锁住
        SendNavigationCommand(nowSlot, MapEnums.PathfindingEnum.EXCLUDE);
        //如果当前的点是终点，那么直接结束
        if (nowSlot == endSlot)
        {
            return;
        }
        //寻找到当前未确认的、距离最小的点，进行下次循环
        SlotController nextSlot = FindMin();
        if (null != nextSlot)
        {
            NavigationStepByStep(nextSlot);
        }
    }

    /// <summary>
    /// 查找未确认的、最小距离的格子
    /// </summary>
    /// <returns></returns>
    SlotController FindMin()
    {
        int x = 0;
        int y = 0;
        int minDistance = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[i].Length; j++)
            {
                if (blendDistanceArray[i][j] < 0)
                {
                    continue;
                }
                if ((minDistance < 0 || blendDistanceArray[i][j] < minDistance) && !flagArray[i][j])
                {
                    minDistance = blendDistanceArray[i][j];
                    x = i;
                    y = j;
                    continue;
                }
            }
        }
        return minDistance < 0 ? null : slots[x][y];
    }

    /// <summary>
    /// 初始化距离数组、链表字典
    /// </summary>
    void Init()
    {
        //起点的距离标识为0，其他标识为-1、代表无穷大
        minDistanceArray = new int[slots.Length][];
        blendDistanceArray = new int[slots.Length][];
        for (int i = 0; i < minDistanceArray.Length; i++)
        {
            minDistanceArray[i] = new int[slots[i].Length];
            blendDistanceArray[i] = new int[slots[i].Length];
            for (int j = 0; j < slots[i].Length; j++)
            {
                minDistanceArray[i][j] = -1;
                blendDistanceArray[i][j] = -1;
            }
        }
        minDistanceArray[(int)startSlot.Pos.x][(int)startSlot.Pos.y] = 0;
        blendDistanceArray[(int)startSlot.Pos.x][(int)startSlot.Pos.y] = 0;

        //链表字典初始化
        slotCollectionDic = new Dictionary<SlotController, List<SlotController>>();
        List<SlotController> list = new List<SlotController>();
        list.Add(startSlot);
        slotCollectionDic.Add(startSlot, list);

        //起点标识位标识为true
        flagArray[(int)startSlot.Pos.x][(int)startSlot.Pos.y] = true;
    }

    /// <summary>
    /// 计算某个点到起点的距离
    /// </summary>
    /// <param name="parentSlot"></param>
    /// <param name="nowSlot"></param>
    /// <returns></returns>
    int CalculateStartDistance(SlotController parentSlot, SlotController childSlot)
    {
        return minDistanceArray[(int)parentSlot.Pos.x][(int)parentSlot.Pos.y] + childSlot.Weight;
    }

    /// <summary>
    /// 计算某个点到终点的曼哈顿距离（预测距离）
    /// 也是A*算法中的启发函数
    /// </summary>
    /// <param name="nowSlot"></param>
    /// <returns></returns>
    int CalculateEndDistance(SlotController nowSlot)
    {
        int dx = (int)Mathf.Abs(nowSlot.Pos.x - endSlot.Pos.x);
        int dy = (int)Mathf.Abs(nowSlot.Pos.y - endSlot.Pos.y);
        return 6 * (dx + dy);
    }
}
