using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件集合
/// </summary>
public class MapEvents 
{
    /// <summary>
    /// 生成/重新生成地图事件
    /// </summary>
    [System.Serializable]
    public class GenerateMapEvent : UnityEvent<GenerateMapParam> { }
    
    /// <summary>
    /// 点击格子事件
    /// </summary>
    [System.Serializable]
    public class ClickSlotEvent : UnityEvent<ClickSlotParam> { }

    /// <summary>
    /// 游戏状态变更事件
    /// </summary>
    [System.Serializable]
    public class GameStateChangeEvent : UnityEvent<MapEnums.GameState> { }

    /// <summary>
    /// 生成/重新生成地图事件-参数
    /// </summary>
    [System.Serializable]
    public class GenerateMapParam
    {
        public int width;
        public int height;
    }

    /// <summary>
    /// 点击格子事件
    /// </summary>
    [System.Serializable]
    public class ClickSlotParam
    {
        public SlotController slot;
    }
}
