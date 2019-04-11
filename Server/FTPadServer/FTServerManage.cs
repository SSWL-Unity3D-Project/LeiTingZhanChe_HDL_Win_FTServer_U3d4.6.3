using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GClientLib;

namespace Server.FTPadServer
{
    public class FTServerManage : MonoBehaviour
    {
        public UnityEngine.UI.Image img;
        public Text ptext;
        public List<string> MsgList;
        public UnityEngine.UI.Image puser;
        SocketLib dll_MainLib;
        Texture2D tex;
        byte[] buffer = new byte[4 * 1024 * 1024];
        Sprite temp;
        Transform tuser;
        private string nSessionGuid;
        Vector2 Vector2d;
        public Canvas maincanvas;
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



        public delegate void MyDelegate();
        public MyDelegate _MsgBox;
        public void MainStart()
        {
            dll_MainLib = new SocketLib();
            //dll_MainLib.Start();
            dll_MainLib._GCSysMessage += A_ShowMessage;
            dll_MainLib._GCSocketClosed += A_GCSocketClosed;
            dll_MainLib._GCSocketConnected += A_GCSocketConnected;
            dll_MainLib._GCSocketError += A_GCSocketError;
            dll_MainLib._GCSocketReceived += A_GCSocketReceived;
            //dll_MainLib.GC_StartUp("123.59.41.81", 10011);
            dll_MainLib.GC_StartUp("123.59.41.81", 10011);
            _MsgBox += ShowText;
            /**/
            return;
        }
        public void RegLocal()
        {
            dll_MainLib.GC_SendCommand("REG", dll_MainLib.GetSystemInfoString());
            Debug.Log("REG:" + dll_MainLib.GetSystemInfoString());

        }
        private void A_GCSocketReceived(string sArguement)
        {
            sArguement = sArguement.Replace("\r\n", "");
            A_ShowMessage(sArguement);
            Debug.Log(System.DateTime.Now.ToString() + ":" + sArguement + "\n");
            string s = sArguement;
            string[] sagr;
            sagr = s.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "Q2CODE":
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

        private void A_GCSocketError(string sArg)
        {
            A_ShowMessage("Dll网络连接出错");
            A_ShowMessage(sArg);
        }

        private void A_GCSocketConnected()
        {
            A_ShowMessage("Dll网络连接成功");
        }

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
            ptext.text = System.DateTime.Now.ToString() + ":" + s + "\n";
            //this.Invoke("ShowText",0.01f);
        }

        // Use this for initialization
        void Start()
        {
            //MainStart();
            MsgList = new List<string>();
            tex = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            tuser = puser.transform;
            Vector2d = new Vector2(0.0f, 100.0f);
        }

        // Update is called once per frame
        void Update()
        {
            ShowText();
            if (buffer.Length > 1)
            {
                tex.LoadImage(buffer);
                temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
                img.sprite = temp;
                buffer.Initialize();
            }
            tuser.localPosition = Vector2d;
            //tuser.Translate(Vector2d.x*Time.deltaTime, Vector2d.y * Time.deltaTime, 0);
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
        public void LateUpdate()
        {

        }
    }
}
