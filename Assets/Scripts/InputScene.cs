using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InputScene : MonoBehaviour {
	
	[SerializeField] Dropdown NameDoropDown;	
	[SerializeField] InputField Health;
	[SerializeField] Button EnterButton;

	private bool IsPossibleEntry = false;
	private bool IsSettingName = false;

	void Start () {
		var nameMasterTable = new NameMasterTable ();
		nameMasterTable.Load ();

		// csvの名前一覧を設定
		Dropdown.OptionDataList nameList = new Dropdown.OptionDataList();
		for (int i = 0; i < nameMasterTable.All.Count; ++i) {
			nameList.options.Add (new Dropdown.OptionData(nameMasterTable.All[i].Name));
		}
		NameDoropDown.options = nameList.options;

		// 初期値はなし
		NameDoropDown.captionText.text = "";
	}

	void Update () {
		if (NameDoropDown.captionText.text != "" && Health.text != "") {
			EnterButton.interactable = true;
		} else if (NameDoropDown.captionText.text == "" || Health.text == "") {
			EnterButton.interactable = false;
		}
	}

	// 名前ドロップダウンリスト
	public void OnValueChanged(int name){
	}

	// 決定ボタン
	public void OnEnter() {
		// 名前
		PlayerPrefs.SetString(Const.NameKey,NameDoropDown.captionText.text);
		// 目標体重
		PlayerPrefs.SetFloat (Const.GoalHealthKey,float.Parse(Health.text));

		// ホームに遷移
		SceneManager.LoadScene ("HomeScene");
	}
}
