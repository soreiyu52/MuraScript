using UnityEngine;
using System.Collections;

/// <summary>
/// Tokenの定義
/// protected Token(int line)  読み込んだスクリプトの行を取得するコンストラクタ
/// public int getLineNumber() 今何行目か返す
/// public bool isIdentifier() 識別子の判定を返す(true/false) 
/// public bool isNumber()     整数リテラルかの判定を返す(true/false)
/// public bool isString()     文字列リテラルかの判定を返す(true/false)
/// public void EOF()          最終行を示す特殊なメソッド
/// </summary>
namespace Mura
{
    public abstract class Token : MonoBehaviour
    {
        private int lineNumber;

        protected Token(int line)
        {
            lineNumber = line;
        }
        public int getLineNumber() { return lineNumber; }
        public bool isIdentifier() { return false; }
        public bool isNumber() { return false; }
        public bool isString() { return false; }
        //public bool getNumber() { throw new MuraException("not number token"); }
        public string getText() { return ""; }

        public void EOF()
        {
            lineNumber = -1;
        }
    }
}