﻿using UnityEngine;
using System.Collections;

public class SSGameUICtrl : SSGameMono
{
    /// <summary>
    /// 游戏UI界面中心锚点.
    /// </summary>
    public Transform m_GameUICenter;
    /// <summary>
    /// 玩家UI父级tr.
    /// </summary>
    public Transform[] m_PlayerUIParent = new Transform[3];

    void Awake()
    {
        if (SSUIRoot.GetInstance() != null)
        {
            SSUIRoot.GetInstance().m_GameUIManage = this;
        }
    }
    
    /// <summary>
    /// 彩票不足UI.
    /// </summary>
    internal SSCaiPiaoBuZu[] m_CaiPiaoBuZuArray = new SSCaiPiaoBuZu[3];
    /// <summary>
    /// 创建彩票不足UI界面.
    /// </summary>
    public void CreatCaiPiaoBuZuPanel(PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("CreatCaiPiaoBuZuPanel -> index was wrong! index ==== " + index);
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/CaiPiaoUI/CaiPiaoBuZu");
        if (gmDataPrefab != null)
        {
            if (m_CaiPiaoBuZuArray[index] == null)
            {
                if (m_PlayerUIParent[index] != null)
                {
                    UnityLog("CreatCaiPiaoBuZuPanel...");
                    GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_PlayerUIParent[index]);
                    m_CaiPiaoBuZuArray[index] = obj.GetComponent<SSCaiPiaoBuZu>();
                    m_CaiPiaoBuZuArray[index].Init(indexPlayer);
                    SetActiveZhengZaiChuPiaoUI(indexPlayer, false);
                }
                else
                {
                    UnityLogWarning("CreatCaiPiaoBuZuPanel -> m_PlayerUIParent was wrong! index ==== " + index);
                }
            }
        }
        else
        {
            UnityLogWarning("CreatCaiPiaoBuZuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除彩票不足UI.
    /// </summary>
    public void RemoveCaiPiaoBuZuPanel(PlayerEnum indexPlayer, bool isWorkerDone)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("RemoveCaiPiaoBuZuPanel -> index was wrong! index ==== " + index);
            return;
        }

        if (m_CaiPiaoBuZuArray[index] != null)
        {
            UnityLog("RemoveCaiPiaoBuZuPanel -> index ==== " + index);
            m_CaiPiaoBuZuArray[index].RemoveSelf();
            SetActiveZhengZaiChuPiaoUI(indexPlayer, true);

            if (isWorkerDone)
            {
                //工作人员清票.
                //这里添加彩票不足机位的彩票数据清理逻辑代码.
                if (XkPlayerCtrl.GetInstanceFeiJi() != null)
                {
                    XkPlayerCtrl.GetInstanceFeiJi().m_SpawnNpcManage.m_CaiPiaoDataManage.ClearPlayerCaiPiaoData(indexPlayer);
                }
            }
        }
    }

    /// <summary>
    /// 删除所有彩票不足界面.
    /// </summary>
    public void RemoveAllCaiPiaoBuZuPanel()
    {
        //工作人员清票.
        for (int i = 0; i < m_CaiPiaoBuZuArray.Length; i++)
        {
            if (m_CaiPiaoBuZuArray[i] != null)
            {
                RemoveCaiPiaoBuZuPanel((PlayerEnum)(i + 1), true);
            }
        }
    }
    
    /// <summary>
    /// 玩家彩票数量UI.
    /// </summary>
    internal SSGameNumUI[] m_CaiPiaoInfoArray = new SSGameNumUI[3];
    /// <summary>
    /// 创建玩家彩票数量UI界面.
    /// </summary>
    void CreatCaiPiaoInfoPanel(PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("CreatCaiPiaoInfoPanel -> index was wrong! index ==== " + index);
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/CaiPiaoUI/CaiPiaoInfo");
        if (gmDataPrefab != null)
        {
            if (m_CaiPiaoInfoArray[index] == null)
            {
                if (m_PlayerUIParent[index] != null)
                {
                    UnityLog("CreatCaiPiaoInfoPanel...");
                    GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_PlayerUIParent[index]);
                    m_CaiPiaoInfoArray[index] = obj.GetComponent<SSGameNumUI>();
                    SetActiveZhengZaiChuPiaoUI(indexPlayer, true);
                }
                else
                {
                    UnityLogWarning("CreatCaiPiaoInfoPanel -> m_PlayerUIParent was wrong! index ==== " + index);
                }
            }
        }
        else
        {
            UnityLogWarning("CreatCaiPiaoInfoPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家彩票数量UI.
    /// </summary>
    void RemoveCaiPiaoInfoPanel(PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("RemoveCaiPiaoInfoPanel -> index was wrong! index ==== " + index);
            return;
        }

        if (m_CaiPiaoInfoArray[index] != null)
        {
            UnityLog("RemoveCaiPiaoInfoPanel -> index ==== " + index);
            Destroy(m_CaiPiaoInfoArray[index].gameObject);
        }
    }

    void SetActiveZhengZaiChuPiaoUI(PlayerEnum indexPlayer, bool isActive)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("SetActiveZhengZaiChuPiaoUI -> index was wrong! index ==== " + index);
            return;
        }

        if (m_CaiPiaoInfoArray[index] != null)
        {
            SSCaiPiaoInfo caiPiaoInfo = m_CaiPiaoInfoArray[index].GetComponent<SSCaiPiaoInfo>();
            if (caiPiaoInfo != null)
            {
                caiPiaoInfo.SetActiveZhengZaiChuPiao(isActive);
            }
            else
            {
                UnityLogWarning("SetActiveZhengZaiChuPiaoUI -> caiPiaoInfo was null!");
            }
        }
    }

    /// <summary>
    /// 是否删除玩家彩票UI.
    /// </summary>
    bool[] IsRemoveCaiPiaoInfo = new bool[3];
    public void ShowPlayerCaiPiaoInfo(PlayerEnum indexPlayer, int num)
    {
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("ShowPlayerCaiPiaoInfo -> index was wrong! index ==== " + index);
            return;
        }

        if (num <= 0)
        {
            //删除玩家彩票UI信息.
            if (IsRemoveCaiPiaoInfo[index] == false)
            {
                IsRemoveCaiPiaoInfo[index] = true;
                StartCoroutine(DelayRemoveCaiPiaoInfoPanle(indexPlayer));
            }
        }
        else
        {
            if (IsRemoveCaiPiaoInfo[index] == true)
            {
                //重置信息.
                IsRemoveCaiPiaoInfo[index] = false;
            }

            if (m_CaiPiaoInfoArray[index] == null)
            {
                //创建彩票数据信息.
                CreatCaiPiaoInfoPanel(indexPlayer);
            }

            if (m_CaiPiaoInfoArray[index] != null)
            {
                //显示彩票数量UI.
                m_CaiPiaoInfoArray[index].ShowNumUI(num);
            }
        }
    }

    /// <summary>
    /// 延迟删除彩票数据信息.
    /// </summary>
    IEnumerator DelayRemoveCaiPiaoInfoPanle(PlayerEnum indexPlayer)
    {
        yield return new WaitForSeconds(1f);
        int index = (int)indexPlayer - 1;
        if (index < 0 || index > 2)
        {
            UnityLogWarning("DelayRemoveCaiPiaoInfoPanle -> index was wrong! index ==== " + index);
            yield break;
        }

        if (IsRemoveCaiPiaoInfo[index] == true)
        {
            RemoveCaiPiaoInfoPanel(indexPlayer);
        }
    }

    /// <summary>
    /// 彩票大奖UI控制组件.
    /// </summary>
    internal SSCaiPiaoDaJiang m_CaiPiaoDaJiang;
    /// <summary>
    /// 玩家获得JPBoss大奖时创建该界面.
    /// 创建彩票大奖UI界面.
    /// </summary>
    public void CreatCaiPiaoDaJiangPanel(PlayerEnum indexPlayer, int num)
    {
        int index = (int)indexPlayer - 1;
        if (index >= 0 && index <= 2)
        {
            if (m_CaiPiaoDaJiang == null)
            {
                GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/CaiPiaoUI/CaiPiaoDaJiang");
                if (gmDataPrefab != null)
                {
                    if (m_GameUICenter != null)
                    {
                        UnityLog("CreateCaiPiaoDaJiangPanel...");
                        GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUICenter);
                        m_CaiPiaoDaJiang = obj.GetComponent<SSCaiPiaoDaJiang>();
                        if (m_CaiPiaoDaJiang != null)
                        {
                            m_CaiPiaoDaJiang.ShowDaJiangCaiPiaoNum(indexPlayer, num);
                        }
                    }
                    else
                    {
                        UnityLogWarning("CreateCaiPiaoDaJiangPanel -> m_GameUICenter was null!");
                    }
                }
            }
        }
        else
        {
            UnityLogWarning("CreateCaiPiaoDaJiangPanel -> index was wrong! index ======= " + index);
        }
    }

    /// <summary>
    /// 玩家彩票出完后删除该界面.
    /// 删除彩票大奖UI界面.
    /// </summary>
    public void RemoveCaiPiaoDaJiangPanel()
    {
        if (m_CaiPiaoDaJiang != null)
        {
            UnityLog("RemoveCaiPiaoDaJiangPanel...");
            Destroy(m_CaiPiaoDaJiang.gameObject);
        }
    }
}