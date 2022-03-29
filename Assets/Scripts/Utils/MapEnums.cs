using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapEnums
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        NORMAL,
        PATHFINDING,
    }

    /// <summary>
    /// 地图修改枚举
    /// </summary>
    public enum MapModifierEnum
    {
        START,
        END,
        OBSTACLE,
        NORMAL
    }

    /// <summary>
    /// 地图寻路枚举
    /// </summary>
    public enum PathfindingEnum
    {
        ORIGIN,
        TEMP,
        PICK,
        EXCLUDE,
        OPTIONAL
    }

    /// <summary>
    /// 算法类型枚举
    /// </summary>
    [System.Serializable]
    public enum AlgorithmEnum
    {
        BFS = 1,
        DFS = 2,
        DIJKSTRA = 3,
        GREEDY = 4,
        ASTAR = 5,
        BSTAR = 6
    }

    /// <summary>
    /// 寻路算法位置枚举
    /// </summary>
    public enum PathfindingDirectionEnum
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
