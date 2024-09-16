﻿using UnityEngine;
using UnityEngine.Events;   // 引用 事件 API

public class PlayerProp : MonoBehaviour
{
    #region 欄位
    [Header("取得道具")]
    public UnityEvent onGetProp;            // 事件
    #endregion

    #region 事件
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "道具")
        {
            Destroy(other.gameObject);
            onGetProp.Invoke();             // 呼叫事件
        }
    }
    #endregion
}
