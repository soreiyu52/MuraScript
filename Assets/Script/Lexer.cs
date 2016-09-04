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
        private int LINE_NUMBER;
        public static string regexPat = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||[!-~]|" +
        "([ぁ-んァ-ヴ一-龠０-９ａ-ｚＡ-Ｚa-zA-Z0-9！▽”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿ー－＾￥＠【『「；：」』】、。・]*))?";
        Regex defaultRegex = new Regex(regexPat);
        List<Parser> pa = new List<Parser>();

        public Lexer(){ LINE_NUMBER = 0; }
        private int getLineNumber() { return LINE_NUMBER; }

        /// <summary>
        /// ファイル読み込み。
        /// </summary>
        /// <returns></returns>
        public void ReadFile(string scriptName)
        {
            // TextAssetとして、Resourcesフォルダからテキストデータをロードする
            // ここはunity特有。
            TextAsset stageTextAsset = Resources.Load("txt/" + scriptName) as TextAsset;
            // 文字列を代入
            using (StringReader reader = new StringReader(stageTextAsset.text))
            {
                while (reader.Peek() >= 0)
                {
                    ReadLine(reader.ReadLine());
                    //行の終わりには必ずeofがつく。
                    pa[getLineNumber()].post(Token.token_eof, "");
                    OutPut();
                    LINE_NUMBER++;
                    //Debug.Log("LinNo=" + getLineNumber());
                }
            }
        }

        /// <summary>
        /// 行出力用メソッド
        /// </summary>
        public void OutPut()
        {
            object v;
            if (pa[getLineNumber()].accept(out v))
                Debug.Log(getLineNumber() + ":Answer=" + v);
        }

        /// <summary>
        /// 受け取った行をトークンに分解する。
        /// </summary>
        /// <param name="ReadLine"></param>
        /// 
        protected void ReadLine(string readLine)
        {
            Debug.Log("=" + readLine);
            //受け取った行から正規表現にマッチするものを取り出す。
            pa.Add(new Parser(new SemanticAction()));
            foreach (Match match in defaultRegex.Matches(readLine))
            {
                if (match.Success && match.Length != 0)
                {
                    //token毎に振り分け
                    addToken(match, getLineNumber());
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
        public void addToken(Match match, int lineNumber)
        {
            //Parser pa = new Parser(new SemanticAction());
            //スペースだけのものは排除。
            if (match.Groups[1].Value != string.Empty) {
                // comment判断
                if (match.Groups[2].Value == string.Empty)
                {
                    //Token token;
                    //整数判断
                    if (match.Groups[3].Value != string.Empty)
                    {
                        //Debug.Log("数字: " + match.Groups[1].Value);
                        //NumTokenを生成。                        
                        pa[lineNumber].post(Token.token_Number, Int32.Parse(match.Groups[1].Value));
                    }
                    //文字リテラル判断
                    else if (match.Groups[4].Value != string.Empty)
                    {
                        //Debug.Log("文字列: " + match.Groups[1].Value);
                        //StrTokenを生成。
                        //token = new StrToken(getLineNumber(), match.Groups[4].Value);
                    }
                    //シナリオ（全角）判断
                    else if (match.Groups[6].Value != string.Empty)
                    {
                        //Debug.Log("これはシナリオ: " + match.Groups[1].Value);
                        //ScenarioTokenを生成。
                        //token = new ScenarioToken(getLineNumber(), match.Groups[6].Value);
                    }
                    //それ以外。変数名などもここ。
                    else
                    {
                        //Debug.Log("それ以外: " + match.Groups[1].Value);
                        //IdTokenを生成。
                        if (match.Groups[1].Value == "+")
                            pa[lineNumber].post(Token.token_Add, match.Groups[1].Value);
                        if (match.Groups[1].Value == "/")
                            pa[lineNumber].post(Token.token_Div, match.Groups[1].Value);
                        if (match.Groups[1].Value == "*")
                            pa[lineNumber].post(Token.token_Mul, match.Groups[1].Value);
                        if (match.Groups[1].Value == "-")
                            pa[lineNumber].post(Token.token_Sub, match.Groups[1].Value);
                    }
                }
            }
        }
    }
}