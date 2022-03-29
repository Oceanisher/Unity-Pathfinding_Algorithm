using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    MapEnums.GameState state;

    MapEvents.GameStateChangeEvent gameStateChangeEvent = new MapEvents.GameStateChangeEvent();
    public MapEvents.GameStateChangeEvent GameStateChangeEvent
    {
        get
        {
            return gameStateChangeEvent;
        }
    }

    public MapEnums.GameState State
    {
        get
        {
            return state;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        state = MapEnums.GameState.NORMAL;
        gameStateChangeEvent.AddListener(OnGameStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 开启寻路
    /// </summary>
    public void StartPathfinding()
    {
        if (!IsReady())
        {
            Debug.LogError("尚未准备好数据。");
        }
        else
        {
            MapController.Instance.ClearMapNav();
            state = MapEnums.GameState.PATHFINDING;
            MapController.Instance.StartPathfinding();
        }
    }

    /// <summary>
    /// 清理寻路
    /// </summary>
    public void ClearPathfinding()
    {
        MapController.Instance.ClearMapNav();
    }

    /// <summary>
    /// 游戏状态变更监听
    /// </summary>
    /// <param name="state"></param>
    void OnGameStateChange(MapEnums.GameState state)
    {
        this.state = state;
    }

    /// <summary>
    /// 是否可以开始寻路
    /// </summary>
    /// <returns></returns>
    bool IsReady()
    {
        return MapController.Instance.IsReady()
            && AlgorithmController.Instance.IsReady()
            && state == MapEnums.GameState.NORMAL;
    }
}
