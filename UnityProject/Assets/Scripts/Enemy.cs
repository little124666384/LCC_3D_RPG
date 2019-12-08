using UnityEngine;
using UnityEngine.AI;   // 引用 人工智慧 API

public class Enemy : MonoBehaviour
{
    [Header("基本欄位")]
    public float atk = 20;
    public float hp = 250;
    [Range(0f, 100f)]
    public float speed = 1.5f;
    [Range(0f, 100f), Tooltip("攻擊距離")]
    public float distanceAttack = 5f;
    [Range(0f, 100f), Tooltip("追蹤距離")]
    public float distanceTrack = 25f;
    [Range(2.5f, 5f)]
    public float cd = 3.5f;

    private float timer;
    private Transform target;   // 目標物件
    private Animator ani;       // 動畫元件
    private NavMeshAgent agent; // 導覽代理器元件

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
    }

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

    private void Idle()
    {
        ani.SetBool("走路開關", !(agent.isStopped = true));                 // 是否停止 = 否 - 動畫狀態 = 顛倒(是否停止)
    }

    private void Attack()
    {
        if (timer >= cd)                                                    // 如果 計時器 >= 冷卻時間
        {
            timer = 0;                                                      // 歸零重新計算時間
            agent.isStopped = true;                                         // 停止代理器避免滑行
            ani.SetTrigger("攻擊觸發");                                      // 工及動畫
        }
        else
        {
            timer += Time.deltaTime;                                        // 否則 計時器 < 冷卻時間，累加時間
            Idle();                                                         // 等待
        }
    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }
}
