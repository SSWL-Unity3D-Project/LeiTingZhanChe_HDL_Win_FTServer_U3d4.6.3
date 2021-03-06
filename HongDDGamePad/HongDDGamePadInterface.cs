﻿using UnityEngine;

namespace Assets.XKGame.Script.HongDDGamePad
{
    /// <summary>
    /// 红点点微信虚拟游戏手柄控制接口.
    /// </summary>
    public class HongDDGamePadInterface : MonoBehaviour
    {
        #region 红点点微信虚拟游戏手柄控制接口
        /// <summary>
        /// 红点点微信手柄消息处理组件.
        /// </summary>
        internal HongDDGamePad m_HongDDGamePadCom;

        /// <summary>
        /// 创建红点点微信虚拟游戏手柄组件.
        /// </summary>
        internal void CreatHongDDGanePad()
        {
            if (pcvr.IsXuNiPhoneShouBing == true)
            {
                m_HongDDGamePadCom = gameObject.AddComponent<HongDDGamePad>();
                if (m_HongDDGamePadCom != null)
                {
                    m_HongDDGamePadCom.Init();
                }
            }
        }

        /// <summary>
        /// 获取微信小程序虚拟手柄类型.
        /// </summary>
        internal SSBoxPostNet.WeiXinShouBingEnum GetWXShouBingType()
        {
            if (m_HongDDGamePadCom == null)
            {
                return SSBoxPostNet.WeiXinShouBingEnum.XiaoChengXu;
            }
            return m_HongDDGamePadCom.m_WXShouBingType;
        }

        /// <summary>
        /// 获取二维码生成脚本组件.
        /// </summary>
        internal BarcodeCam GetBarcodeCam()
        {
            if (m_HongDDGamePadCom == null)
            {
                return null;
            }
            return m_HongDDGamePadCom.m_BarcodeCam;
        }

        /// <summary>
        /// 获取BoxPostNet控制组件.
        /// </summary>
        internal SSBoxPostNet GetBoxPostNet()
        {
            if (m_HongDDGamePadCom == null)
            {
                return null;
            }
            return m_HongDDGamePadCom.m_SSBoxPostNet;
        }

        /// <summary>
        /// 获取红点点微信虚拟游戏手柄微信支付控制组件.
        /// </summary>
        internal HongDDGamePadWXPay GetHongDDGamePadWXPay()
        {
            if (m_HongDDGamePadCom == null)
            {
                return null;
            }
            return m_HongDDGamePadCom.m_HongDDGamePadWXPay;
        }

        /// <summary>
        /// 添加遥控器按键信息响应事件.
        /// </summary>
        internal void AddTVYaoKongBtEvent()
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.AddTVYaoKongBtEvent();
            }
        }

        /// <summary>
        /// 当展示玩家评级UI时进入此函数.
        /// 此时需要对微信付费玩家进行红点点账户扣费.
        /// </summary>
        internal void OnNeedSubPlayerMoney(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnNeedSubPlayerMoney(indexPlayer);
            }
        }

        /// <summary>
        /// 获取玩家是否为付费激活游戏用户.
        /// </summary>
        internal bool GetPlayerIsFuFeiActiveGame(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                return m_HongDDGamePadCom.GetPlayerIsFuFeiActiveGame(indexPlayer);
            }
            return false;
        }

        /// <summary>
        /// 设置玩家结束游戏时间.
        /// </summary>
        internal void SetPlayerEndGameTime(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                //设置玩家结束游戏的时间.
                m_HongDDGamePadCom.SetPlayerEndGameTime(indexPlayer);
            }
        }

        /// <summary>
        /// 设置玩家激活状态信息.
        /// </summary>
        internal void SetIndexPlayerActiveGameState(int index, byte activeState)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SetIndexPlayerActiveGameState(index, activeState);
            }
        }

        /// <summary>
        /// 删除微信手柄玩家的数据信息.
        /// </summary>
        internal void RemoveWeiXinPadPlayerData(int userId)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.RemoveWeiXinPadPlayerData(userId);
            }
        }
        
        /// <summary>
        /// 当玩家游戏倒计时结束,清理玩家的游戏微信数据.
        /// 主要目的是想让新来的玩家可以立马进入游戏.
        /// </summary>
        internal void OnPlayerGameDaoJiShiOver(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnPlayerGameDaoJiShiOver(indexPlayer);
            }
        }

        /// <summary>
        /// 清理玩家微信数据信息.
        /// </summary>
        internal void ClearGameWeiXinData()
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.ClearGameWeiXinData();
            }
        }
        
        /// <summary>
        /// 玩家获得免费再玩一局游戏奖品之后,使玩家免费再玩一局游戏.
        /// </summary>
        internal void MakePlayerMianFeiZaiWanYiJu(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.MakePlayerMianFeiZaiWanYiJu(indexPlayer);
            }
        }

        /// <summary>
        /// 发送玩家首次免费游戏登录信息给服务器.
        /// </summary>
        internal void SendPlayerShouCiMianFeiInfoToServer(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendPlayerShouCiMianFeiInfoToServer(indexPlayer);
            }
        }

        /// <summary>
        /// 发送玩家付费激活游戏登录信息给服务器.
        /// </summary>
        internal void SendPlayerFuFeiActiveGameInfoToServer(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendPlayerFuFeiActiveGameInfoToServer(indexPlayer);
            }
        }

        /// <summary>
        /// 添加微信玩家游戏币.
        /// </summary>
        //internal void AddWeiXinGameCoinToPlayer(int userId, int coin)
        //{
        //    if (m_HongDDGamePadCom != null)
        //    {
        //        m_HongDDGamePadCom.AddWeiXinGameCoinToPlayer(userId, coin);
        //    }
        //}

        /// <summary>
        /// 获取玩家微信头像url.
        /// </summary>
        internal string GetPlayerHeadUrl(int index)
        {
            string url = "";
            if (m_HongDDGamePadCom != null)
            {
                if (index >= 0 && index < m_HongDDGamePadCom.m_PlayerHeadUrl.Length)
                {
                    url = m_HongDDGamePadCom.m_PlayerHeadUrl[index];
                }
            }
            return url;
        }
        
        /// <summary>
        /// 发送玩家获取商家代金券的信息给服务器.
        /// indexPlayer玩家索引.
        /// money代金券金额(元).
        /// </summary>
        internal void SendPostHddPlayerCouponInfo(PlayerEnum indexPlayer, int money, SSCaiPiaoDataManage.GameCaiPiaoData.DaiJinQuanState daiJinQuanType)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendPostHddPlayerCouponInfo(indexPlayer, money, daiJinQuanType);
            }
        }

        /// <summary>
        /// 通过抽奖获取的代金券.
        /// 发送玩家获取商家代金券的信息给服务器.
        /// indexPlayer玩家索引.
        /// money代金券金额(元).
        /// </summary>
        internal void SendPostHddPlayerCouponInfoByChouJiang(PlayerEnum indexPlayer, int money, SSCaiPiaoDataManage.GameCaiPiaoData.DaiJinQuanState daiJinQuanType)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendPostHddPlayerCouponInfoByChouJiang(indexPlayer, money, daiJinQuanType);
            }
        }

        /// <summary>
        /// 获取玩家微信数据信息.
        /// </summary>
        public HongDDGamePad.GamePlayerData GetGamePlayerData(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                return m_HongDDGamePadCom.FindGamePlayerData(indexPlayer);
            }
            return null;
        }
        
        /// <summary>
        /// 更新免费试玩信息.
        /// </summary>
        internal void UpdateMianFeiCountInfo(int args)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.UpdateMianFeiCountInfo(args);
            }
        }

        /// <summary>
        /// 更新游戏一币等于多少人民币的信息.
        /// </summary>
        internal void UpdateGameCoinToMoney(int args)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.UpdateGameCoinToMoney(args);
            }
        }
        
        /// <summary>
        /// 关闭WebSocket
        /// </summary>
        internal void CloseWebSocket()
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.CloseWebSocket();
            }
        }

        /// <summary>
        /// 获取游戏在红点点平台的屏幕码信息.
        /// </summary>
        internal void GetGameHddScreenNum()
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.GetGameHddScreenNum();
            }
        }

        /// <summary>
        /// 当心跳消息检测超时来自网络故障UI提示.
        /// </summary>
        internal void OnXiTiaoMsgTimeOutFromWangLuoGuZhang()
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnXiTiaoMsgTimeOutFromWangLuoGuZhang();
            }
        }

        /// <summary>
        /// 删除玩家微信数据信息.
        /// </summary>
        internal void RemoveGamePlayerData(PlayerEnum indexPlayer)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.RemoveGamePlayerData(indexPlayer);
            }
        }
        
        /// <summary>
        /// 发送红点点微信游戏手柄展示防沉迷的消息.
        /// </summary>
        internal void SendWXPadShowFangChenMiPanel(PlayerEnum playerIndex)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendWXPadShowFangChenMiPanel(playerIndex);
            }
        }

        /// <summary>
        /// 发送显示微信游戏手柄抽奖ui.
        /// </summary>
        internal void SendWXPadShowChouJiangUI(PlayerEnum playerIndex)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendWXPadShowChouJiangUI(playerIndex);
            }
        }

        /// <summary>
        /// 发送隐藏微信游戏手柄抽奖ui.
        /// </summary>
        internal void SendWXPadHiddenChouJiangUI(PlayerEnum playerIndex)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.SendWXPadHiddenChouJiangUI(playerIndex);
            }
        }
        #endregion

        #region 纷腾手机虚拟游戏手柄接口管理
        /// <summary>
        /// 玩家从纷腾服务器登陆游戏.
        /// </summary>
        internal void OnPlayerLoginFromFTServer(WebSocketSimpet.PlayerWeiXinData playerDt)
        {
            if (playerDt == null)
            {
                SSDebug.LogWarning("OnPlayerLoginFromFTServer -> playerDt was null");
                return;
            }

            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnPlayerLoginFromFTServer(playerDt);
            }
        }

        /// <summary>
        /// 玩家从纷腾服务器退出游戏.
        /// </summary>
        internal void OnPlayerExitFromFTServer(int userId)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnPlayerExitFromFTServer(userId);
            }
        }

        /// <summary>
        /// 收到玩家操作手柄的方向信息从纷腾服务器.
        /// </summary>
        internal void OnReceiveDirectionAngleMsgFTServer(string val, int userId)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnReceiveDirectionAngleMsgFTServer(val, userId);
            }
        }

        /// <summary>
        /// 收到玩家操作手柄的按键信息从纷腾服务器.
        /// </summary>
        internal void OnReceiveActionOperationMsgFTServer(string val, int userId)
        {
            if (m_HongDDGamePadCom != null)
            {
                m_HongDDGamePadCom.OnReceiveActionOperationMsgFTServer(val, userId);
            }
        }
        #endregion
    }
}
