using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 寻路结束命令
/// </summary>
public class PathEndingCommond : ICommand
{
    public void Execute()
    {
        GameManager.Instance.GameStateChangeEvent.Invoke(MapEnums.GameState.NORMAL);
    }
}
