using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// Lexer（字句解析クラス）
/// [0-9]+ 整数リテラルが一文字以上並んだもの
/// 
/// </summary>
/// 
namespace Mura
{
    public class Lexer
    {
        private int LINE_NUBER = 0;
        public static string regexPat = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||\\p{P}|[\\|$'=~^\\`+><])?";
        //public static string regexPat = "\\s*(([0-9]+)|(\\p{P}))?";
        Regex defaultRegex = new Regex(regexPat);
        //MatchCollection matches;
        // private List<Token> queue = new List<Token>();


        /*
         *テキスト読み込み メソッド
         * 
         * string stageData テキストを代入
         * 
         */
        private int getLineNumber()
        {
            return LINE_NUBER;
        }

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
                Debug.Log("LinNo="+getLineNumber());
            }
        }

        /// <summary>
        /// 受け取った行をトークンに分解する。
        /// </summary>
        /// <param name="ReadLine"></param>
        protected void ReadLine(string readLine)
        {
            String line;
            line = readLine;
            int lineNo = getLineNumber();
            foreach (Match match in defaultRegex.Matches(line))
            {
                if (match.Value != "") {
                    Debug.Log(match.Value);
                    //Debug.Log(" index=" + match.Index);
                }
            }
        }

        /*Match match = defaultRegex.Match(line);
            Debug.Log("line="+line+":"+"match="+ match.Value);
        }*/
    }
}


