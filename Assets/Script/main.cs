using Mura;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        List<Lexer> l = new List<Lexer>();
        l.Add(new Lexer());
        l.Add(new Lexer());

        Debug.Log("start");
        l[0].ReadFile("Test");
        Debug.Log("end");

        Debug.Log("start");
        l[1].ReadFile("Test2");
        Debug.Log("end");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
