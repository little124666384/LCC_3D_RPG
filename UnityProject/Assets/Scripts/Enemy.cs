using UnityEngine;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);                // 圖示.顏色
        Gizmos.DrawSphere(transform.position, distanceAttack);  // 圖示.繪製球體(中心點，半徑)

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrack);
    }
}
