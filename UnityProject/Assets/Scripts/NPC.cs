using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("對話介面")]
    public GameObject dialog;
    public Text textDialog;
    [Header("對話內容")]
    public string dialogStart = "陌生人，請問可以替我找到十個零件嗎？";
    public string dialogNotComplete = "陌生人，請問還沒找到十個零件嗎？";
    public string dialogComplete = "陌生人，謝謝替我找到十個零件。";
    [Header("對話速度"), Range(0.001f, 5.5f)]
    public float dialogSpeed = 0.5f;
    [Header("音效")]
    public AudioClip soundDialog;
    public AudioClip soundProp;

    // 定義列舉
    public enum npcState
    {
        start, notComplete, complete
    }
    // 實例化列舉
    public npcState _npcState;

    [Header("零件任務")]
    public int propCurrent;
    public int propTotal;
    public Text textProp;

    private AudioSource aud;

    /// <summary>
    /// 對話開始
    /// </summary>
    private void DialogStart()
    {
        dialog.SetActive(true);
        StopAllCoroutines();                        // 停止所有協同程序

        switch (_npcState)
        {
            case npcState.start:
                StartCoroutine(ShowDialog(dialogStart));
                _npcState = npcState.notComplete;
                break;
            case npcState.notComplete:
                StartCoroutine(ShowDialog(dialogNotComplete));
                break;
            case npcState.complete:
                StartCoroutine(ShowDialog(dialogComplete));
                break;
        }

        /**
        int lv = 1;
        // 判斷式 switch
        switch (lv)
        {
            case 1:
                print("1 等");
                break;
            case 10:
                print("10 等");
                break;
            default:
                print("未定義");
                break;
        }
        */
    }

    /// <summary>
    /// 顯示對話內容：打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowDialog(string dialog)
    {
        textDialog.text = "";

        // for (初始值；條件(布林值)；迭代器
        for (int i = 0; i < dialog.Length; i++)
        {
            aud.PlayOneShot(soundDialog, 0.5f);
            textDialog.text += dialog[i].ToString();
            yield return new WaitForSeconds(dialogSpeed);
        }
    }

    /// <summary>
    /// 對話結束
    /// </summary>
    private void DialogEnd()
    {
        StopAllCoroutines();
        dialog.SetActive(false);
    }

    /// <summary>
    /// 取得道具，每次增加 1 個道具並更新介面
    /// </summary>
    public void GetProp()
    {
        propCurrent++;
        aud.PlayOneShot(soundProp, 0.5f);
        textProp.text = "零件： " + propCurrent + " / " + propTotal;
        if (propCurrent == propTotal) _npcState = npcState.complete;
    }

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        propTotal = GameObject.FindGameObjectsWithTag("道具").Length;
        textProp.text = "零件： 0 / " + propTotal;
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
