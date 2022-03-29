using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class NavigationInvoker
{
    private static NavigationInvoker instance;
    private static object obj = new object();
    public static NavigationInvoker Instance
    {
        get
        {
            if (null == instance)
            {
                lock (obj)
                {
                    if (null == instance)
                    {
                        instance = new NavigationInvoker();
                    }
                }
            }
            return instance;
        }
    }
 
    private NavigationInvoker() { }

    //List<ICommand> commandList = new List<ICommand>();
    ConcurrentQueue<ICommand> commandList = new ConcurrentQueue<ICommand>();
    //int point = 0;

    /// <summary>
    /// 添加命令
    /// </summary>
    /// <param name="command"></param>
    public void AddCommond(ICommand command)
    {
        Debug.Log("添加命令");
        //commandList.Add(command);
        commandList.Enqueue(command);
    }

    /// <summary>
    /// 单步执行命令
    /// </summary>
    public void StepExecute()
    {
        //if (point < 0 || point > commandList.Count - 1)
        //{
        //    return;
        //}
        //commandList[point].Execute();
        //point++;

        //if (commandList[point] is PathEndingCommond)
        //{
        //    Clear(point);
        //}
        ICommand command;
        if (!commandList.TryDequeue(out command))
        {
            return;
        }
        command.Execute();
    }

    /// <summary>
    /// 清除命令
    /// </summary>
    //public void Clear(int index)
    //{
    //    commandList = new List<ICommand>();
    //    point = 0;
    //}
}
