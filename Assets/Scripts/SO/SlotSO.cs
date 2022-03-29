using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图每一格子的颜色数据存储
/// </summary>
[CreateAssetMenu(fileName = "SlotSO", menuName = "SO/SlotSO", order = 1)]
public class SlotSO : ScriptableObject
{
    //正常格子颜色
    public Color normalModifierColor = Color.white;
    //障碍格子颜色
    public Color obstacleModifierColor = Color.black;
    //起始格子颜色
    public Color startModifierColor = Color.red;
    //终点格子颜色
    public Color endModifierColor = new Color(143, 0, 212);

    //原始格子颜色
    public Color originPathfindingColor = Color.white;
    //临时格子颜色
    public Color tempPathfindingColor = Color.yellow;
    //最终格子颜色
    public Color pickPathfindingColor = Color.green;
    //排除格子颜色
    public Color excludePathfindingColor = Color.gray;
    //可选格子颜色
    public Color optionalPathfindingColor = Color.blue;

    //初始权重
    public int weight = 5;

    //地图修改枚举与颜色对应
    public Dictionary<MapEnums.MapModifierEnum, Color> mapModifierEnumMap = new Dictionary<MapEnums.MapModifierEnum, Color>();
    //地图寻路枚举与颜色对应
    public Dictionary<MapEnums.PathfindingEnum, Color> pathfindingEnumMap = new Dictionary<MapEnums.PathfindingEnum, Color>();

    public SlotSO()
    {
        mapModifierEnumMap.Add(MapEnums.MapModifierEnum.NORMAL, normalModifierColor);
        mapModifierEnumMap.Add(MapEnums.MapModifierEnum.OBSTACLE, obstacleModifierColor);
        mapModifierEnumMap.Add(MapEnums.MapModifierEnum.START, startModifierColor);
        mapModifierEnumMap.Add(MapEnums.MapModifierEnum.END, endModifierColor);

        pathfindingEnumMap.Add(MapEnums.PathfindingEnum.ORIGIN, originPathfindingColor);
        pathfindingEnumMap.Add(MapEnums.PathfindingEnum.TEMP, tempPathfindingColor);
        pathfindingEnumMap.Add(MapEnums.PathfindingEnum.PICK, pickPathfindingColor);
        pathfindingEnumMap.Add(MapEnums.PathfindingEnum.EXCLUDE, excludePathfindingColor);
        pathfindingEnumMap.Add(MapEnums.PathfindingEnum.OPTIONAL, optionalPathfindingColor);
    }
}
