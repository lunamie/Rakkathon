using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class RecordScene : MonoBehaviour {

	[SerializeField] Text CurrentCountWeek;
	[SerializeField] Image[] RecordResultClearImages;
	[SerializeField] Text[] RecordResultTexts;
	[SerializeField] Text[] RecordGoalTexts;

	void Start () {
		// 今何週目かを設定
		string week = string.Format("{0}週目",PlayerPrefs.GetInt(Const.CurrentWeek,1));
		CurrentCountWeek.text = week;

		//　目標値設定
		for (int i = 0; i < RecordGoalTexts.Length; ++i) {
			RecordGoalTexts [i].text = Const.RecordGoalList [i].ToString();
		}

		// PlayerPrefsから結果を取得して設定 なかったら０が入る
		for (int i = 0; i < RecordResultTexts.Length; ++i) {
			string key = string.Format(Const.RecordKey,i);
			int record = PlayerPrefs.GetInt(key, 0);
			RecordResultTexts [i].text = record.ToString();
			// 目標数を達成していたら中を黄色くする
			if (record >= Const.RecordGoalList [i]) {
				RecordResultClearImages [i].gameObject.SetActive (true);
			} else {
				RecordResultClearImages [i].gameObject.SetActive (false);
			}
		}
	}

	// OKボタンでホームに戻る
	public void OnOKButton() {
		SceneManager.LoadScene ("HomeScene");
	}
}
