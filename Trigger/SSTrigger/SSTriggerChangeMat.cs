﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 改变游戏材质球触发器.
/// </summary>
using System.Collections;


public class SSTriggerChangeMat : MonoBehaviour
{
    [System.Serializable]
    public class MaterialData
    {
        /// <summary>
        /// 渲染器.
        /// </summary>
        public MeshRenderer MeshRender;
        /// <summary>
        /// 原材质球.
        /// </summary>
        public Material[] Materials;
        /// <summary>
        /// 透明材质球.
        /// </summary>
        public Material[] TouMingMaterials;

        internal void UpdateMaterial(MaterialState type)
        {
            if (MeshRender == null)
            {
                return;
            }

            Material[] mats = null;
            switch (type)
            {
                case MaterialState.Normal:
                    {
                        mats = Materials;
                        break;
                    }

                case MaterialState.TouMing:
                    {
                        mats = TouMingMaterials;
                        break;
                    }
            }

            if (mats != null)
            {
                MeshRender.materials = mats;
            }
        }
    }
    /// <summary>
    /// 更换材质球的数据信息.
    /// </summary>
    public MaterialData[] m_MaterrialDt;

    public enum MaterialState
    {
        /// <summary>
        /// 正常.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 透明.
        /// </summary>
        TouMing = 1,
    }

	float m_TimeLast = 0f;
	MaterialState m_MatChangeState = MaterialState.Normal;
	/// <summary>
    /// 更新材质球.
    /// </summary>
    void UpdateMaterialState(MaterialState type)
	{
		if (m_MatChangeState == type)
		{
			return;
		}
		m_MatChangeState = type;
		
		if (Time.time - m_TimeLast < 0.8f)
		{
			StartCoroutine(DelayChangeMaterialState(type));
			return;
		}
		m_TimeLast = Time.time;
		ChangeMaterialState(type);
    }

	void ChangeMaterialState(MaterialState type)
	{
		//SSDebug.Log("ChangeMaterialState -> type ================ " + type);
		for (int i = 0; i < m_MaterrialDt.Length; i++)
		{
			if (m_MaterrialDt[i] != null)
			{
				m_MaterrialDt[i].UpdateMaterial(type);
			}
		}
	}
	
	IEnumerator DelayChangeMaterialState(MaterialState type)
	{
		yield return new WaitForSeconds(0.8f);
		if (m_MatChangeState == type)
		{
			ChangeMaterialState(type);
		}
	}

    /// <summary>
    /// 进入触发器的次数信息.
    /// </summary>
    //int m_EnterCount = 0;
    /// <summary>
    /// 进入触发器的玩家信息.
    /// </summary>
    List<PlayerEnum> m_PlayerEnumList = null;
    internal void SubEnterCount(PlayerEnum indexPlayer)
    {
        if (m_PlayerEnumList == null)
        {
            return;
        }

        if (m_PlayerEnumList.Contains(indexPlayer) == false)
        {
            return;
        }
        m_PlayerEnumList.Remove(indexPlayer);

        if (m_PlayerEnumList.Count <= 0)
            {
            if (m_PlayerEnumList != null)
            {
                m_PlayerEnumList.Clear();
                m_PlayerEnumList = null;
            }

            //更换材质球为正常的.
            UpdateMaterialState(MaterialState.Normal);
            if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_TriggerManage != null)
            {
                //清理信息.
                XkGameCtrl.GetInstance().m_TriggerManage.SetTriggerChangeMat(null);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        XKPlayerMoveCtrl playerMoveCom = other.GetComponent<XKPlayerMoveCtrl>();
        if (playerMoveCom == null)
        {
            return;
        }
        //SSDebug.Log("SSTriggerChangeMat::OnTriggerEnter -> playerMoveCom.playerIndex ==== " + playerMoveCom.PlayerIndex);

        if (m_PlayerEnumList == null)
        {
            if (XkGameCtrl.GetInstance() != null && XkGameCtrl.GetInstance().m_TriggerManage != null)
            {
                //设置信息.
                XkGameCtrl.GetInstance().m_TriggerManage.SetTriggerChangeMat(this);
            }
            m_PlayerEnumList = new List<PlayerEnum>();
        }

        if (m_PlayerEnumList != null && m_PlayerEnumList.Contains(playerMoveCom.PlayerIndex) == false)
        {
            m_PlayerEnumList.Add(playerMoveCom.PlayerIndex);
            //更换材质球为透明的.
            UpdateMaterialState(MaterialState.TouMing);
        }
    }

    void OnTriggerExit(Collider other)
    {
        XKPlayerMoveCtrl playerMoveCom = other.GetComponent<XKPlayerMoveCtrl>();
        if (playerMoveCom == null)
        {
            return;
        }
        //SSDebug.Log("SSTriggerChangeMat::OnTriggerExit -> playerMoveCom.playerIndex ==== " + playerMoveCom.PlayerIndex);
        SubEnterCount(playerMoveCom.PlayerIndex);
    }
}
