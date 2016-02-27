using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InputScene : MonoBehaviour {

	[SerializeField] string[] NameList;
	[SerializeField] Dropdown NameDoropDown;	
	[SerializeField] InputField Health;

	void Start () {
		var nameMasterTable = new NameMasterTable ();
		nameMasterTable.Load ();

		// csvの名前一覧を設定
		Dropdown.OptionDataList nameList = new Dropdown.OptionDataList();
		for (int i = 0; i < nameMasterTable.All.Count; ++i) {
			nameList.options.Add (new Dropdown.OptionData(nameMasterTable.All[i].Name));
		}
		NameDoropDown.options = nameList.options;
	}

	void Update () {
		
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
		SceneManager.LoadScene ("TitleScene");
	}
}
