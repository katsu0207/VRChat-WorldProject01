using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Text;
using System.Collections.Generic;
using System.IO;

public class DataCollector : UdonSharpBehaviour
{
    [Header("アンケートのContent (親オブジェクト)")]
    public Transform questionnaireContent;  // Scroll View > Viewport > Content

    [Header("参加者ID入力フィールド")]
    public InputField participantIdField;

    [Header("結果を表示するText (任意)")]
    public Text outputText;

    // UdonではListが使えないため配列で固定長バッファを確保
    private string[] dataLines = new string[100];
    private int dataCount = 0;

    public void SaveData()
    {
        Debug.Log("呼び出されてるよ！！！！！");


      string id = participantIdField != null ? participantIdField.text : "N/A";

        StringBuilder sb = new StringBuilder();
        sb.Append("ID: ").Append(id);

        int qIndex = 1;
        foreach (Transform question in questionnaireContent)
        {
            if (question.name.StartsWith("7Likert"))
            {
                int selectedValue = GetSelectedToggleValue(question);
                sb.Append($", Q{qIndex}: {selectedValue}");
                qIndex++;
            }
        }

        string resultLine = sb.ToString();

        // 🔧 UdonではList.Addが使えないので自前で配列管理
        if (dataCount < dataLines.Length)
        {
            dataLines[dataCount] = resultLine;
            dataCount++;
        }

        Debug.Log("[DataCollectorLikertUdon] " + resultLine);

        if (outputText != null)
            outputText.text = resultLine;
    }

    private int GetSelectedToggleValue(Transform question)
    {
        Transform choices = question.Find("Choices");
        if (choices == null) return 0;

        int i = 1;
        foreach (Transform toggleObj in choices)
        {
            Toggle t = toggleObj.GetComponent<Toggle>();
            if (t != null && t.isOn)
                return i;
            i++;
        }
        return 0; // どれも選ばれていない場合
    }

    public void PrintAllResults()
    {
        Debug.Log("=== All Stored Results ===");
        for (int i = 0; i < dataCount; i++)
        {
            Debug.Log(dataLines[i]);
        }
        Debug.Log("==========================");
    }
}