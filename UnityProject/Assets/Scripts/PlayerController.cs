using UnityEngine;
using UnityEngine.UI;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    #region 欄位
    [Header("血量"), Range(100, 500)]
    public float hp = 100;
    [Header("血條")]
    public Slider hpSlider;
    [Range(1.5f, 5f), Tooltip("攻擊範圍")]
    public float rangeAttack = 3f;
    [Range(0, 5), Tooltip("攻擊延遲判定")]
    public float delayAttack = 1.2f;
    [Header("攻擊力")]
    public float attack = 20;

    private Animator ani, aniRoot;          // 模型動畫控制器，工具動畫控制器
    private vThirdPersonController tpc;     // 第三人稱控制器
    private Rigidbody rig;
    #endregion

    #region 事件
    private void Start()
    {
        ani = GetComponent<Animator>();
        aniRoot = transform.root.GetComponent<Animator>();          // 根物件 transform.root
        tpc = transform.root.GetComponent<vThirdPersonController>();
        rig = transform.root.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (ani.GetBool("死亡開關")) return;
        Move();
        Jump();
        Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack);
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動狀態：動畫控制
    /// </summary>
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");     // 左右、AD
        float v = Input.GetAxisRaw("Vertical");       // 上下、WS

        //print("左右：" + h);
        //print("上下：" + v);

        ani.SetBool("走路開關", h != 0 || v != 0);
        ani.SetBool("跑步開關", Input.GetKey(KeyCode.LeftShift));
    }

    /// <summary>
    /// 跳躍狀態：動畫控制
    /// </summary>
    private void Jump()
    {
        ani.SetBool("跳躍開關", !aniRoot.GetBool("IsGrounded"));
    }

    /// <summary>
    /// 攻擊狀態：動畫控制
    /// </summary>
    private void Attack()
    {
        // 如果 按下左鍵 並且 不是攻擊動畫 並且 不是在轉換線
        if (Input.GetKeyDown(KeyCode.Mouse0) && !ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊") && !ani.IsInTransition(0))
        {
            ani.SetTrigger("攻擊觸發");
            Invoke("DelayAttack", delayAttack);
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("受傷"))
        {
            CancelInvoke("DelayAttack");
        }

        // 取得動畫狀態資訊.動畫名稱是否為 ""
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊") || ani.GetCurrentAnimatorStateInfo(0).IsName("受傷"))
        {
            tpc.enabled = false;
            tpc.lockMovement = true;
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            tpc.enabled = true;
            tpc.lockMovement = false;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    /// <summary>
    /// 延遲給予玩家怪物傷害值，避免剛舉手怪物就受傷
    /// </summary>
    private void DelayAttack()
    {
        RaycastHit hit; // 射線碰撞資訊
        // 物理.射線碰撞(起點，方向，射線碰撞資訊，長度)
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rangeAttack))
        {
            if (hit.collider.gameObject.tag == "敵人")
            {
                hit.collider.GetComponent<Enemy>().Hit(attack);
            }
        }
    }

    /// <summary>
    /// 受傷狀態：動畫與扣血
    /// </summary>
    /// <param name="damage">每次接收的傷害值</param>
    public void Hit(float damage)
    {
        hp -= damage;
        hpSlider.value = hp;
        ani.SetTrigger("受傷觸發");
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        tpc.enabled = false;
        tpc.lockMovement = true;
        rig.constraints = RigidbodyConstraints.FreezeAll;
        ani.SetBool("死亡開關", true);
    }
    #endregion
}
