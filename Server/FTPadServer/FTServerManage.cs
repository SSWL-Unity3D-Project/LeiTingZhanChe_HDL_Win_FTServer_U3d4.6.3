using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GClientLib;
using System.Collections;

namespace Server.FTPadServer
{
    /// <summary>
    /// 纷腾服务器管理组件.
    /// </summary>
    public class FTServerManage : MonoBehaviour
    {
        public UnityEngine.UI.Image img;
        public Text ptext;
        internal List<string> MsgList;
        public UnityEngine.UI.Image puser;
        SocketLib dll_MainLib;
        byte[] buffer;
        Sprite temp;
        Transform tuser;
        private string nSessionGuid;
        Vector2 Vector2d;
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

        /// <summary>
        /// 初始化.
        /// </summary>
        void Init()
        {
            CreatFTServerInterface();
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

            string systemInfo = "";
            systemInfo = dll_MainLib.GetSystemInfoString();
            dll_MainLib.GC_SendCommand("REG", systemInfo);
            Debug.Log("REG:" + systemInfo);
        }

        /// <summary>
        /// 服务器返回的消息.
        /// </summary>
        private void A_GCSocketReceived(string sArguement)
        {
            sArguement = sArguement.Replace("\r\n", "");
            A_ShowMessage(sArguement);
            //Debug.Log(System.DateTime.Now.ToString() + ":" + sArguement + "\n");

            string s = sArguement;
            string[] sagr;
            sagr = s.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "Q2CODE":
                        //二维码信息返回.
                        try
                        {
                            buffer = dll_MainLib.GC_GetQRCodeBitmap(sagr[1], 100, 100);
                        }
                        catch (Exception e)
                        {

                            Debug.LogError(e.Message);
                        }
                        break;
                    case "REGOK":
                        //注册成功.
                        if (sagr.Length >= 1)
                        {
                            SessionID = sagr[1];
                        }
                        if (sagr.Length >= 2)
                        {
                            GlobalNum = sagr[2];
                        }
                        dll_MainLib.GC_SendCommand("Q2CODE", GlobalNum);
                        break;
                    case "DATA":
                        //手机游戏手柄数据信息.
                        //if (Vector2d.x<= Screen.width)
                        {
                            Vector2d.x = float.Parse(sagr[3]) / 1.0f;
                        }
                        //if (Vector2d.y <= Screen.height)
                        {
                            Vector2d.y = float.Parse(sagr[4]) / -1.0f;
                        }
                        if (sagr[3] == "0" && sagr[4] == "0")
                        {
                            //Vector2d.x = 0.0f;
                            //Vector2d.y = 100.0f;
                        }
                        break;
                    case "TEST":
                        if (sagr.Length >= 4)
                        {
                            dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[3]);
                        }
                        else
                        {
                            dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[sagr.Length - 1]);
                        }
                        break;

                    default:
                        break;
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

        private void A_ShowMessage(string s)
        {
            MsgList.Add(s);
            //ShowMsg(s);
        }

        public void ShowMsg(string s)
        {
            SSDebug.Log("ShowMsg -> msg == " + s);
            if (ptext != null)
            {
                ptext.text = System.DateTime.Now.ToString() + ":" + s + "\n";
            }
            //this.Invoke("ShowText",0.01f);
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
            }
        }

        // Update is called once per frame
        void Update()
        {
            ShowText();
            if (buffer != null && buffer.Length > 1)
            {
                //将二维码像素列表信息转换为图片.
                Texture2D tx = new Texture2D(100, 100, TextureFormat.ARGB32, false);
                tx.LoadImage(buffer);
                OnErWeiMaLoad(tx);
                buffer = null;

                if (img != null)
                {
                    temp = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(0, 0));
                    img.sprite = temp;
                }
            }

            if (tuser != null)
            {
                tuser.localPosition = Vector2d;
            }
        }

        public void ShowText()
        {
            string s;
            if (MsgList.Count > 0)
            {
                s = MsgList[0];
                ShowMsg(s);
                MsgList.RemoveAt(0);
            }
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
