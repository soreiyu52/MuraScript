using Mura;
using UnityEngine;

public class main : MonoBehaviour
{
    private string guitxt;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        Lexer lexer = new Lexer();
        lexer.ReadFile("Test");

        foreach (Token queue in lexer.queue)
        {
            Debug.Log(queue+"=>"+ queue.getText()+ " ;" +queue.getLineNumber());
            Debug.Log(" id="+queue.isIdentifier()+" str:"+ queue.isString()+" num:"+queue.isNumber());
        }
        Debug.Log("end");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
