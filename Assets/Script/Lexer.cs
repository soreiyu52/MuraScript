using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// Lexer（字句解析クラス）
/// (//.*) :
/// [0-9]+ :整数リテラルが一文字以上並んだもの
/// (\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\"):[("][")]の中に入ってるもの。
/// [A-Z_a-z][A-Z_a-z0-9]*半角英字及び[_]の含む英数字
/// ==
/// <=
/// >=
/// &&
/// \\|\\|
/// [!-~]半角記号
/// ([ぁ-んァ-ヴ一-龠０-９ａ-ｚＡ-Ｚa-zA-Z0-9！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠【『「；：」』】、。・]*全角
/// </summary>
/// 
namespace Mura
{
    public class Lexer
    {
        private int LINE_NUBER = 0;
        public static string regexPat = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||[!-~]|" +
        "([ぁ-んァ-ヴ一-龠０-９ａ-ｚＡ-Ｚa-zA-Z0-9！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠【『「；：」』】、。・]*))?";
        Regex defaultRegex = new Regex(regexPat);
        public List<Token> queue = new List<Token>();
 
        /// <summary>
        /// 行番号を取り出す。
        /// </summary>
        /// <returns></returns>
        private int getLineNumber(){return LINE_NUBER;}

        /// <summary>
        /// ファイル読み込み。
        /// </summary>
        /// <returns></returns>
        public void ReadFile(string scriptName)
        {
            // TextAssetとして、Resourcesフォルダからテキストデータをロードする
            TextAsset stageTextAsset = Resources.Load("txt/" + scriptName) as TextAsset;
            // 文字列を代入
            // using句をしようすることにより、わざわざ閉じなくても良いようにする。
            using (StringReader reader = new StringReader(stageTextAsset.text))
            {
                //一行づつ分解
                while (reader.Peek() >= 0)
                {
                    //行を足す。
                    LINE_NUBER++;
                    ReadLine(reader.ReadLine());
                }
                Debug.Log("LinNo=" + getLineNumber());
            }
        }

        /// <summary>
        /// 受け取った行をトークンに分解する。
        /// </summary>
        /// <param name="ReadLine"></param>
        protected void ReadLine(string readLine)
        {
            //String line = readLine;
            //int lineNo = getLineNumber();
            //受け取った行から正規表現にマッチするものを取り出す。
            foreach (Match match in defaultRegex.Matches(readLine))
            {
                if (match.Success && match.Length != 0)
                {
                    //token毎に振り分け
                    addToken(match);
                }
            }
        }

        /// <summary>
        /// トークン毎に振り分け。
        /// Debug.Log("match.Groups0 " + match.Groups[0]);//全てのグループ（特殊）
        /// Debug.Log("match.Groups1 " + match.Groups[1]);//スペースを除いたすべての文字
        /// Debug.Log("match.Groups2 " + match.Groups[2]);
        /// Debug.Log("match.Groups3 " + match.Groups[3]);//数字
        /// Debug.Log("match.Groups4 " + match.Groups[4]);//文字列リテラル
        /// Debug.Log("match.Groups5 " + match.Groups[5]);
        /// Debug.Log("match.Groups6 " + match.Groups[6]);//全角かなカナ漢字記号
        /// Debug.Log("match.Groups7 " + match.Groups[7]);
        /// </summary>
        /// <param name="match"></param>
        protected void addToken(Match match)
        {
            //スペースだけのものは排除。
            if (match.Groups[1].Value != string.Empty) {
                // comment判断
                if (match.Groups[2].Value == string.Empty)
                { 
                    Token token;
                    //整数判断
                    if (match.Groups[3].Value != string.Empty)
                    {
                        //Debug.Log("数字: " + match.Groups[1].Value);
                        //NumTokenを生成。
                        token = new NumToken(getLineNumber(), int.Parse(match.Groups[1].Value));
                    }
                    //文字リテラル判断
                    else if (match.Groups[4].Value != string.Empty)
                    {
                        //Debug.Log("文字列: " + match.Groups[1].Value);
                        //StrTokenを生成。
                        token = new StrToken(getLineNumber(), match.Groups[4].Value);
                    }
                    //シナリオ（全角）判断
                    else if (match.Groups[6].Value != string.Empty)
                    {
                        //Debug.Log("これはシナリオ: " + match.Groups[1].Value);
                        //ScenarioTokenを生成。
                        token = new ScenarioToken(getLineNumber(), match.Groups[6].Value);
                    }
                    //それ以外。変数名などもここ。
                    else
                    {
                        //Debug.Log("それ以外: " + match.Groups[1].Value);
                        //IdTokenを生成。
                        token = new IdToken(getLineNumber(), match.Groups[1].Value);
                    }
                    queue.Add(token);
                    //Debug.Log(token + " :" + token.getText()+" :");*/
                }
            }
        }
    }

    /// <summary>
    /// 整数のtoken
    /// isNumber()  :tureを返す。他はfalseを継承。(tokenから)
    /// getText()   :整数をstringにしたものを返す
    /// getNumber() :整数を返す。
    /// lineNo      :行番号
    /// valse       :数字
    /// </summary>
    public class NumToken :Token
    {
        public int value;
        //オーバーライドされた基底クラス TokenのコンストラクターのlineNoに代入。
        public NumToken(int lineNo,int value) : base(lineNo)
        {
            base.lineNo = lineNo;
            this.value = value;
        }
        //基底クラスに同名クラスがあるので隠蔽するのでnew演算子が必要。
        public override bool isNumber(){ return true; }
        public override string getText(){ return value.ToString(); }
        public int getNumber() { return value; }
    }

    /// <summary>
    /// 文字リテラルのtoken
    /// isString()  :tureを返す。他はfalseを継承。(tokenから)
    /// getText()   :文字リテラルを返す。
    /// lineNo      :行番号
    /// litral      :文字リテラル
    /// </summary>
    public class StrToken : Token
    {
        private string literal;
        //オーバーライドされた基底クラス TokenのコンストラクターのlineNoに代入。
        internal StrToken(int lineNo, string literal) : base(lineNo)
        {
            base.lineNo = lineNo;
            this.literal = literal;
        }
        //基底クラスに同名クラスがあるので隠蔽するのでnew演算子が必要。
        public override bool isString() { return true; }
        public override string getText() { return literal; }
    }

    /// <summary>
    /// 文字リテラル(シナリオ)のtoken
    /// isString()  :tureを返す。他はfalseを継承。(tokenから)
    /// getText()   :文字リテラルを返す。
    /// lineNo      :行番号
    /// litral      :文字リテラル
    /// </summary>
    public class ScenarioToken : Token
    {
        private string scenario;
        //オーバーライドされた基底クラス TokenのコンストラクターのlineNoに代入。
        internal ScenarioToken(int lineNo, string scenario) : base(lineNo)
        {
            base.lineNo = lineNo;
            this.scenario = scenario;
        }
        //基底クラスに同名クラスがあるので隠蔽するのでnew演算子が必要。
        public override bool isString() { return true; }
        public override string getText() { return scenario; }
    }

    /// <summary>
    /// Id(Identifier)のtoken
    /// isIdentifier():tureを返す。他はfalseを継承。(tokenから)
    /// getText()     :文字リテラルを返す。
    /// lineNo        :行番号
    /// litral        :文字リテラル
    /// </summary>
    public class IdToken : Token
    {
        private String identifier;
        //オーバーライドされた基底クラス TokenのコンストラクターのlineNoに代入。
        internal IdToken(int lineNo, string identifier) : base(lineNo)
        {
            base.lineNo = lineNo;
            this.identifier = identifier;
        }
        //基底クラスの仮想メソッドをオーバーライドする。
        public override bool isIdentifier() { return true; }
        public override string getText() { return identifier; }
    }

}


