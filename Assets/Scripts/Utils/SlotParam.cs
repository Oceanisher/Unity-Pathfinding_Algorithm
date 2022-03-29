using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子参数
/// </summary>
public class SlotParam
{
    /// <summary>
    /// 颜色配置
    /// </summary>
    static SlotSO slotSO = ScriptableObject.CreateInstance<SlotSO>();

    /// <summary>
    /// 当前权重
    /// </summary>
    public int weight;

    /// <summary>
    /// 当前地图修改枚举
    /// </summary>
    public MapEnums.MapModifierEnum modifierEnum;

    /// <summary>
    /// 当前地图寻路枚举
    /// </summary>
    public MapEnums.PathfindingEnum pathfindingEnum;

    /// <summary>
    /// 位置坐标
    /// </summary>
    public Vector2 pos;

    public SlotParam()
    {
        ResetToDefault();
    }

    /// <summary>
    /// 重置地图格子为原始状态
    /// </summary>
    public void ResetToDefault()
    {
        weight = slotSO.weight;
        modifierEnum = MapEnums.MapModifierEnum.NORMAL;
        pathfindingEnum = MapEnums.PathfindingEnum.ORIGIN;
    }

    /// <summary>
    /// 重置格子为其他状态
    /// </summary>
    /// <param name="modifierEnum"></param>
    public void ResetModifier(MapEnums.MapModifierEnum modifierEnum)
    {
        this.modifierEnum = modifierEnum;
        pathfindingEnum = MapEnums.PathfindingEnum.ORIGIN;
    }

    /// <summary>
    /// 重置格子寻路
    /// </summary>
    /// <param name="pathfindingEnum"></param>
    public void ResetPathfinding(MapEnums.PathfindingEnum pathfindingEnum)
    {
        this.pathfindingEnum = pathfindingEnum;
    }

    /// <summary>
    /// 计算展示颜色
    /// </summary>
    /// <returns></returns>
    public Color GenerateShowColor()
    {
        //如果有寻路状态，那么就是展示寻路颜色
        Color color;
        if (pathfindingEnum != MapEnums.PathfindingEnum.ORIGIN)
        {
            slotSO.pathfindingEnumMap.TryGetValue(pathfindingEnum, out color);
        }
        //否则展示修改颜色
        else
        {
            slotSO.mapModifierEnumMap.TryGetValue(modifierEnum, out color);
        }
        return color;
    }
}
