using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MapController : Singleton<MapController>
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] float size = 30f;
    [SerializeField] float startPosX = -455f;
    [SerializeField] float startPosY = 296f;
    [SerializeField] int interval = 5;

    SlotController[][] slots;

    //起点格子
    public SlotController startSlot = null;
    //终点格子
    public SlotController endSlot = null;
    //当前选中的格子
    public SlotController currentSlot = null;

    int width = 29;
    int height = 19;
    int intervalCount = 0;

    public bool stepByStepSwitch = true;

    Dictionary<MapEnums.AlgorithmEnum, IAlgorithm> algorithmMap = new Dictionary<MapEnums.AlgorithmEnum, IAlgorithm>();

    NavigationInvoker invoker = NavigationInvoker.Instance;

    //格子点击事件
    MapEvents.ClickSlotEvent clickSlotEvent = new MapEvents.ClickSlotEvent();
    public MapEvents.ClickSlotEvent ClickSlotEvent
    {
        get
        {
            return clickSlotEvent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MapModifierGenerateGroup.Instance.GenerateMapEvent.AddListener(ResetMap);

        algorithmMap.Add(MapEnums.AlgorithmEnum.BFS, new BFSAlgorithm());
        algorithmMap.Add(MapEnums.AlgorithmEnum.DFS, new DFSAlgorithm());
        algorithmMap.Add(MapEnums.AlgorithmEnum.DIJKSTRA, new DijstraAlgorithm());
        algorithmMap.Add(MapEnums.AlgorithmEnum.GREEDY, new GreedyAlgorithm());
        algorithmMap.Add(MapEnums.AlgorithmEnum.ASTAR, new AStarAlgorithm());
    }

    void FixedUpdate()
    {
        if (intervalCount % interval == 0)
        {
            invoker.StepExecute();
        }
        intervalCount++;
    }

    /// <summary>
    /// 开始寻路
    /// </summary>
    public void StartPathfinding()
    {
        IAlgorithm algorithm;
        if (algorithmMap.TryGetValue(AlgorithmController.Instance.Algorithm, out algorithm))
        {
            //StartCoroutine("CorotinePathfinding", algorithm);
            StartThreadNav(algorithm);
        }
    }

    /// <summary>
    /// 开启寻路的线程
    /// </summary>
    void StartThreadNav(IAlgorithm algorithm)
    {
        new Thread(() =>
        {
            algorithm.Pathfinding(slots, startSlot, endSlot);
        }).Start();
    }    

    /// <summary>
    /// 地图是否准备就绪
    /// </summary>
    public bool IsReady()
    {
        return null != startSlot && null != endSlot && null != slots;
    }

    /// <summary>
    /// 点击了某个格子
    /// </summary>
    /// <param name="slot"></param>
    public void ClickSlot(SlotController slot)
    {
        currentSlot = slot;
        //发送格子点击事件
        MapEvents.ClickSlotParam clickSlotParam = new MapEvents.ClickSlotParam();
        clickSlotParam.slot = slot;
        clickSlotEvent.Invoke(clickSlotParam);
    }

    /// <summary>
    /// 清理地图的寻路痕迹
    /// </summary>
    public void ClearMapNav()
    {
        if (null == slots)
        {
            return;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[i].Length; j++)
            {
                SlotController slot = slots[i][j];
                if (null != slot)
                {
                    slot.ClearPathfinding();
                }
            }
        }
    }

    /// <summary>
    /// 重置地图
    /// </summary>
    void ResetMap(MapEvents.GenerateMapParam param)
    {
        DestroyAllSlot();

        this.width = param.width;
        this.height = param.height;

        slots = new SlotController[height][];
        for (int i = 0; i < height; i++)
        {
            slots[i] = new SlotController[width];
            for (int j = 0; j < width; j++)
            {
                Vector3 pos = GeneratePosition(i, j);
                GameObject ojb = Instantiate(slotPrefab, transform, false);
                slots[i][j] = ojb.GetComponent<SlotController>();
                slots[i][j].transform.position = pos;
                slots[i][j].Pos = new Vector2(i, j);
            }
        }
    }

    /// <summary>
    /// 删除所有格子
    /// </summary>
    void DestroyAllSlot()
    {
        if (null != slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                for (int j = 0; j < slots[i].Length; j++)
                {
                    Destroy(slots[i][j]);
                }
            }
            slots = null;
        }
        startSlot = null;
        endSlot = null;
    }

    /// <summary>
    /// 生成每个slot的position
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    Vector3 GeneratePosition(int row, int col)
    {
        float posX = transform.position.x + startPosX + col * size;
        float posY = transform.position.y + startPosY - row * size;
        return new Vector3(posX, posY, 0f);
    }
}
