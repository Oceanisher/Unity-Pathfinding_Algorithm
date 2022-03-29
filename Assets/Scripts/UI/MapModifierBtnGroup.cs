using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapModifierBtnGroup : Singleton<MapModifierBtnGroup>
{
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject obstacleBtn;
    [SerializeField] GameObject endBtn;
    [SerializeField] GameObject resetBtn;
    [SerializeField] GameObject weightInput;
    List<GameObject> modifierBtnGroup;

    public MapEnums.MapModifierEnum mapModifierEnum;

    // Start is called before the first frame update
    void Start()
    {
        mapModifierEnum = MapEnums.MapModifierEnum.NORMAL;
        modifierBtnGroup = new List<GameObject>();
        modifierBtnGroup.Add(startBtn);
        modifierBtnGroup.Add(obstacleBtn);
        modifierBtnGroup.Add(endBtn);
        modifierBtnGroup.Add(resetBtn);

        startBtn.GetComponent<Button>().onClick.AddListener(OnBtnStart);
        obstacleBtn.GetComponent<Button>().onClick.AddListener(OnBtnObstacle);
        endBtn.GetComponent<Button>().onClick.AddListener(OnBtnEnd);
        resetBtn.GetComponent<Button>().onClick.AddListener(OnBtnReset);
        weightInput.GetComponent<InputField>().onEndEdit.AddListener(OnSlotWeightInputChange);

        MapController.Instance.ClickSlotEvent.AddListener(OnSlotClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 鼠标点击-起始
    /// </summary>
    public void OnBtnStart()
    {
        ResetOtherBtn(startBtn);
        mapModifierEnum = MapEnums.MapModifierEnum.START;
    }

    /// <summary>
    /// 鼠标点击-障碍
    /// </summary>
    public void OnBtnObstacle()
    {
        ResetOtherBtn(obstacleBtn);
        mapModifierEnum = MapEnums.MapModifierEnum.OBSTACLE;
    }

    /// <summary>
    /// 鼠标点击-终点
    /// </summary>
    public void OnBtnEnd()
    {
        ResetOtherBtn(endBtn);
        mapModifierEnum = MapEnums.MapModifierEnum.END;
    }

    /// <summary>
    /// 鼠标点击-重置
    /// </summary>
    public void OnBtnReset()
    {
        ResetOtherBtn(resetBtn);
        mapModifierEnum = MapEnums.MapModifierEnum.NORMAL;
    }

    /// <summary>
    /// 格子点击事件
    /// </summary>
    /// <param name="param"></param>
    void OnSlotClick(MapEvents.ClickSlotParam param)
    {
        Debug.Log("格子点击事件参数：" + param);
        int weight = param.slot.Weight;
        weightInput.GetComponent<InputField>().text = weight.ToString();
    }

    /// <summary>
    /// 格子权重改变监听
    /// </summary>
    /// <param name="value"></param>
    void OnSlotWeightInputChange(string value)
    {
        if (null != MapController.Instance.currentSlot)
        {
            MapController.Instance.currentSlot.Weight = int.Parse(value);
        }
    }

    /// <summary>
    /// 重置按钮是否可交互
    /// </summary>
    /// <param name="clickBtn"></param>
    void ResetOtherBtn(GameObject clickBtn)
    {
        foreach (GameObject obj in modifierBtnGroup)
        {
            if (obj == clickBtn)
            {
                if (resetBtn != clickBtn)
                {
                    obj.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }
}
