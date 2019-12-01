using UnityEngine;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    private Animator ani, aniRoot;          // 模型動畫控制器，工具動畫控制器
    private vThirdPersonController tpc;     // 第三人稱控制器

    private void Start()
    {
        ani = GetComponent<Animator>();
        aniRoot = transform.root.GetComponent<Animator>();          // 根物件 transform.root
        tpc = transform.root.GetComponent<vThirdPersonController>();
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");     // 左右、AD
        float v = Input.GetAxisRaw("Vertical");       // 上下、WS

        //print("左右：" + h);
        //print("上下：" + v);

        ani.SetBool("走路開關", h != 0 || v != 0);
        ani.SetBool("跑步開關", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jump()
    {
        ani.SetBool("跳躍開關", !aniRoot.GetBool("IsGrounded"));
    }

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
        }
        else
        {
            tpc.enabled = true;
        }
    }
}
