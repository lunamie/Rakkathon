using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InputScene : MonoBehaviour {
	
	[SerializeField] Dropdown NameDoropDown;	
	[SerializeField] InputField Health;
	[SerializeField] Button EnterButton;

	private MessageMasterTable MessageTable = new MessageMasterTable();
	private NameMasterTable NameTable = new NameMasterTable();

	void Start () {
		NameTable.Load ();
		MessageTable.Load();

		// csvの名前一覧を設定
		Dropdown.OptionDataList nameList = new Dropdown.OptionDataList();
		for (int i = 0; i < NameTable.All.Count; ++i) {
			nameList.options.Add (new Dropdown.OptionData(NameTable.All[i].Name));
		}
		NameDoropDown.options = nameList.options;

		// 初期値はなし
		NameDoropDown.captionText.text = PlayerPrefs.GetString(Const.NameKey,"");
		float health = PlayerPrefs.GetFloat (Const.GoalHealthKey, 0.0f);
		if (health > 0.0f) {
			Health.text = health.ToString ();
		}
	}

	void Update () {
		if (NameDoropDown.captionText.text != "" && Health.text != "") {
			EnterButton.interactable = true;
		} else if (NameDoropDown.captionText.text == "" || Health.text == "") {
			EnterButton.interactable = false;
		}
	}

	// 名前ドロップダウンリスト
	public void OnValueChanged(){
		// ボイス再生
		string seName = NameTable.All.Find(x => x.Name == NameDoropDown.captionText.text).SE1;
		Audio.instance.PlayVoice(string.Format(Const.SEPathFormat,seName));
	}

	// 決定ボタン
	public void OnEnter() {
		// 名前
		PlayerPrefs.SetString(Const.NameKey,NameDoropDown.captionText.text);
		// 目標体重
		PlayerPrefs.SetFloat (Const.GoalHealthKey,float.Parse(Health.text));

		Audio.instance.PlayVoice (string.Format(Const.SEPathFormat,"125"));

		// ホームに遷移
		SceneManager.LoadScene ("HomeScene");
	}
}
