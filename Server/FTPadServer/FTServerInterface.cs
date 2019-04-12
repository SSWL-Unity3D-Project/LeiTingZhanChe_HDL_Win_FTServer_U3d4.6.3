using UnityEngine;

namespace Server.FTPadServer
{
    /// <summary>
    /// 纷腾服务器消息接口.
    /// </summary>
    public class FTServerInterface : MonoBehaviour
    {
        #region 服务器二维码加载事件.
        /// <summary>
        /// 二维码加载事件.
        /// </summary>
        public delegate void EventErWeiMaLoad(Texture2D val);
        public event EventErWeiMaLoad OnEventErWeiMaLoad;
        /// <summary>
        /// 当二维码被加载之后.
        /// </summary>
        public void OnErWeiMaLoad(Texture2D val)
        {
            if (OnEventErWeiMaLoad != null)
            {
                OnEventErWeiMaLoad(val);
            }
        }
        #endregion
    }
}
