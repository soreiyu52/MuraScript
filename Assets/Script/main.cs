using Mura;
using UnityEngine;

public class main : MonoBehaviour
{
    private string guitxt;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        Lexer mura = new Lexer();
        mura.ReadFile("Test");
        Debug.Log("end");
    }

    // Update is called once per frame
    void Update()
    {

    }


}
