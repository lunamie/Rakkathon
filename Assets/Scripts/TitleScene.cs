using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TitleScene : MonoBehaviour {

	private bool IsEntry = false;

	void Start () {
		string name = PlayerPrefs.GetString (Const.NameKey);
		if (name != "") {
			IsEntry = true;
		}

		MessageMasterTable MessageTable = new MessageMasterTable();
		MessageTable.Load();
		// ここでランダムボイス再生
		List<MessageMaster> randomSEs = MessageTable.All.FindAll(x => x.Timing == (int)Const.PlaytTiming.GameStart);
		int result = Random.Range (0, randomSEs.Count);
		// もし挨拶系のやつだったら時間を見て再生
		if (randomSEs[result].SEName == "45" || randomSEs[result].SEName == "46" || randomSEs[result].SEName == "47") {
			string se;
			if (System.DateTime.Now.Hour < 12) {
				se = "45";
			} else if (System.DateTime.Now.Hour < 18) {
				se = "46";
			} else {
				se = "47";
			}
			Audio.instance.PlayVoice (string.Format(Const.SEPathFormat,se));
		} else {
			Audio.instance.PlayVoice (string.Format(Const.SEPathFormat,randomSEs[result].SEName));
		}
	}

	// スタート
	public void OnStart() {
		//　登録済みだったらホーム画面に行く
		if (IsEntry) {
			SceneManager.LoadScene ("HomeScene");
		} else {
			SceneManager.LoadScene ("InputScene");
		}
	}
}
