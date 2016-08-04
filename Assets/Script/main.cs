using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class main : MonoBehaviour {
    private string guitxt;

    // Use this for initialization
    void Start () {
        Debug.Log("start");
        ReadFile();
        Debug.Log("end");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void ReadFile() {
        // FileReadTest.txtファイルを読み込む
        FileInfo fi = new FileInfo(Application.dataPath + "/txt/" + "Test.txt");
        // 一行毎読み込み
        using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
            guitxt = sr.ReadToEnd();
            Debug.Log(guitxt);
        }
    }
}
