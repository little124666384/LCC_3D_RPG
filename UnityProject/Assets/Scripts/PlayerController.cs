using UnityEngine;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    #region 欄位
    [Header("血量"), Range(100, 500)]
    public float hp = 100;

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
        Move();
        Jump();
        Attack();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊觸發");
        }

        // 取得動畫狀態資訊.動畫名稱是否為 ""
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊"))
        {
            tpc.enabled = false;
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            tpc.enabled = true;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    /// <summary>
    /// 受傷狀態：動畫與扣血
    /// </summary>
    /// <param name="damage">每次接收的傷害值</param>
    public void Hit(float damage)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
    }
    #endregion
}
