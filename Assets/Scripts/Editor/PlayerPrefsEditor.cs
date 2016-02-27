using UnityEngine;
using UnityEditor;

public class PlayerPrefsEditor {
	[MenuItem("Tools/PlayerPrefs/DeleteAll")]
	static void DeleteAll(){
		PlayerPrefs.DeleteAll ();
		Debug.Log ("全てのデータを削除しました。");
	}
}
