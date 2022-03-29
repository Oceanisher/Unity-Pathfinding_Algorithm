using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptController : MonoBehaviour
{
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject clearBtn;

    // Start is called before the first frame update
    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(OnStartBtn);
        clearBtn.GetComponent<Button>().onClick.AddListener(OnClearBtn);
    }

    /// <summary>
    /// 开始寻路
    /// </summary>
    void OnStartBtn()
    {
        GameManager.Instance.StartPathfinding();
    }

    /// <summary>
    /// 清理寻路
    /// </summary>
    void OnClearBtn()
    {
        GameManager.Instance.ClearPathfinding();
    }
}
