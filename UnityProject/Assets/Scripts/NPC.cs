using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("對話介面")]
    public GameObject dialog;
    public Text textDialog;
    [Header("對話內容")]
    public string dialogStart = "陌生人你好，請問可以幫我找到十個零件嗎？";
    public string dialogNotComplete = "請問你還沒找到十個零件嗎？";
    public string dialogComplete = "陌生人，謝謝你幫我找到十個零件。";
    [Header("對話速度"), Range(0.1f, 5.5f)]
    public float dialogSpeed = 0.5f;

    /// <summary>
    /// 對話開始
    /// </summary>
    private void DialogStart()
    {
        dialog.SetActive(true);
        StartCoroutine(ShowDialog());
    }

    /// <summary>
    /// 顯示對話內容：打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowDialog()
    {
        textDialog.text = dialogStart[0].ToString();
        yield return new WaitForSeconds(dialogSpeed);
        textDialog.text += dialogStart[1].ToString();
        yield return new WaitForSeconds(dialogSpeed);
        textDialog.text += dialogStart[2].ToString();
        yield return new WaitForSeconds(dialogSpeed);
        textDialog.text += dialogStart[3].ToString();
        yield return new WaitForSeconds(dialogSpeed);
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
