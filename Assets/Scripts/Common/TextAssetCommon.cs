using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/*===============================================================*/
/**
* テキストアセットのユーティリティクラス
* 2014年11月30日 Buravo
*/ 
public sealed class TextAssetCommon 
{
	
    /*===============================================================*/
    /**
    * @brief コンストラクタ
    */
    private TextAssetCommon () {}
    /*===============================================================*/

    /*===============================================================*/
    /**
    * @brief テキストデータの読み込みを行う関数
    * @param string テキストデータのファイルパス
    * @return string テキストデータ
    */
    public static string ReadText (string t_file_path)
    {
        if (t_file_path != null)
        {
            TextAsset textAsset = Resources.Load(t_file_path) as TextAsset;
            string text = textAsset.text;
            return text;
        }
        else
        {
            Debug.Log("TextAssetCommon.ReadText : filePath is null .");
            return null;
        }
    }
    /*===============================================================*/

    /*===============================================================*/
    /**
    * @brief 改行を区切り文字として分割したテキストデータを取得する関数
    * @param string テキストデータ
    * @return string[] 分割したテキストデータ
    */
    public static string[] GetTextLines (string t_text)
    {
        if (t_text != null)
        {
            // OS環境ごとに適切な改行コードをCR(=キャリッジリターン)に置換.
            string text = t_text.Replace(Environment.NewLine, "\r");
            // テキストデータの前後からCRを取り除く.
            text = text.Trim('\r');
            // CRを区切り文字として分割して配列に変換.
            string[] textLines = text.Split('\r');
            return textLines;
        } 
        else
        {
            Debug.Log("TextAssetCommon.GetTextLines : text is null .");
            return null;
        }
    }
    /*===============================================================*/
}
/*===============================================================*/

