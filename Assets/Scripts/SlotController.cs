using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 地图格子控制
/// </summary>
public class SlotController : MonoBehaviour
{
    //格子配置
    SlotParam param;
    public Vector2 Pos
    {
        set
        {
            param.pos = value;
        }
        get
        {
            return param.pos;
        }
    }

    public int Weight
    {
        set
        {
            param.weight = value;
            SetSlotShow(param.GenerateShowColor(), param.weight);
        }
        get
        {
            return param.weight;
        }
    }

    void Awake()
    {
        param = new SlotParam();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Text>().text = param.weight.ToString();
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickSlot);
    }

    /// <summary>
    /// 将格子设置为已经被寻找颜色
    /// </summary>
    public void SetToSearched()
    {
        param.ResetPathfinding(MapEnums.PathfindingEnum.PICK);
        SetSlotShow(param.GenerateShowColor(), param.weight);
    }

    /// <summary>
    /// 将格子设置为寻路中的各种状态、并修改展示
    /// </summary>
    /// <param name="pathfindingEnum"></param>
    public void SetToPathfindingShow(MapEnums.PathfindingEnum pathfindingEnum)
    {
        param.ResetPathfinding(pathfindingEnum);
        SetSlotShow(param.GenerateShowColor(), param.weight);
    }

    /// <summary>
    /// 判断该格子是否可以进行寻路
    /// </summary>
    public bool CanPathfinding()
    {
        if (param.modifierEnum == MapEnums.MapModifierEnum.OBSTACLE
            || param.pathfindingEnum == MapEnums.PathfindingEnum.PICK
            || param.pathfindingEnum == MapEnums.PathfindingEnum.EXCLUDE)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 将格子重置为初始状态
    /// </summary>
    public void ResetSlotToDefault()
    {
        param.ResetToDefault();
        SetSlotShow(param.GenerateShowColor(), param.weight);
    }

    /// <summary>
    /// 重置地图格子
    /// </summary>
    /// <param name="mapEnum"></param>
    /// <param name="weight"></param>
    void OnClickSlot()
    {
        MapEnums.MapModifierEnum mapEnum = MapModifierBtnGroup.Instance.mapModifierEnum;

        //如果是起点、终点，那么还要看是否已经有了，如果有了，那么清除原来的、换上新的。
        if (mapEnum == MapEnums.MapModifierEnum.START)
        {
            if (null != MapController.Instance.startSlot)
            {
                MapController.Instance.startSlot.ResetSlotToDefault();
            }
            MapController.Instance.startSlot = this;
        }
        if (mapEnum == MapEnums.MapModifierEnum.END)
        {
            if (null != MapController.Instance.endSlot)
            {
                MapController.Instance.endSlot.ResetSlotToDefault();
            }
            MapController.Instance.endSlot = this;
        }

        param.ResetModifier(mapEnum);
        SetSlotShow(param.GenerateShowColor(), param.weight);
        MapController.Instance.ClickSlot(this);
    }

    /// <summary>
    /// 设置格子展示
    /// </summary>
    /// <param name="color"></param>
    /// <param name="weight"></param>
    void SetSlotShow(Color color, int weight)
    {
        //设置格子颜色
        gameObject.GetComponentInChildren<Text>().text = weight.ToString();
        ColorBlock colorBlock = gameObject.GetComponent<Button>().colors;
        colorBlock.normalColor = color;
        colorBlock.highlightedColor = color;
        gameObject.GetComponent<Button>().colors = colorBlock;
        //让格子脱离focus状态，从而展示normal状态
        //gameObject.GetComponent<Button>().
        //GUI.FocusControl(gameObject.name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 重置格子寻路
    /// </summary>
    public void ClearPathfinding()
    {
        param.ResetPathfinding(MapEnums.PathfindingEnum.ORIGIN);
        SetSlotShow(param.GenerateShowColor(), param.weight);
    }
}
