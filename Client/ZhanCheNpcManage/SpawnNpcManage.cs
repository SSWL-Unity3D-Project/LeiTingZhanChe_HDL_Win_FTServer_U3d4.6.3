﻿using UnityEngine;

public class SpawnNpcManage : MonoBehaviour
{
    /// <summary>
    /// 彩票数据管理.
    /// </summary>
    public SSCaiPiaoDataManage m_CaiPiaoDataManage;
    public enum NpcState
    {
        /// <summary>
        /// 战车类型.
        /// </summary>
        ZhanChe = 0,
        /// <summary>
        /// JPBoss类型.
        /// </summary>
        JPBoss = 1,
        /// <summary>
        /// SuperJPBoss类型.
        /// </summary>
        SuperJPBoss = 2,
    }
    /// <summary>
    /// 产生点方位.
    /// </summary>
    public enum SpawnPointState
    {
        /// <summary>
        /// 不产生战车、JPBoss和SuperJPBoss.
        /// </summary>
        Null = -1,
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
    }
    [System.Serializable]
    public class NpcData
    {
        /// <summary>
        /// 战车npc预制.
        /// </summary>
        public GameObject[] ZhanChePrefabGp;
        /// <summary>
        /// JPBoss预制.
        /// </summary>
        public GameObject[] JPBossPrefabGp;
        /// <summary>
        /// 超级JPBoss预制.
        /// </summary>
        public GameObject[] SuperJPBossPrefabGp;
        /// <summary>
        /// 左边创建npc的产生点组.
        /// </summary>
        public SSCreatNpcData[] LeftSpawnPointGp;
        /// <summary>
        /// 右边创建npc的产生点组.
        /// </summary>
        public SSCreatNpcData[] RightSpawnPointGp;
        /// <summary>
        /// 上边创建npc的产生点组.
        /// </summary>
        public SSCreatNpcData[] UpSpawnPointGp;
        /// <summary>
        /// 下边创建npc的产生点组.
        /// </summary>
        public SSCreatNpcData[] DownSpawnPointGp;
    }
    /// <summary>
    /// npc预制数据.
    /// </summary>
    public NpcData m_NpcData;

    /// <summary>
    /// 创建npc的数据.
    /// </summary>
    public class NpcSpawnData
    {
        /// <summary>
        /// npc预制.
        /// </summary>
        public GameObject NpcPrefab;
        /// <summary>
        /// npc路径.
        /// </summary>
        public NpcPathCtrl NpcPath;
        /// <summary>
        /// npc产生点组件.
        /// </summary>
        public XKSpawnNpcPoint SpawnPoint;

        /// <summary>
        /// 产生npc.
        /// </summary>
        public GameObject CreatPointNpc()
        {
            if (SpawnPoint != null)
            {
                SpawnPoint.NpcObj = NpcPrefab;
                SpawnPoint.NpcPath = NpcPath.transform;
                //不进行循环产生npc.
                SpawnPoint.SpawnMaxNpc = 1;
                return SpawnPoint.CreatPointNpc();
            }
            else
            {
                Debug.LogWarning("Unity: CreatPointNpc -> SpawnPoint was null");
                return null;
            }
        }
    }

    /// <summary>
    /// 创建战车npc的状态.
    /// </summary>
    [System.Serializable]
    public class CreatZhanCheState
    {
        /// <summary>
        /// 打开左边.
        /// </summary>
        public bool IsOpenLeft = false;
        public bool IsOpenRight = false;
        public bool IsOpenUp = false;
        public bool IsOpenDown = false;

        /// <summary>
        /// 获取战车、JPBoss和SuperJPBoss的产生点方位.
        /// </summary>
        public SpawnPointState GetSpawnPointState()
        {
            if (!IsOpenLeft
                && !IsOpenRight
                && !IsOpenUp
                && !IsOpenDown)
            {
                return SpawnPointState.Null;
            }

            SpawnPointState type = SpawnPointState.Null;
            int rv = 0;
            int count = 0;
            bool isFindPointState = false;
            do
            {
                count++;
                for (int i = 0; i < 4; i++)
                {
                    if (count >= 3)
                    {
                        //超过3次检索就不再随机了.
                        rv = 1;
                    }
                    else
                    {
                        rv = Random.Range(0, 100) % 2;
                    }

                    switch (i)
                    {
                        case 0:
                            {
                                if (IsOpenLeft && rv == 1)
                                {
                                    type = SpawnPointState.Left;
                                    isFindPointState = true;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (IsOpenRight && rv == 1)
                                {
                                    type = SpawnPointState.Right;
                                    isFindPointState = true;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (IsOpenUp && rv == 1)
                                {
                                    type = SpawnPointState.Up;
                                    isFindPointState = true;
                                }
                                break;
                            }
                        case 3:
                            {
                                if (IsOpenDown && rv == 1)
                                {
                                    type = SpawnPointState.Down;
                                    isFindPointState = true;
                                }
                                break;
                            }
                    }

                    if (isFindPointState)
                    {
                        break;
                    }
                }
            } while (!isFindPointState);
            return type;
        }
    }
    /// <summary>
    /// 创建战车npc的状态.
    /// </summary>
    [HideInInspector]
    public CreatZhanCheState m_CreatZhanCheState;

    [System.Serializable]
    public class ZhanCheRulerData
    {
        /// <summary>
        /// 战车产生的最小间隔时间.
        /// </summary>
        public float TimeMin = 10f;
        /// <summary>
        /// 战车产生的最大间隔时间.
        /// </summary>
        public float TimeMax = 20f;
        [HideInInspector]
        public float LastTime = 0f;

        float _RandTime = 0f;
        /// <summary>
        /// JPBoss产生的间隔时间
        /// </summary>
        public float RandTime
        {
            set
            {
                _RandTime = value;
            }
            get
            {
                return _RandTime;
            }
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        public void Init()
        {
            RandTime = Random.Range(TimeMin, TimeMax);
            LastTime = Time.time;
        }

        /// <summary>
        /// 信息重置.
        /// </summary>
        public void Reset()
        {
            RandTime = Random.Range(TimeMin, TimeMax);
            LastTime = Time.time;
        }

        public enum ZhanCheJiBaoState
        {
            /// <summary>
            /// 各机位投币数量相同：爆率均为30%
            /// </summary>
            State1 = 0,
            /// <summary>
            /// 各机位当前统计投币数量不同：情形1：某位最多，其它两位相同，则最多的爆率为40%，其它两位各25%
            /// </summary>
            State2 = 1,
            /// <summary>
            /// 各机位当前统计投币数量不同：情形2：三位均不同，则按大小顺讯爆率为40%、30%、20%。
            /// </summary>
            State3 = 2,
            /// <summary>
            /// 各机位当前统计投币数量不同：情形3：两位相同并多于另一位，爆率40%、40%、10%。
            /// </summary>
            State4 = 3,
        }

        [System.Serializable]
        public class ZhanCheJiBaoRuler
        {
            /// <summary>
            /// 战车击爆状态.
            /// </summary>
            public ZhanCheJiBaoState m_ZhanCheJiBaoState;
            /// <summary>
            /// 最高击爆概率.
            /// </summary>
            public float MaxJiBaoGaiLv = 0f;
            /// <summary>
            /// 中间击爆概率.
            /// </summary>
            public float CenJiBaoGaiLv = 0f;
            /// <summary>
            /// 最低击爆概率.
            /// </summary>
            public float MinJiBaoGaiLv = 0f;
        }
        /// <summary>
        /// 战车击爆规则.
        /// </summary>
        public ZhanCheJiBaoRuler[] m_ZhanCheJiBaoRuler = new ZhanCheJiBaoRuler[4];
    }
    /// <summary>
    /// 战车创建和击爆规则数据.
    /// </summary>
    public ZhanCheRulerData m_ZhanCheRulerData = new ZhanCheRulerData();
    
    /// <summary>
    /// 获取可以被哪个玩家击爆,通过击爆规则的产生.
    /// </summary>
    public PlayerEnum GetPlayerIndexByJiBaoGaiLv()
    {
        PlayerEnum index = PlayerEnum.Null;
        SSCaiPiaoDataManage.PlayerCoinData[] coinDt = XkPlayerCtrl.GetInstanceFeiJi().m_SpawnNpcManage.m_CaiPiaoDataManage.GetSortPlayerCoinData();
        ZhanCheRulerData.ZhanCheJiBaoState type = ZhanCheRulerData.ZhanCheJiBaoState.State1;
        if (coinDt[0].XuBiVal == coinDt[1].XuBiVal && coinDt[1].XuBiVal == coinDt[2].XuBiVal)
        {
            type = ZhanCheRulerData.ZhanCheJiBaoState.State1;
        }
        else if (coinDt[0].XuBiVal > coinDt[1].XuBiVal && coinDt[1].XuBiVal == coinDt[2].XuBiVal)
        {
            type = ZhanCheRulerData.ZhanCheJiBaoState.State2;
        }
        else if (coinDt[0].XuBiVal > coinDt[1].XuBiVal && coinDt[1].XuBiVal > coinDt[2].XuBiVal)
        {
            type = ZhanCheRulerData.ZhanCheJiBaoState.State3;
        }
        else if (coinDt[0].XuBiVal == coinDt[1].XuBiVal && coinDt[1].XuBiVal > coinDt[2].XuBiVal)
        {
            type = ZhanCheRulerData.ZhanCheJiBaoState.State4;
        }

        Debug.Log("Unity: GetPlayerIndexByJiBaoGaiLv::xuBiVal -> " + coinDt[0].XuBiVal + ", " + coinDt[1].XuBiVal + ", " + coinDt[2].XuBiVal
            + ", type ====== " + type);
        ZhanCheRulerData.ZhanCheJiBaoRuler ruler = m_ZhanCheRulerData.m_ZhanCheJiBaoRuler[(int)type];
        float rv = Random.Range(0f, 100f) / 100f;
        if (rv < ruler.MaxJiBaoGaiLv)
        {
            index = coinDt[0].IndexPlayer;
        }
        else if (rv < ruler.MaxJiBaoGaiLv + ruler.CenJiBaoGaiLv)
        {
            index = coinDt[1].IndexPlayer;
        }
        else if (rv < ruler.MaxJiBaoGaiLv + ruler.CenJiBaoGaiLv + ruler.MinJiBaoGaiLv)
        {
            index = coinDt[2].IndexPlayer;
        }
        Debug.Log("Unity: GetPlayerIndexByJiBaoGaiLv::xuBiVal -> index ============= " + index);
        return index;
    }

    [System.Serializable]
    public class JPBossRulerData
    {
        /// <summary>
        /// JPBoss产生的最小间隔时间.
        /// </summary>
        public float TimeMin = 40f;
        /// <summary>
        /// JPBoss产生的最大间隔时间.
        /// </summary>
        public float TimeMax = 60f;
        bool _IsPlayerXuBi = false;
        /// <summary>
        /// 玩家是否续币.
        /// </summary>
        public bool IsPlayerXuBi
        {
            set
            {
                _IsPlayerXuBi = value;
                if (value == true)
                {
                    //更新玩家续币的时间记录.
                    LastXuBiTime = Time.time;
                }
            }
            get
            {
                return _IsPlayerXuBi;
            }
        }
        /// <summary>
        /// 续币最小时间.
        /// </summary>
        public float TimeXuBiMin = 3f;
        /// <summary>
        /// 续币最大时间.
        /// </summary>
        public float TimeXuBiMax = 5f;
        [HideInInspector]
        public float LastTime = 0f;
        /// <summary>
        /// 玩家续币的时间记录.
        /// </summary>
        [HideInInspector]
        public float LastXuBiTime = 0f;

        float _RandTime = 0f;
        /// <summary>
        /// JPBoss产生的间隔时间
        /// </summary>
        public float RandTime
        {
            set
            {
                _RandTime = value;
            }
            get
            {
                return _RandTime;
            }
        }

        float _RandTimeXuBi = 0f;
        /// <summary>
        /// JPBoss产生的续币间隔时间
        /// </summary>
        public float RandTimeXuBi
        {
            set
            {
                _RandTimeXuBi = value;
            }
            get
            {
                return _RandTimeXuBi;
            }
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        public void Init()
        {
            RandTime = Random.Range(TimeMin, TimeMax);
            RandTimeXuBi = Random.Range(TimeXuBiMin, TimeXuBiMax);
            LastTime = Time.time;
        }

        /// <summary>
        /// 信息重置.
        /// </summary>
        public void Reset()
        {
            RandTime = Random.Range(TimeMin, TimeMax);
            RandTimeXuBi = Random.Range(TimeXuBiMin, TimeXuBiMax);
            LastTime = Time.time;
            IsPlayerXuBi = false;
        }
    }
    /// <summary>
    /// JPBoss创建规则.
    /// JPBoss击爆条件和战车相同.
    /// </summary>
    public JPBossRulerData m_JPBossRulerData = new JPBossRulerData();

    /// <summary>
    /// 战车和JPBoss的创建状态.
    /// </summary>
    public class ZhanCheJPBossData
    {
        /// <summary>
        /// 是否可以产生战车.
        /// </summary>
        public bool IsCreatZhanChe = false;
        /// <summary>
        /// 是否可以产生JPBoss.
        /// </summary>
        public bool IsCreatJPBoss = false;
        /// <summary>
        /// 是否可以产生JPBoss.
        /// </summary>
        public bool IsCreatSuperJPBoss = false;
        /// <summary>
        /// 战车数据.
        /// </summary>
        public SSNpcDateManage ZhanCheData;
        /// <summary>
        /// JPBoss数据.
        /// </summary>
        public SSNpcDateManage JPBossData;
        /// <summary>
        /// SuperJPBoss数据.
        /// </summary>
        public SSNpcDateManage SuperJPBossData;
        public void Init(GameObject obj)
        {
            if (obj != null)
            {
                ZhanCheData = obj.AddComponent<SSNpcDateManage>();
                JPBossData = obj.AddComponent<SSNpcDateManage>();
                SuperJPBossData = obj.AddComponent<SSNpcDateManage>();
            }
        }
    }
    /// <summary>
    /// 战车和JPBoss的创建状态.
    /// </summary>
    public ZhanCheJPBossData m_ZhanCheJPBossData = new ZhanCheJPBossData();

    void Awake()
    {
        m_CaiPiaoDataManage.Init();
    }

    void Start()
    {
        m_ZhanCheJPBossData.Init(gameObject);
        m_ZhanCheRulerData.Init();
        m_JPBossRulerData.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CreatNpcObj(NpcState.JPBoss, m_CreatZhanCheState.GetSpawnPointState()); //test.
        }

        if (XkGameCtrl.PlayerActiveNum <= 0)
        {
            //没有玩家激活时.
            return;
        }

        if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_AiPathGroup != null
            && XkGameCtrl.GetInstance().m_AiPathGroup.m_CameraMoveType == AiPathGroupCtrl.MoveState.YuLe)
        {
            //娱乐阶段不产生npc.
            return;
        }

        if (Time.time - m_ZhanCheRulerData.LastTime >= m_ZhanCheRulerData.RandTime)
        {
            //检测是否可以产生战车.
            if (!m_ZhanCheJPBossData.IsCreatZhanChe)
            {
                //m_ZhanCheJPBossData.IsCreatZhanChe = true;
            }
        }

        if (Time.time - m_JPBossRulerData.LastTime >= m_JPBossRulerData.RandTime)
        {
            //检测是否可以产生JPBoss.
            if (!m_ZhanCheJPBossData.IsCreatJPBoss)
            {
                m_ZhanCheJPBossData.IsCreatJPBoss = true;
            }
        }

        if (m_JPBossRulerData.IsPlayerXuBi)
        {
            //玩家已经续币.
            if (Time.time - m_JPBossRulerData.LastXuBiTime >= m_JPBossRulerData.RandTimeXuBi)
            {
                //检测是否可以产生JPBoss.
                if (!m_ZhanCheJPBossData.IsCreatJPBoss)
                {
                    m_ZhanCheJPBossData.IsCreatJPBoss = true;
                }
            }
        }

        if (m_ZhanCheJPBossData.IsCreatSuperJPBoss)
        {
            //优先产生SuperJPBoss.
            if (!m_ZhanCheJPBossData.ZhanCheData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.JPBossData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.SuperJPBossData.GetIsHaveNpc())
            {
                //没有战车、JPBoss和SuperJPBoss时产生JPBoss.
                CreatNpcObj(NpcState.SuperJPBoss, m_CreatZhanCheState.GetSpawnPointState());
            }
        }
        else if (m_ZhanCheJPBossData.IsCreatJPBoss)
        {
            //其次产生JPBoss.
            if (!m_ZhanCheJPBossData.ZhanCheData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.JPBossData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.SuperJPBossData.GetIsHaveNpc())
            {
                //没有战车、JPBoss和SuperJPBoss时产生JPBoss.
                CreatNpcObj(NpcState.JPBoss, m_CreatZhanCheState.GetSpawnPointState());
            }
        }
        else if (m_ZhanCheJPBossData.IsCreatZhanChe)
        {
            //最后产生战车.
            if (!m_ZhanCheJPBossData.ZhanCheData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.JPBossData.GetIsHaveNpc()
                && !m_ZhanCheJPBossData.SuperJPBossData.GetIsHaveNpc())
            {
                //没有战车、JPBoss和SuperJPBoss时产生JPBoss.
                CreatNpcObj(NpcState.ZhanChe, m_CreatZhanCheState.GetSpawnPointState());
            }
        }
    }

    public void ResetCreatNpcInfo(NpcState type)
    {
        Debug.Log("ResetCreatNpcInfo -> type =================== " + type);
        switch (type)
        {
            case NpcState.ZhanChe:
                {
                    m_ZhanCheJPBossData.IsCreatZhanChe = false;
                    m_ZhanCheRulerData.Reset();
                    break;
                }
            case NpcState.JPBoss:
                {
                    m_ZhanCheJPBossData.IsCreatJPBoss = false;
                    m_JPBossRulerData.Reset();
                    break;
                }
            case NpcState.SuperJPBoss:
                {
                    m_ZhanCheJPBossData.IsCreatSuperJPBoss = false;
                    break;
                }
        }
    }

    /// <summary>
    /// 创建npc.
    /// npcType 产生的npc类型.
    /// pointState 产生点的方位信息.
    /// </summary>
    void CreatNpcObj(NpcState npcType, SpawnPointState pointState)
    {
        //Debug.Log("Unity: CreatNpcObj -> npcType ====== " + npcType + ", pointState ======= " + pointState);
        NpcSpawnData data = GetNpcSpawnData(npcType, pointState);
        if (data != null)
        {
            GameObject obj = data.CreatPointNpc();
            if (obj != null)
            {
                switch (npcType)
                {
                    case NpcState.ZhanChe:
                        {
                            if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_AiPathGroup != null
                                && XkGameCtrl.GetInstance().m_AiPathGroup.m_CameraMoveType != AiPathGroupCtrl.MoveState.YuLe)
                            {
                                XkGameCtrl.GetInstance().m_AiPathGroup.SetCameraMoveType(AiPathGroupCtrl.MoveState.Default);
                            }
                            m_ZhanCheJPBossData.ZhanCheData.AddNpcToList(obj);
                            break;
                        }
                    case NpcState.JPBoss:
                        {
                            if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_AiPathGroup != null
                                && XkGameCtrl.GetInstance().m_AiPathGroup.m_CameraMoveType != AiPathGroupCtrl.MoveState.YuLe)
                            {
                                XkGameCtrl.GetInstance().m_AiPathGroup.SetCameraMoveType(AiPathGroupCtrl.MoveState.Boss);
                            }
                            m_ZhanCheJPBossData.JPBossData.AddNpcToList(obj);
                            break;
                        }
                    case NpcState.SuperJPBoss:
                        {
                            if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_AiPathGroup != null
                                && XkGameCtrl.GetInstance().m_AiPathGroup.m_CameraMoveType != AiPathGroupCtrl.MoveState.YuLe)
                            {
                                XkGameCtrl.GetInstance().m_AiPathGroup.SetCameraMoveType(AiPathGroupCtrl.MoveState.Boss);
                            }
                            m_ZhanCheJPBossData.SuperJPBossData.AddNpcToList(obj);
                            break;
                        }
                }
            }
        }
    }

    NpcSpawnData GetNpcSpawnData(NpcState npcType, SpawnPointState pointState)
    {
        if (pointState == SpawnPointState.Null)
        {
            //不产生战车、JPBoss和SuperJPBoss.
            return null;
        }

        NpcSpawnData data = new NpcSpawnData();
        //获取ncp预制.
        data.NpcPrefab = GetNpcPrefab(npcType);
        SSCreatNpcData creatNpcDt = GetCreatNpcData(pointState);
        if (creatNpcDt != null)
        {
            //获取npc路径.
            data.NpcPath = creatNpcDt.GetNpcPahtData();
            //获取产生点组件.
            data.SpawnPoint = creatNpcDt.m_SpawnPoint;
        }
        return data;
    }

    /// <summary>
    /// 获取创建npc的组件.
    /// </summary>
    SSCreatNpcData GetCreatNpcData(SpawnPointState type)
    {
        SSCreatNpcData[] comGp = null;
        switch (type)
        {
            case SpawnPointState.Left:
                {
                    comGp = m_NpcData.LeftSpawnPointGp;
                    break;
                }
            case SpawnPointState.Right:
                {
                    comGp = m_NpcData.RightSpawnPointGp;
                    break;
                }
            case SpawnPointState.Up:
                {
                    comGp = m_NpcData.UpSpawnPointGp;
                    break;
                }
            case SpawnPointState.Down:
                {
                    comGp = m_NpcData.DownSpawnPointGp;
                    break;
                }
        }

        if (comGp == null || comGp.Length <= 0)
        {
            Debug.LogWarning("Unity: not find CreatNpcData! type ================ " + type);
            return null;
        }

        SSCreatNpcData com = null;
        int rv = Random.Range(0, 100) % comGp.Length;
        com = comGp[rv];
        if (com == null)
        {
            Debug.LogWarning("Unity: com was null! rv ============ " + rv + ", type == " + type);
        }
        return com;
    }

    /// <summary>
    /// 获取npc预制.
    /// </summary>
    GameObject GetNpcPrefab(NpcState type)
    {
        GameObject[] npcPrefabGp = null;
        switch (type)
        {
            case NpcState.ZhanChe:
                {
                    npcPrefabGp = m_NpcData.ZhanChePrefabGp;
                    break;
                }
            case NpcState.JPBoss:
                {
                    npcPrefabGp = m_NpcData.JPBossPrefabGp;
                    break;
                }
            case NpcState.SuperJPBoss:
                {
                    npcPrefabGp = m_NpcData.SuperJPBossPrefabGp;
                    break;
                }
        }

        if (npcPrefabGp == null || npcPrefabGp.Length <= 0)
        {
            Debug.LogWarning("Unity: not find npc! type ================ " + type);
            return null;
        }

        GameObject npcPrefab = null;
        int rv = Random.Range(0, 100) % npcPrefabGp.Length;
        npcPrefab = npcPrefabGp[rv];
        if (npcPrefab == null)
        {
            Debug.LogWarning("Unity: npcPrefab was null! rv ============ " + rv + ", type == " + type);
        }
        return npcPrefab;
    }
}