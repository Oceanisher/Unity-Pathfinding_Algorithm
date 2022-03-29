using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapModifierGenerateGroup : Singleton<MapModifierGenerateGroup>
{
    [SerializeField] GameObject widthInput;
    [SerializeField] GameObject heightInput;
    [SerializeField] GameObject generateBtn;
    [SerializeField] int defaultWidth = 29;
    [SerializeField] int defaultHeight = 19;
    [SerializeField] int maxWidth = 29;
    [SerializeField] int maxHeight = 19;

    int width;
    int height;

    /// <summary>
    /// 重新生成地图事件
    /// </summary>
    MapEvents.GenerateMapEvent generateMapEvent = new MapEvents.GenerateMapEvent();

    public MapEvents.GenerateMapEvent GenerateMapEvent
    {
        get
        {
            return generateMapEvent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        widthInput.GetComponent<InputField>().onValueChanged.AddListener(SetWidth);
        heightInput.GetComponent<InputField>().onValueChanged.AddListener(SetHeight);
        generateBtn.GetComponent<Button>().onClick.AddListener(OnGenerateMap);

        //初始化宽、高输入框显示数值
        width = defaultWidth;
        height = defaultHeight;
        SetHeightAndWidthText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 写入宽度
    /// </summary>
    /// <param name="value"></param>
    void SetWidth(string value)
    {
        width = int.Parse(value);
        if (width < 0 || width > maxWidth)
        {
            if (width < 0)
            {
                width = 1;
            }
            else
            {
                width = maxWidth;
            }
            widthInput.GetComponent<InputField>().text = width.ToString();
        }
    }

    /// <summary>
    /// 写入高度
    /// </summary>
    /// <param name="value"></param>
    void SetHeight(string value)
    {
        height = int.Parse(value);
        if (height < 0 || height > maxHeight)
        {
            if (height < 0)
            {
                height = 1;
            }
            else
            {
                height = maxHeight;
            }
            heightInput.GetComponent<InputField>().text = height.ToString();
        }
    }

    /// <summary>
    /// 设置输入框的展示数值
    /// </summary>
    void SetHeightAndWidthText()
    {
        heightInput.GetComponent<InputField>().text = height.ToString();
        widthInput.GetComponent<InputField>().text = width.ToString();
    }

    /// <summary>
    /// 生成地图/重新生成地图按钮点击
    /// </summary>
    void OnGenerateMap()
    {
        MapEvents.GenerateMapParam param = new MapEvents.GenerateMapParam();
        param.height = height;
        param.width = width;
        generateMapEvent.Invoke(param);
        //Debug.Log("发送生成地图事件:" + param);
    }
}
