using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmController : Singleton<AlgorithmController>
{
    [SerializeField] GameObject[] algorithmBtnArray;
    //选中的算法
    MapEnums.AlgorithmEnum algorithm;
    public MapEnums.AlgorithmEnum Algorithm
    {
        get
        {
            return algorithm;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 算法选择按钮
    /// </summary>
    /// <param name="i"></param>
    public void OnClick(int i)
    {
        foreach (MapEnums.AlgorithmEnum algorithm in Enum.GetValues(typeof(MapEnums.AlgorithmEnum)))
        {
            if ((int)algorithm == i)
            {
                this.algorithm = algorithm;
            }
        }
        foreach (GameObject obj in algorithmBtnArray)
        {
            if (obj.name.ToLower().Equals(algorithm.ToString().ToLower()))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    /// <summary>
    /// 算法是否准备就绪
    /// </summary>
    /// <returns></returns>
    public bool IsReady()
    {
        return 0 != algorithm;
    }
}
