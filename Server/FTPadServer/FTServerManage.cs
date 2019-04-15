using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GClientLib;
using System.Collections;
using System.Net.NetworkInformation;

namespace Server.FTPadServer
{
    /// <summary>
    /// 纷腾服务器管理组件.
    /// </summary>
    public class FTServerManage : MonoBehaviour
    {
        public UnityEngine.UI.Image img;
        //public Text ptext;
        /// <summary>
        /// 纷腾服务器消息缓存容器.
        /// </summary>
        List<string> MsgList;
        /// <summary>
        /// 纷腾服务器手柄方向消息缓存容器.
        /// </summary>
        List<string> MsgListDirection;
        /// <summary>
        /// 纷腾服务器手柄按键消息缓存容器.
        /// </summary>
        List<string> MsgListButton;
        //public UnityEngine.UI.Image puser;
        SocketLib dll_MainLib;
        //byte[] buffer;
        //Sprite temp;
        //Transform tuser;
        private string nSessionGuid;
        public string SessionID
        {
            get { return nSessionGuid; }
            set { nSessionGuid = value; }
        }

        private string sGlobalNum = "";
        public string GlobalNum
        {
            get { return sGlobalNum; }
            set { sGlobalNum = value; }
        }

        static FTServerManage _Instance;
        /// <summary>
        /// 创建纷腾服务器管理组件.
        /// </summary>
        public static void CreateFTServerManage()
        {
            SSDebug.Log("CreateFTServerManage----------------------------------");
            if (_Instance == null)
            {
                GameObject obj = new GameObject("_FTServerManage");
                _Instance = obj.AddComponent<FTServerManage>();
            }

            if (_Instance != null)
            {
                _Instance.Init();
            }
        }

        /// <summary>
        /// 获取纷腾服务器管理组件.
        /// </summary>
        public static FTServerManage GetInstance()
        {
            if (_Instance == null)
            {
                CreateFTServerManage();
            }
            return _Instance;
        }

        //public delegate void MyDelegate();
        //public MyDelegate _MsgBox;

        bool IsInit = false;
        /// <summary>
        /// 初始化.
        /// </summary>
        void Init()
        {
            if (m_FTServerInterface == null)
            {
                CreatFTServerInterface();
            }

            if (IsInit == true)
            {
                return;
            }
            IsInit = true;
            InitInfo();
            StartCoroutine(DelayLinkServer());
        }

        /// <summary>
        /// 延迟连接服务器.
        /// </summary>
        IEnumerator DelayLinkServer()
        {
            yield return new WaitForSeconds(5f);
            MainStart();
            yield return new WaitForSeconds(3f);
            RegLocal();
        }

        /// <summary>
        /// 在服务器进行连接.
        /// </summary>
        public void MainStart()
        {
            dll_MainLib = new SocketLib();
            dll_MainLib._GCSysMessage += A_ShowMessage;
            dll_MainLib._GCSocketClosed += A_GCSocketClosed;
            dll_MainLib._GCSocketConnected += A_GCSocketConnected;
            dll_MainLib._GCSocketError += A_GCSocketError;
            dll_MainLib._GCSocketReceived += A_GCSocketReceived;
            //纷腾服务器IP.
            string ftServerIp = "123.59.41.81";
            //纷腾服务器端口.
            int port = 10011;
            dll_MainLib.GC_StartUp(ftServerIp, port);
            //_MsgBox += ShowText;
        }

        /// <summary>
        /// 在服务器上注册当前机器信息.
        /// </summary>
        public void RegLocal()
        {
            if (dll_MainLib == null)
            {
                SSDebug.LogWarning("RegLocal -> dll_MainLib was null");
                return;
            }

            string defaultPcMac = "000000000000";
            string boxNum = defaultPcMac;
#if UNITY_STANDALONE_WIN
            try
            {
                bool isFindLocalAreaConnection = false;
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in nis)
                {
                    if (ni.Name == "本地连接" || ni.Name == "Local Area Connection")
                    {
                        isFindLocalAreaConnection = true;
                        boxNum = ni.GetPhysicalAddress().ToString();
                        break;
                    }
                }

                if (isFindLocalAreaConnection == false)
                {
                    SSDebug.LogWarning("RegLocal -> not find local area connection!");
                }
            }
            catch (Exception ex)
            {
                SSDebug.LogWarning("RegLocal -> Mac get error! ex == " + ex);
            }
#endif

            string systemInfo = boxNum;
            //systemInfo = dll_MainLib.GetSystemInfoString();
            dll_MainLib.GC_SendCommand("REG", systemInfo);
            SSDebug.Log("RegLocal -> systemInfo == " + systemInfo);
        }

        /// <summary>
        /// 服务器返回的消息.
        /// </summary>
        private void A_GCSocketReceived(string sArguement)
        {
            sArguement = sArguement.Replace("\r\n", "");
            A_ShowMessage(sArguement);

            string s = sArguement;
            string[] sagr;
            sagr = s.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "REGOK":
                        {
                            //注册成功.
                            if (sagr.Length >= 1)
                            {
                                SessionID = sagr[1];
                            }
                            if (sagr.Length >= 2)
                            {
                                GlobalNum = sagr[2];
                            }
                            //获取二维码数据.
                            dll_MainLib.GC_SendCommand("Q2CODE", GlobalNum);
                            break;
                        }
                    case "TEST":
                        {
                            //测试.
                            if (sagr.Length >= 4)
                            {
                                dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[3]);
                            }
                            else
                            {
                                dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[sagr.Length - 1]);
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 网络连接出错.
        /// </summary>
        private void A_GCSocketError(string sArg)
        {
            A_ShowMessage("Dll网络连接出错");
            A_ShowMessage(sArg);
        }

        /// <summary>
        /// 网络连接成功.
        /// </summary>
        private void A_GCSocketConnected()
        {
            A_ShowMessage("Dll网络连接成功");
        }

        /// <summary>
        /// 网络连接关闭.
        /// </summary>
        private void A_GCSocketClosed()
        {
            A_ShowMessage("Dll连接关闭");
        }

        private void A_ShowMessage(string msg)
        {
            if (msg == null || msg.Length < 1)
            {
                return;
            }

            countTest++;
            string s = msg;
            string[] args;
            args = s.Split(',');
            if (args.Length < 1)
            {
                return;
            }

            switch (args[0])
            {
                case "DATA":
                    {
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,button,0
                        if (args.Length > 2)
                        {
                            string key = args[2];
                            if (key == "move")
                            {
                                if (MsgListDirection != null)
                                {
                                    MsgListDirection.Add(msg);
                                }
                            }
                            else if (key == "button")
                            {
                                if (MsgListButton != null)
                                {
                                    MsgListButton.Add(msg);
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (MsgList != null)
                        {
                            MsgList.Add(msg);
                        }
                        break;
                    }
            }
        }

        void ShowMsg(string s)
        {
            //SSDebug.Log("ShowMsg -> msg == " + s);
            //if (ptext != null)
            //{
            //    ptext.text = System.DateTime.Now.ToString() + ":" + s + "\n";
            //}
            OnReceivedMsgFromFTServer(s);
        }

        /// <summary>
        /// 当收到纷腾服务器的回传消息.
        /// </summary>
        void OnReceivedMsgFromFTServer(string args)
        {
            if (args == null || args.Length < 1)
            {
                return;
            }
            //SSDebug.Log("OnReceivedMsgFromFTServer -> msg == " + args);

            string[] sagr = args.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "LOGIN":
                        {
                            //玩家登录手柄消息.
                            OnReceivedPlayerLoginMsg(sagr);
                            break;
                        }
                    case "DATA":
                        {
                            //玩家手柄操作消息.
                            OnReceivedPlayerPadMsg(sagr);
                            break;
                        }
                    case "WEBSESSION_CLOSE":
                        {
                            //玩家退出手柄消息.
                            OnReceivedPlayerExit(sagr);
                            break;
                        }
                    case "Q2CODE":
                        {
                            //收到游戏二维码数据消息.
                            OnReceivedGameErWeiMaMsg(sagr);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 玩家数据信息.
        /// </summary>
        [Serializable]
        public class PlayerData
        {
            /// <summary>
            /// 当前登录玩家的会话id.
            /// </summary>
            public string huiHuaId = "";
            /// <summary>
            /// 玩家Id.
            /// </summary>
            public int userId = 0;
            /// <summary>
            /// 玩家昵称.
            /// </summary>
            internal string name = "";
            /// <summary>
            /// 玩家性别.
            /// </summary>
            internal string sex = "";
            /// <summary>
            /// 玩家头像url.
            /// </summary>
            internal string headUrl = "";
        }
        /// <summary>
        /// 登录游戏的玩家数据信息列表.
        /// </summary>
        public List<PlayerData> m_LoginPlayerDt = new List<PlayerData>();

        /// <summary>
        /// 查找玩家游戏数据.
        /// </summary>
        PlayerData FindGamePlayerData(string huiHuaId)
        {
            PlayerData playerDt = m_LoginPlayerDt.Find((dt) =>
            {
                return dt.huiHuaId.Equals(huiHuaId);
            });
            return playerDt;
        }

        /// <summary>
        /// 查找玩家游戏数据.
        /// </summary>
        PlayerData FindGamePlayerData(int userId)
        {
            PlayerData playerDt = m_LoginPlayerDt.Find((dt) =>
            {
                return dt.userId.Equals(userId);
            });
            return playerDt;
        }
        
        /// <summary>
        /// 添加玩家微信数据信息.
        /// </summary>
        void AddGamePlayerData(PlayerData playerDt)
        {
            if (playerDt != null && m_LoginPlayerDt != null)
            {
                if (FindGamePlayerData(playerDt.userId) == null)
                {
                    m_LoginPlayerDt.Add(playerDt);
                }
            }
        }

        /// <summary>
        /// 删除玩家微信数据信息.
        /// </summary>
        internal void RemoveGamePlayerData(int userId)
        {
            PlayerData playerDt = FindGamePlayerData(userId);
            if (playerDt != null)
            {
                m_LoginPlayerDt.Remove(playerDt);
            }
        }

        int userIdTest = 0; //测试用户id信息.
        /// <summary>
        /// 收到玩家手柄登录消息.
        /// </summary>
        void OnReceivedPlayerLoginMsg(string[] args)
        {
            //会话id信息是当玩家每次登录后产生的.
            //LOGIN,会话id信息,机器特征码,玩家id,玩家昵称
            //LOGIN,374b1b26-ea3c-4669-aaca-7e42dc799c0e,43142003142014402211616555881165971,id,name
            //玩家登录消息.
            if (args.Length >= 3)
            {
                int userId = 0;
                string name = "";
                string sex = "";
                string headUrl = "";
                userIdTest++;
                userId = userIdTest;
                name = "test";
                sex = "1";

                if (FindGamePlayerData(userId) == null)
                {
                    //添加玩家数据.
                    PlayerData playerDt = new PlayerData();
                    playerDt.huiHuaId = args[1];
                    playerDt.userId = userId;
                    playerDt.name = name;
                    playerDt.sex = sex;
                    playerDt.headUrl = headUrl;
                    AddGamePlayerData(playerDt);
                }

                WebSocketSimpet.PlayerWeiXinData playerWeiXinDt = new WebSocketSimpet.PlayerWeiXinData();
                playerWeiXinDt.sex = sex;
                playerWeiXinDt.headUrl = headUrl;
                playerWeiXinDt.userName = name;
                playerWeiXinDt.userId = userId;
                if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                {
                    pcvr.GetInstance().m_HongDDGamePadInterface.OnPlayerLoginFromFTServer(playerWeiXinDt);

                    //测试,暂时当收到登录消息后直接发送开始按键消息.
                    StartCoroutine(TestDelaySendClickStartBtMsg(userId));
                }
            }
        }

        IEnumerator TestDelaySendClickStartBtMsg(int userId)
        {
            yield return new WaitForSeconds(2f);
            if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
            {
                //测试,暂时当收到登录消息后直接发送开始按键消息.
                //开始按键消息.
                string startBtDown = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingFireBt.startGameBtDown.ToString();
                pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveActionOperationMsgFTServer(startBtDown, userId);
            }
        }

        /// <summary>
        /// 收到玩家手柄操作消息.
        /// </summary>
        void OnReceivedPlayerPadMsg(string[] args)
        {
            //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
            //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,button,0
            if (args.Length < 3)
            {
                return;
            }

            int userId = 0; //玩家id信息.
            //userId = 123;
            string huiHuaId = args[1];
            PlayerData playerDt = FindGamePlayerData(huiHuaId);
            if (playerDt != null)
            {
                userId = playerDt.userId;
            }
            else
            {
                return;
            }

            string key = args[2];
            switch (key)
            {
                case "move":
                    {
                        //手柄方向数据消息.
                        //采用向量方式将收到的手柄坐标信息转换为方向信息.
                        //    -1
                        //-1      1
                        //     1
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
                        if (args.Length >= 5)
                        {
                            float px = Assets.XKGame.Script.Comm.MathConverter.StringToFloat(args[3]);
                            float py = Assets.XKGame.Script.Comm.MathConverter.StringToFloat(args[4]);
                            if (px == py && px == 0f)
                            {
                                //玩家手指离开方向.
                                string angle = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingDir.up.ToString();
                                if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                                {
                                    pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveDirectionAngleMsgFTServer(angle, userId);
                                }
                            }
                            else
                            {
                                Vector2 vP0 = new Vector2(-1f, 0f);
                                Vector2 vP1 = new Vector2(px, py);
                                vP1 = vP1.normalized;
                                float cosVal = Vector2.Dot(vP0, vP1);
                                float sign = py > 0f ? -1f : 1f; //方向向下时角度为负数,方向向上时角度为正数.
                                float angle = sign * Mathf.Acos(cosVal) * Mathf.Rad2Deg;
                                //SSDebug.Log("angle =================== " + angle);
                                if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                                {
                                    pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveDirectionAngleMsgFTServer(angle.ToString(), userId);
                                }
                            }
                        }
                        break;
                    }
                case "button":
                    {
                        //手柄按键消息.
                        if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                        {
                            //发射按键消息.
                            string fireBtDown = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingFireBt.fireBDown.ToString();
                            pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveActionOperationMsgFTServer(fireBtDown, userId);
                        }
                        break;
                    }
            }
        }
        
        /// <summary>
        /// 收到玩家退出手柄消息.
        /// </summary>
        void OnReceivedPlayerExit(string[] args)
        {
            //WEBSESSION_CLOSE,374b1b26-ea3c-4669-aaca-7e42dc799c0e,ClientClosing
        }

        /// <summary>
        /// 收到游戏二维码数据消息.
        /// </summary>
        void OnReceivedGameErWeiMaMsg(string[] args)
        {
            //二维码信息返回.
            try
            {
                if (args.Length > 1)
                {
                    byte[] buffer = dll_MainLib.GC_GetQRCodeBitmap(args[1], 100, 100);
                    OnReceivedErWeiMaData(buffer);
                }
                else
                {
                    SSDebug.LogWarning("OnReceivedGameErWeiMaMsg -> args was wrong!");
                }
            }
            catch (Exception e)
            {

                SSDebug.LogError(e.Message);
            }
        }

        // Use this for initialization
        //void Start()
        //{
        //MainStart();
        //MsgList = new List<string>();
        //Texture2D tex = new Texture2D(100, 100, TextureFormat.ARGB32, false);
        //if (puser != null)
        //{
        //    tuser = puser.transform;
        //}
        //Vector2d = new Vector2(0.0f, 100.0f);
        //}

        void Awake()
        {
            if (_Instance == null)
            {
                _Instance = this;
                Init();
            }
        }

        void InitInfo()
        {
            SSDebug.Log("FTServerManage::InitInfo.......................");
            if (MsgList == null)
            {
                MsgList = new List<string>();
                MsgListButton = new List<string>();
                MsgListDirection = new List<string>();
            }
        }

        /// <summary>
        /// 循环询问消息池.
        /// </summary>
        void FixedUpdate()
        {
            ShowText();
        }

        /// <summary>
        /// 收到二位码数据.
        /// </summary>
        void OnReceivedErWeiMaData(byte[] buffer)
        {
            if (buffer != null && buffer.Length > 1)
            {
                //将二维码像素列表信息转换为图片.
                Texture2D tx = new Texture2D(100, 100, TextureFormat.ARGB32, false);
                tx.LoadImage(buffer);
                OnErWeiMaLoad(tx);
                buffer = null;

                if (img != null)
                {
                    //二维码测试.
                    img.sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(0, 0));
                }
            }
        }

        int countTest = 0;
        public void ShowText()
        {
            if (MsgList.Count > 0)
            {
                string s = MsgList[0];
                ShowMsg(s);
                MsgList.RemoveAt(0);
            }

            if (MsgListButton.Count > 0)
            {
                string s = MsgListButton[0];
                ShowMsg(s);
                MsgListButton.RemoveAt(0);
            }

            if (MsgListDirection.Count > 0)
            {
                ShowMsg(MsgListDirection[MsgListDirection.Count - 1]);
                //string s = MsgListDirection[0];
                //ShowMsg(s);
                MsgListDirection.RemoveAt(0);
            }
        }

        private void OnGUI()
        {
            if (MsgList == null)
            {
                return;
            }

            GUI.Box(new Rect(15f, 30f, 500f, 25f), "");
            string info = "msgCount: " + MsgList.Count
                + ", msgDirCount: " + MsgListDirection.Count
                + ", msgBtCount: " + MsgListButton.Count
                + ", countTest: " + countTest;
            GUI.Label(new Rect(15f, 30f, 500f, 25f), info);
        }

        #region 服务器消息管理.
        /// <summary>
        /// 纷腾服务器消息接口.
        /// </summary>
        internal FTServerInterface m_FTServerInterface;
        /// <summary>
        /// 创建纷腾服务器消息接口组件.
        /// </summary>
        void CreatFTServerInterface()
        {
            SSDebug.Log("CreatFTServerInterface+++++++++++++++++++++++++++++++++++");
            if (m_FTServerInterface == null)
            {
                GameObject obj = new GameObject("_FTServerInterface");
                m_FTServerInterface = obj.AddComponent<FTServerInterface>();
            }
        }

        /// <summary>
        /// 当二维码加载之后.
        /// </summary>
        void OnErWeiMaLoad(Texture2D val)
        {
            if (m_FTServerInterface != null)
            {
                SSDebug.Log("OnErWeiMaLoad -> loaded erWeiMa..........................");
                m_FTServerInterface.OnErWeiMaLoad(val);
            }
            else
            {
                SSDebug.LogWarning("OnErWeiMaLoad -> m_FTServerInterface was null");
            }
        }
        #endregion
    }
}
