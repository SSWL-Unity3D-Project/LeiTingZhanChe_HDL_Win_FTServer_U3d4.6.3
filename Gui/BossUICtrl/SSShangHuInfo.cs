﻿using System;
using System.IO;
using System.Xml;
using UnityEngine;

/// <summary>
/// 游戏商户配置信息.
/// </summary>
public class SSShangHuInfo : MonoBehaviour
{
    /// <summary>
    /// 大奖Boss商户信息.
    /// </summary>
    [Serializable]
    public class DaJiangBossShangHuData
    {
        /// <summary>
        /// 商户名信息.
        /// 最多5个字.
        /// </summary>
        public string ShangHuMing = "盛世网络";
        /// <summary>
        /// 代金券名称.
        /// </summary>
        public string DaiJinQuanName = "恭喜获得抵扣代金券";
        /// <summary>
        /// 代金券详情信息.
        /// </summary>
        public string XiangQingInfo = "此代金券只能在游戏合作商家内使用。";
        public override string ToString()
        {
            return "ShangHuMing == " + ShangHuMing + ", DaiJinQuanName == " + DaiJinQuanName + ", XiangQingInfo == " + XiangQingInfo;
        }
    }
    /// <summary>
    /// 大奖Boss商户信息.
    /// </summary>
    public DaJiangBossShangHuData[] m_DaJiangBossShangHuDt = new DaJiangBossShangHuData[4];

    /// <summary>
    /// 商户信息.
    /// </summary>
    [Serializable]
    public class ShangHuData
    {
        /// <summary>
        /// 商户名列表信息索引.
        /// </summary>
        internal int IndexShangHu = 0;
        /// <summary>
        /// 商户名信息.
        /// 最多5个字.
        /// </summary>
        public string ShangHuMing = "盛世网络";
        /// <summary>
        /// 商户弹幕文本信息.
        /// 最多9个字.
        /// </summary>
        public string ShangHuDanMuInfo = "盛世网络50元";
        public override string ToString()
        {
            return "IndexShangHu == " + IndexShangHu + ", ShangHuMing == " + ShangHuMing + ", ShangHuDanMuInfo == " + ShangHuDanMuInfo;
        }
        /// <summary>
        /// 代金券名称.
        /// </summary>
        public string DaiJinQuanName = "恭喜获得抵扣代金券";
        /// <summary>
        /// 代金券详情信息.
        /// </summary>
        public string XiangQingInfo = "此代金券只能在游戏合作商家内使用。";
    }
    /// <summary>
    /// 商户配置信息.
    /// 最多4个商户数据信息.
    /// </summary>
    public ShangHuData[] m_ShangHuDt = new ShangHuData[4];

    /// <summary>
    /// 手机端代金券展示信息.
    /// </summary>
    public class DaiJinQuanData
    {
        /// <summary>
        /// 商户名信息.
        /// 最多5个字.
        /// </summary>
        public string ShangHuMing = "盛世网络";
        /// <summary>
        /// 代金券名称.
        /// </summary>
        public string DaiJinQuanName = "恭喜获得抵扣代金券";
        /// <summary>
        /// 代金券详情信息.
        /// </summary>
        public string XiangQingInfo = "此代金券只能在游戏合作商家内使用。";
        public void Reset()
        {
            ShangHuMing = "";
            DaiJinQuanName = "";
            XiangQingInfo = "";
        }
    }
    internal DaiJinQuanData m_DaiJinQuanDt;
    
    /// <summary>
    /// 商户名列表信息索引.
    /// </summary>
    int m_IndexShangHu = 0;

    internal void Init()
    {
        m_DaiJinQuanDt = new DaiJinQuanData();
        m_IndexShangHu = m_ShangHuDt.Length;
        m_IndexJPShangHu = m_DaJiangBossShangHuDt.Length;
        for (int i = 0; i < m_ShangHuDt.Length; i++)
        {
            m_ShangHuDt[i].IndexShangHu = i;
            SSDebug.Log("Init -> ShangHuMing[" + i + "] ===== " + m_ShangHuDt[i].ShangHuMing);
        }
        InitReadConfig();
    }
    
    /// <summary>
    /// 更新游戏大奖Boss商户数据信息.
    /// </summary>
    internal void UpdateDaJiangBossShangHuInfo(string[] shangHuInfoArray)
    {
        if (m_DaJiangBossShangHuDt != null)
        {
            for (int i = 0; i < m_DaJiangBossShangHuDt.Length; i++)
            {
                m_DaJiangBossShangHuDt[i].ShangHuMing = shangHuInfoArray[i];
                SSDebug.Log("UpdateDaJiangBossShangHuInfo -> JPShangHuMing[" + i + "] ===== " + shangHuInfoArray[i]);
            }
        }
    }

    /// <summary>
    /// 更新游戏大奖Boss代金券使用详情数据信息.
    /// </summary>
    internal void UpdateDaJiangBossDaiJinQuanXiangQing(string[] xiangQingInfoArray)
    {
        if (m_DaJiangBossShangHuDt != null)
        {
            for (int i = 0; i < m_DaJiangBossShangHuDt.Length; i++)
            {
                m_DaJiangBossShangHuDt[i].XiangQingInfo = xiangQingInfoArray[i];
                SSDebug.Log("UpdateDaJiangBossDaiJinQuanXiangQing -> JPDescription[" + i + "] ===== " + xiangQingInfoArray[i]);
            }
        }
    }

    /// <summary>
    /// 更新游戏大奖Boss代金券名称数据信息.
    /// </summary>
    internal void UpdateDaJiangBossDaiJinQuanName(string[] infoArray)
    {
        if (m_DaJiangBossShangHuDt != null)
        {
            for (int i = 0; i < m_DaJiangBossShangHuDt.Length; i++)
            {
                m_DaJiangBossShangHuDt[i].DaiJinQuanName = infoArray[i];
                SSDebug.Log("UpdateDaJiangBossDaiJinQuanName -> JPName[" + i + "] ===== " + infoArray[i]);
            }
        }
    }

    /// <summary>
    /// 更新游戏商户数据信息.
    /// </summary>
    internal void UpdateShangHuInfo(string[] shangHuInfoArray)
    {
        for (int i = 0; i < m_ShangHuDt.Length; i++)
        {
            m_ShangHuDt[i].ShangHuMing = shangHuInfoArray[i];
            SSDebug.Log("UpdateShangHuInfo -> ZCShangHuMing[" + i + "] ===== " + shangHuInfoArray[i]);
        }
    }

    /// <summary>
    /// 更新游戏商户战车代金券使用详情数据信息.
    /// </summary>
    internal void UpdateShangHuDaiJinQuanXiangQing(string[] xiangQingInfoArray)
    {
        for (int i = 0; i < m_ShangHuDt.Length; i++)
        {
            m_ShangHuDt[i].XiangQingInfo = xiangQingInfoArray[i];
            SSDebug.Log("UpdateShangHuDaiJinQuanXiangQing -> ZCDescription[" + i + "] ===== " + xiangQingInfoArray[i]);
        }
    }
    
    /// <summary>
    /// 更新游戏商户战车代金券名称数据信息.
    /// </summary>
    internal void UpdateShangHuDaiJinQuanName(string[] infoArray)
    {
        for (int i = 0; i < m_ShangHuDt.Length; i++)
        {
            m_ShangHuDt[i].DaiJinQuanName = infoArray[i];
            SSDebug.Log("UpdateShangHuDaiJinQuanXiangQing -> ZCName[" + i + "] ===== " + infoArray[i]);
        }
    }

    /// <summary>
    /// 更新游戏商户弹幕数据信息.
    /// </summary>
    internal void UpdateShangHuDanMuInfo(string[] shangHuDanMuInfoArray)
    {
        for (int i = 0; i < m_ShangHuDt.Length; i++)
        {
            m_ShangHuDt[i].ShangHuDanMuInfo = shangHuDanMuInfoArray[i];
            SSDebug.Log("UpdateShangHuDanMuInfo -> ShangHuDanMuInfo[" + i + "] ===== " + shangHuDanMuInfoArray[i]);
        }

        if (SSUIRoot.GetInstance().m_GameUIManage != null
            && SSUIRoot.GetInstance().m_GameUIManage.m_DanMuTextUI != null)
        {
            //更新游戏弹幕的商户名信息.
            SSUIRoot.GetInstance().m_GameUIManage.m_DanMuTextUI.UpdateShangJiaDanMuInfo();
        }
    }

    int m_IndexJPShangHu = 0;
    /// <summary>
    /// 获取JPBoss代金券的商户名信息.
    /// </summary>
    internal string GetJPBossShangHuMingInfo()
    {
        m_IndexJPShangHu++;
        if (m_IndexJPShangHu >= m_DaJiangBossShangHuDt.Length)
        {
            m_IndexJPShangHu = 0;
        }
        int indexVal = m_IndexJPShangHu;
        //SSDebug.LogWarning("GetJPBossShangHuMingInfo -> " + m_DaJiangBossShangHuDt[indexVal].ToString());
        m_DaiJinQuanDt.ShangHuMing = m_DaJiangBossShangHuDt[indexVal].ShangHuMing;
        m_DaiJinQuanDt.DaiJinQuanName = m_DaJiangBossShangHuDt[indexVal].DaiJinQuanName;
        m_DaiJinQuanDt.XiangQingInfo = m_DaJiangBossShangHuDt[indexVal].XiangQingInfo;
        return m_DaJiangBossShangHuDt[indexVal].ShangHuMing;
    }

    /// <summary>
    /// 获取JPBoss代金券的商户名信息.
    /// </summary>
    internal string GetJPBossShangHuMingDt()
    {
        int indexVal = m_IndexJPShangHu;
        if (indexVal >= m_DaJiangBossShangHuDt.Length)
        {
            indexVal = 0;
        }
        //SSDebug.LogWarning("GetJPBossShangHuMingDt -> " + m_DaJiangBossShangHuDt[indexVal].ToString());
        return m_DaJiangBossShangHuDt[indexVal].ShangHuMing;
    }

    /// <summary>
    /// 战车
    /// 获取代金券npc的商户名信息.
    /// </summary>
    internal ShangHuData GetShangHuMingInfo()
    {
        m_IndexShangHu++;
        if (m_IndexShangHu >= m_ShangHuDt.Length)
        {
            m_IndexShangHu = 0;
        }
        int indexVal = m_IndexShangHu;
        //SSDebug.LogWarning("GetShangHuMingInfo -> ====================== " + m_ShangHuDt[indexVal].ToString());
        m_DaiJinQuanDt.ShangHuMing = m_ShangHuDt[indexVal].ShangHuMing;
        m_DaiJinQuanDt.DaiJinQuanName = m_ShangHuDt[indexVal].DaiJinQuanName;
        m_DaiJinQuanDt.XiangQingInfo = m_ShangHuDt[indexVal].XiangQingInfo;
        return m_ShangHuDt[indexVal];
    }

    /// <summary>
    /// 战车
    /// 获取代金券npc的商户名信息.
    /// </summary>
    internal ShangHuData GetShangHuMingDt()
    {
        int indexVal = m_IndexShangHu;
        if (indexVal >= m_ShangHuDt.Length)
        {
            indexVal = 0;
        }
        //SSDebug.LogWarning("GetShangHuMingDt -> ============================== " + m_ShangHuDt[indexVal].ToString());
        return m_ShangHuDt[indexVal];
    }

    int m_IndexSuiJiDaoJu = 0;
    /// <summary>
    /// 获取随机道具商户信息.
    /// </summary>
    internal ShangHuData GetSuiJiDaoJuShangHuInfo()
    {
        int indexVal = 0;
        if (SSGameLogoData.m_GameDaiJinQuanMode == SSGameLogoData.GameDaiJinQuanMode.HDL_CaiPinQuan)
        {
            //海底捞菜品券版本,随机道具只送战车的第一种优惠券.
            indexVal = 0;
        }
        else
        {
            m_IndexSuiJiDaoJu++;
            if (m_IndexSuiJiDaoJu >= m_ShangHuDt.Length)
            {
                m_IndexSuiJiDaoJu = 0;
            }
            indexVal = m_IndexSuiJiDaoJu;
            //SSDebug.Log("GetSuiJiDaoJuShangHuInfo -> " + m_ShangHuDt[indexVal].ToString());
            m_DaiJinQuanDt.ShangHuMing = m_ShangHuDt[indexVal].ShangHuMing;
            m_DaiJinQuanDt.DaiJinQuanName = m_ShangHuDt[indexVal].DaiJinQuanName;
            m_DaiJinQuanDt.XiangQingInfo = m_ShangHuDt[indexVal].XiangQingInfo;
        }

        //SSDebug.LogWarning("GetSuiJiDaoJuShangHuInfo -> " + m_ShangHuDt[indexVal].ToString());
        return m_ShangHuDt[indexVal];
    }

    /// <summary>
    /// 获取随机道具商户信息.
    /// </summary>
    internal ShangHuData GetSuiJiDaoJuShangHuDt()
    {
        if (SSGameLogoData.m_GameDaiJinQuanMode == SSGameLogoData.GameDaiJinQuanMode.HDL_CaiPinQuan)
        {
            //海底捞菜品券版本,随机道具只送战车的第一种优惠券.
            return m_ShangHuDt[0];
        }
        else
        {
            int indexVal = m_IndexSuiJiDaoJu;
            if (indexVal >= m_ShangHuDt.Length)
            {
                indexVal = 0;
            }
            return m_ShangHuDt[indexVal];
        }
    }

    #region 从配置文件读取商户信息
    /// <summary>
    /// 商户配置信息.
    /// </summary>
    string ShangHuFileName = "../config/ShangHuConfig.xml";
    /**
     * <!-- DaJiangBoss -> ShangHuMing 该属性用于jpBoss出场时的商户信息            最多5个字 -->
     * <!-- ShangHuData -> ShangHuMing 该属性用于战车出场时的商户信息              最多5个字 -->
     * <!-- ShangHuData -> ShangHuDanMuInfo 该属性用于循环动画时的商户代金券信息   最多9个字 -->
     */
    void InitReadConfig()
    {
        bool isFix = false;
        //读取jpBoss商户名信息.
        string[] daJiangBossShangHuMingArray = ReadArrayFromFileXml(ShangHuFileName, "DaJiangBoss", "ShangHuMing");
        if (daJiangBossShangHuMingArray == null || daJiangBossShangHuMingArray.Length < 4)
        {
            isFix = true;
            //daJiangBossShangHuMingArray = "盛世网络";
            //WriteToFileXml(ShangHuFileName, "DaJiangBoss", "ShangHuMing", daJiangBossShangHuMingArray);
        }
        else if (daJiangBossShangHuMingArray.Length >= 4)
        {
            for (int i = 0; i < daJiangBossShangHuMingArray.Length; i++)
            {
                if (daJiangBossShangHuMingArray[i] == "")
                {
                    isFix = true;
                }
            }
        }

        if (isFix == true)
        {
            //初始化信息.
            daJiangBossShangHuMingArray = new string[4] { "盛世网络", "陕西纷腾", "西安纷腾", "三角犀" };
            WriteToFileXml(ShangHuFileName, "DaJiangBoss", "ShangHuMing", daJiangBossShangHuMingArray);
        }

        //商户名信息.
        string[] shangHuMingArray = ReadArrayFromFileXml(ShangHuFileName, "ShangHuData", "ShangHuMing");
        if (shangHuMingArray == null || shangHuMingArray.Length < 4)
        {
            isFix = true;
        }
        else if (shangHuMingArray.Length >= 4)
        {
            for (int i = 0; i < shangHuMingArray.Length; i++)
            {
                if (shangHuMingArray[i] == "")
                {
                    isFix = true;
                }
            }
        }

        if (isFix == true)
        {
            //初始化信息.
            shangHuMingArray = new string[4] { "盛世网络", "陕西纷腾", "西安纷腾", "三角犀" };
            WriteToFileXml(ShangHuFileName, "ShangHuData", "ShangHuMing", shangHuMingArray);
        }

        //商户代金券信息,在游戏循环动画弹幕中展示.
        isFix = false;
        string[] shangHuDaiJinQuanArray = ReadArrayFromFileXml(ShangHuFileName, "ShangHuData", "ShangHuDanMuInfo");
        if (shangHuDaiJinQuanArray == null || shangHuDaiJinQuanArray.Length < 4)
        {
            isFix = true;
        }
        else if (shangHuDaiJinQuanArray.Length >= 4)
        {
            for (int i = 0; i < shangHuDaiJinQuanArray.Length; i++)
            {
                if (shangHuDaiJinQuanArray[i] == "")
                {
                    isFix = true;
                }
            }
        }

        if (isFix == true)
        {
            //初始化信息.
            shangHuDaiJinQuanArray = new string[4] { "盛世网络50元", "陕西纷腾50元", "西安纷腾50元", "三角犀50元" };
            WriteToFileXml(ShangHuFileName, "ShangHuData", "ShangHuDanMuInfo", shangHuDaiJinQuanArray);
        }
        
        UpdateDaJiangBossShangHuInfo(daJiangBossShangHuMingArray);
        UpdateShangHuInfo(shangHuMingArray);
        UpdateShangHuDanMuInfo(shangHuDaiJinQuanArray);
    }

    /// <summary>
    /// 创建商户数据信息.
    /// </summary>
    void CreatShangHuData(string filepath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("ConfigShangHuData");
        for (int i = 0; i < 4; i++)
        {
            //JP大奖商户信息.
            XmlElement elmNewJPDaJiang = xmlDoc.CreateElement("DaJiangBoss");
            root.AppendChild(elmNewJPDaJiang);
        }

        for (int i = 0; i < 4; i++)
        {
            //游戏弹幕中商户信息和游戏里战车代金券信息.
            XmlElement elmNew = xmlDoc.CreateElement("ShangHuData");
            root.AppendChild(elmNew);
        }
        xmlDoc.AppendChild(root);
        xmlDoc.Save(filepath);
        File.SetAttributes(filepath, FileAttributes.Normal);
    }

    /// <summary>
    /// 写入单条数据.
    /// </summary>
    void WriteToFileXml(string fileName, string elementName, string attribute, string valueStr)
    {
        string filepath = Application.dataPath + "/" + fileName;
#if UNITY_ANDROID
		filepath = Application.persistentDataPath + "//" + fileName;
#endif

        //create file
        if (!File.Exists(filepath))
        {
            CreatShangHuData(filepath);
        }

        //update value
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("ConfigShangHuData").ChildNodes;

            foreach (XmlElement xe in nodeList)
            {
                if (xe.Name == elementName)
                {
                    xe.SetAttribute(attribute, valueStr);
                    break;
                }
            }
            File.SetAttributes(filepath, FileAttributes.Normal);
            xmlDoc.Save(filepath);
        }
    }

    /// <summary>
    /// 写入多条数据.
    /// </summary>
    void WriteToFileXml(string fileName, string elementName, string attribute, string[] valueStr)
    {
        string filepath = Application.dataPath + "/" + fileName;
#if UNITY_ANDROID
		filepath = Application.persistentDataPath + "//" + fileName;
#endif

        //create file
        if (!File.Exists(filepath))
        {
            CreatShangHuData(filepath);
        }

        //update value
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("ConfigShangHuData").ChildNodes;
            int countNum = 0;
            foreach (XmlElement xe in nodeList)
            {
                if (xe.Name == elementName)//"ShangHuData")
                {
                    xe.SetAttribute(attribute, valueStr[countNum]);
                    countNum++;
                }
            }
            File.SetAttributes(filepath, FileAttributes.Normal);
            xmlDoc.Save(filepath);
        }
    }

    /// <summary>
    /// 读取单条数据.
    /// </summary>
    string ReadFromFileXml(string fileName, string elementName, string attribute)
    {
        string filepath = Application.dataPath + "/" + fileName;
#if UNITY_ANDROID
		//filepath = Application.persistentDataPath + "//" + fileName;
#endif
        string valueStr = null;
        if (File.Exists(filepath))
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode("ConfigShangHuData").ChildNodes;
                foreach (XmlElement xe in nodeList)
                {
                    if (xe.Name == elementName)
                    {
                        valueStr = xe.GetAttribute(attribute);
                        break;
                    }
                }
                File.SetAttributes(filepath, FileAttributes.Normal);
                xmlDoc.Save(filepath);
            }
            catch (Exception exception)
            {
                File.SetAttributes(filepath, FileAttributes.Normal);
                File.Delete(filepath);
                SSDebug.LogError("error: xml was wrong! " + exception);
            }
        }
        return valueStr;
    }

    /// <summary>
    /// 读取多条数据.
    /// </summary>
    string[] ReadArrayFromFileXml(string fileName, string elementName, string attribute)
    {
        string filepath = Application.dataPath + "/" + fileName;
#if UNITY_ANDROID
		//filepath = Application.persistentDataPath + "//" + fileName;
#endif
        string[] valueStr = null;
        if (File.Exists(filepath))
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode("ConfigShangHuData").ChildNodes;
                valueStr = new string[nodeList.Count];
                int countNum = 0;
                foreach (XmlElement xe in nodeList)
                {
                    if (xe.Name == elementName)
                    {
                        valueStr[countNum] = xe.GetAttribute(attribute);
                        countNum++;
                    }
                }
                File.SetAttributes(filepath, FileAttributes.Normal);
                xmlDoc.Save(filepath);
            }
            catch (Exception exception)
            {
                File.SetAttributes(filepath, FileAttributes.Normal);
                File.Delete(filepath);
                SSDebug.LogError("error: xml was wrong! " + exception);
            }
        }
        return valueStr;
    }
    #endregion
}
