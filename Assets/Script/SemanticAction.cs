using UnityEngine;

namespace Mura
{
    public class SemanticAction : ISemanticAction
    {
        public void stack_overflow() { }
        public void syntax_error() { }

        public void Identity(out int arg0, int arg1) {arg0 = arg1; }


        public void MakeSub(out int arg0, int arg1, int arg2)
        {
            //Debug.Log("expr " + arg1 + " - " + arg2);
            arg0 = arg1 - arg2;
        }

        public void MakeMul(out int arg0, int arg1, int arg2)
        {
            //Debug.Log("expr " + arg1 + " * " + arg2);
            arg0 = arg1 * arg2;
        }

        public void MakeDiv(out int arg0, int arg1, int arg2)
        {
            //Debug.Log("expr " + arg1 + " / " + arg2);
            arg0 = arg1 / arg2;
        }

        public void MakeAdd(out int arg0, int arg1, int arg2)
        {
            //Debug.Log("expr " + arg1 + " + " + arg2);
            arg0 = arg1 + arg2;
        }
    }
}