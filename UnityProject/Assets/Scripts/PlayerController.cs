using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator ani, aniRoot;  // 模型動畫控制器，工具動畫控制器

    private void Start()
    {
        ani = GetComponent<Animator>();
        aniRoot = transform.root.GetComponent<Animator>();  // 根物件 transform.root
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");     // 左右、AD
        float v = Input.GetAxisRaw("Vertical");       // 上下、WS

        print("左右：" + h);
        print("上下：" + v);

        ani.SetBool("走路開關", h != 0 || v != 0);
        ani.SetBool("跑步開關", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jump()
    {
        ani.SetBool("跳躍開關", !aniRoot.GetBool("IsGrounded"));
    }

    private void Attack()
    {

    }
}
