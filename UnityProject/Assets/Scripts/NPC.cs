using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("對話介面")]
    public GameObject dialog;
    public Text textDialog;

    public string dialogStart = "陌生人你好，請問可以幫我找到十個零件嗎？";
    public string dialogNotComplete = "請問你還沒找到十個零件嗎？";
    public string dialogComplete = "陌生人，謝謝你幫我找到十個零件。";

    /// <summary>
    /// 對話開始
    /// </summary>
    private void DialogStart()
    {
        dialog.SetActive(true);
    }

    /// <summary>
    /// 對話結束
    /// </summary>
    private void DialogEnd()
    {
        dialog.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        DialogStart();
    }

    private void OnTriggerExit(Collider other)
    {
        DialogEnd();
    }
}
