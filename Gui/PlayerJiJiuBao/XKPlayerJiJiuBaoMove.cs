﻿using UnityEngine;

public class XKPlayerJiJiuBaoMove : MonoBehaviour
{
	[Range(0.1f, 10f)]public float PiaoZiTime = 0.5f;
	[Range(10f, 500f)]public float PiaoZiPY = 100f;
	public Vector2 LocalScale = new Vector2(1f, 1f);
	public void SetPlayerJiJiuBaoVal(Vector3 startPos)
	{
		startPos.y += XKDaoJuGlobalDt.GetInstance().DaoJuMaoZiPY;
		transform.localPosition = startPos;
		transform.localEulerAngles = Vector3.zero;
		transform.localScale = new Vector3(LocalScale.x, LocalScale.y, 1f);
		gameObject.SetActive(true);
		TweenPosition twPos = gameObject.AddComponent<TweenPosition>();
		twPos.from = startPos;
		twPos.to = startPos + new Vector3(0f, PiaoZiPY, 0f);
		twPos.duration = PiaoZiTime;
		twPos.PlayForward();

        EventDelegate.Add(twPos.onFinished, delegate {
            StartCoroutine(StartPlayTweenAlpha());
        });
        m_TimeLast = Time.time;
    }

    /// <summary>
    /// 开始播放淡化效果.
    /// </summary>
    System.Collections.IEnumerator StartPlayTweenAlpha()
    {
        yield return new WaitForSeconds(1f);
        TweenAlpha twAlp = gameObject.AddComponent<TweenAlpha>();
        twAlp.from = 1f;
        twAlp.to = 0f;
        twAlp.duration = 1f;
        twAlp.PlayForward();
    }
        
    void Update()
    {
        UpdateRemoveSelf();
    }

    bool IsRemoveSelf = false;
    float m_TimeLast = 0f;
    void UpdateRemoveSelf()
    {
        if (IsRemoveSelf == false)
        {
            if (Time.time - m_TimeLast >= 2f + PiaoZiTime)
            {
                m_TimeLast = Time.time;
                RemoveSelf();
            }
        }
    }

    internal void RemoveSelf()
    {
        if (IsRemoveSelf == false)
        {
            IsRemoveSelf = true;
            Destroy(gameObject);
        }
    }
}