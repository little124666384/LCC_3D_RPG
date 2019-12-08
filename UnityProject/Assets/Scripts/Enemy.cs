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

        if (dis <= distanceTrack)
        {
            ani.SetBool("走路開關", true);
            agent.SetDestination(target.position);                          // 代理器.設定目的地(三維向量)
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        
    }

    private void Attack()
    {

    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }
}
