using UnityEngine;
using UnityEngine.AI;   // 引用 人工智慧 API
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("基本欄位")]
    public float attack = 20;
    public float hp = 250;
    [Range(0f, 100f)]
    public float speed = 1.5f;
    [Range(0f, 100f), Tooltip("攻擊距離")]
    public float distanceAttack = 5f;
    [Range(0f, 100f), Tooltip("追蹤距離")]
    public float distanceTrack = 25f;
    [Range(2.5f, 5f), Tooltip("每次攻擊冷卻時間")]
    public float cd = 3.5f;
    [Range(2.5f, 5f), Tooltip("攻擊範圍")]
    public float rangeAttack = 3f;
    [Range(0, 5), Tooltip("攻擊延遲判定")]
    public float delayAttack = 1.2f;

    private float timer;
    private Transform target;   // 目標物件
    private Animator ani;       // 動畫元件
    private NavMeshAgent agent; // 導覽代理器元件
    #endregion

    public Renderer[] smr;

    #region 事件
    private void Start()
    {
        
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        target = GameObject.Find("玩家").transform;
    }

    private void Update()
    {
        Track();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);                // 圖示.顏色
        Gizmos.DrawSphere(transform.position, distanceAttack);  // 圖示.繪製球體(中心點，半徑)

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack);  // forward : 前方 Z、right : 右方 X、up : 上方 Y
    }
    #endregion

    #region 方法
    /// <summary>
    /// 追蹤：用距離判斷玩家位置並切換狀態 - 攻擊、追蹤與等待
    /// </summary>
    private void Track()
    {
        float dis = Vector3.Distance(target.position, transform.position);  // 距離 = 三維向量.距離(A 點，B 點)

        if (dis <= distanceAttack)                                          // 距離 <= 攻擊距離 進行攻擊
        {
            Attack();
        }
        else if (dis <= distanceTrack)                                      // 距離 <= 追蹤距離 進行追蹤
        {
            ani.SetBool("走路開關", !(agent.isStopped = false));             // 是否停止 = 否 - 動畫狀態 = 顛倒(是否停止)
            agent.SetDestination(target.position);                          // 代理器.設定目的地(三維向量)
        }
        else
        {
            Idle();
        }
    }

    /// <summary>
    /// 等待狀態：等待動畫與停止代理器
    /// </summary>
    private void Idle()
    {
        ani.SetBool("走路開關", !(agent.isStopped = true));                 // 是否停止 = 否 - 動畫狀態 = 顛倒(是否停止)
    }

    /// <summary>
    /// 攻擊狀態：啟動計時器、判斷時間並播放攻擊動畫
    /// </summary>
    private void Attack()
    {
        if (timer >= cd)                                                    // 如果 計時器 >= 冷卻時間
        {
            timer = 0;                                                      // 歸零重新計算時間
            agent.isStopped = true;                                         // 停止代理器避免滑行
            ani.SetTrigger("攻擊觸發");                                      // 攻擊動畫
            Invoke("DelayAttack", delayAttack);                             // 延遲調用("方法名稱"，延遲時間)
        }
        else
        {
            timer += Time.deltaTime;                                        // 否則 計時器 < 冷卻時間，累加時間
            Idle();                                                         // 等待
        }
    }

    /// <summary>
    /// 延遲給予玩家傷害值，避免怪物剛舉手玩家就受傷
    /// </summary>
    private void DelayAttack()
    {
        RaycastHit hit; // 射線碰撞資訊
        // 物理.射線碰撞(起點，方向，射線碰撞資訊，長度)
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rangeAttack))
        {
            if (hit.collider.gameObject.name == "守衛")
            {
                hit.collider.GetComponent<PlayerController>().Hit(attack);
            }
        }
    }

    public void Hit(float damage)
    {
        ani.SetTrigger("受傷觸發");
        hp -= damage;
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        ani.SetBool("死亡開關", true);
        StartCoroutine(DeadEffect());
    }

    private IEnumerator DeadEffect()
    {
        float da = smr[0].material.GetFloat("_DissolveAmount");

        while (da < 1)
        {
            da += 0.01f;
            smr[0].material.SetFloat("_DissolveAmount", da);
            smr[1].material.SetFloat("_DissolveAmount", da);
            smr[2].material.SetFloat("_DissolveAmount", da);
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion
}
