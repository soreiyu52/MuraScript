using UnityEngine;
using System.Collections;

/// <summary>
/// Tokenの定義
/// protected Token(int lineNo)  読み込んだスクリプトの行を取得するコンストラクタ
/// public int getLineNumber() 今何行目か返す
/// public bool isIdentifier() 識別子の判定を返す(true/false) 
/// public bool isNumber()     整数リテラルかの判定を返す(true/false)
/// public bool isString()     文字列リテラルかの判定を返す(true/false)
/// public void EOF()          最終行を示す特殊なメソッド
/// 継承先でメソッドを変えるため、仮想メソッドにしておく。
/// </summary>
namespace Mura
{
    public abstract class Token
    {
        internal int lineNo;

        public Token(int lineNo)
        {
            this.lineNo = lineNo;
        }
        public virtual int getLineNumber() { return lineNo; }
        public virtual bool isIdentifier() { return false; }
        public virtual bool isNumber() { return false; }
        public virtual bool isString() { return false; }
        //public bool getNumber() { throw new MuraException("not number token"); }
        public virtual string getText() { return ""; }
        public void EOF()
        {
            lineNo = -1;
        }
    }
}