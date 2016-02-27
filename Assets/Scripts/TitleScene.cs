using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScene : MonoBehaviour {

	private bool IsEntry = false;

	void Start () {
		string name = PlayerPrefs.GetString (Const.NameKey);
		if (name != "") {
			IsEntry = true;
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
