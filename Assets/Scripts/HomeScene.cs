using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeScene : MonoBehaviour {

	[SerializeField] Text CountUPText;
	[SerializeField] GameObject AbsButton;
	[SerializeField] GameObject ResultDialog;

	private int MaxCount = 5;		// 目標値
	private int CurrentCount = 0;
	private bool IsChangeMode = false;

	// ホーム画面内でのモード
	private enum Mode{
		Home,
		Abs,
		Result,
	};
	private Mode CurrentMode = Mode.Home;

	private static readonly string VoicePathFormat = "Voice/{0}";

	//------------------------------------------
	// Mono -> Start
	//------------------------------------------
	void Start () {
		Debug.Log("ホーム画面");
		CountUPText.text = CurrentCount.ToString();
		// ジャイロON?
		Input.gyro.enabled = true;

		Debug.Log (PlayerPrefs.GetString(Const.NameKey));
		Debug.Log (PlayerPrefs.GetFloat(Const.GoalHealthKey));
	}

	//------------------------------------------
	// Mono -> Update
	//------------------------------------------
	void Update () {
		q = Input.gyro.attitude;
		// モードによって状態を分ける
		switch (CurrentMode) {
		case Mode.Home:
			// 何もしない
			break;
		case Mode.Abs:
			// @todo:ここは腹筋をしたかどうかの判定になる予定
			if (Input.GetKeyDown (KeyCode.Space)) {
				CountUP();
			}
			break;
		case Mode.Result:
			// リザルトでOK押したらHomeに戻る
			break;
		};
	}

	//------------------------------------------
	// 呼び出される度にカウントアップ
	//------------------------------------------
	private void CountUP() {
		// 目標カウントに到達してなかったらカウントアップ
		if (CurrentCount < MaxCount) {
			++CurrentCount;
			// 画面の数字に反映させる
			CountUPText.text = CurrentCount.ToString();
			// ボイス再生
			string seName = string.Format("count_voice_{0}",CurrentCount);
			// @todo:ラスト一回になった時だけ違うボイス再生
			Audio.instance.PlaySE(string.Format(VoicePathFormat,"sample"));

			if (CurrentCount == MaxCount) {
				CurrentMode = Mode.Result;
				CurrentCount = 0;
				// ほめてくれるボイス再生
				Audio.instance.PlaySE(string.Format(VoicePathFormat,"sample"));
				// リザルト表示
				ShowResult();

				Debug.Log ("目標カウントに到達しました！");
			}
		}
	}

	//------------------------------------------
	// ジャイロセンサーチェック？
	//------------------------------------------
	private bool CheckAbs() {

		return false;
	}

	//------------------------------------------
	// リザルトダイアログ表示
	//------------------------------------------
	private void ShowResult() {
		ResultDialog.gameObject.SetActive(true);
	}

	//------------------------------------------
	// 腹筋ボタン
	//------------------------------------------
	public void OnAbsButton() {
		IsChangeMode = true;
		CurrentMode = Mode.Abs;
		AbsButton.SetActive (false);
		Debug.Log("腹筋モードへ移行");
	}

	//------------------------------------------
	// リザルトOKボタン
	//------------------------------------------
	public void OnResultOKButton() {
		ResultDialog.gameObject.SetActive(false);
		CurrentMode = Mode.Home;
		AbsButton.SetActive(true);
	}
	Quaternion q = Quaternion.identity;
	//------------------------------------------
	// 実機用デバッグ表示
	//------------------------------------------
	void OnGUI() {
		// Androidの傾き表示

		//Input.gyro.userAcceleration;

		GUIStyle style = new GUIStyle ();
		GUIStyleState state = new GUIStyleState ();
		state.textColor = Color.black;
		style.normal = state;
		
		GUI.Label(new Rect(20,20,300,60),string.Format("X:{0}", q.eulerAngles.x),style);
		GUI.Label(new Rect(20,40,300,80),string.Format("Y:{0}", q.eulerAngles.y),style);
		GUI.Label(new Rect(20,60,300,100),string.Format("Z:{0}", q.eulerAngles.z),style);
	}
}
