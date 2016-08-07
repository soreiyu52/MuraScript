using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class main : MonoBehaviour
{
    private string guitxt;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        ReadFile();
        //Debug.Log(MatchTest("274-0805"));
        Debug.Log("end");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ReadFile()
    {
        // TextAssetとして、Resourcesフォルダからテキストデータをロードする
        TextAsset stageTextAsset = Resources.Load("txt/Test") as TextAsset;
        // 文字列を代入
        string stageData = stageTextAsset.text;

        //Debug.Log("asset=" + stageData);

        //一行づつ分解
        string[] lines = stageData.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log("=" + lines[i]);
        }
    }


    String MatchTest(String TextBox)
    {
        //TextBox1に郵便番号っぽい文字列が含まれているか調べる
        if (System.Text.RegularExpressions.Regex.IsMatch(TextBox, @"\d\d\d-\d\d\d\d"))
        {
            Debug.Log("郵便番号が含まれています");
        }
        //TextBox1内の郵便番号っぽい文字列をすべて抽出する
        System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(TextBox, @"\d\d\d-\d\d\d\d");
        foreach (System.Text.RegularExpressions.Match m in mc)
        {
            Debug.Log(m.Value);
        }
        //TextBox1内の郵便番号っぽい文字列の"-"を削除して、【】で囲む
        TextBox = System.Text.RegularExpressions.Regex.Replace(TextBox, @"(\d\d\d)-(\d\d\d\d)", "【$1$2】");
        return TextBox;
    }
}
