﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class HomeScene : MonoBehaviour {

	[SerializeField] Text CountUPText;
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
	void Start(){
		Debug.Log("ホーム画面");
		CountUPText.text = CurrentCount.ToString();
		// ジャイロON?
		Input.gyro.enabled = true;

		Debug.Log(PlayerPrefs.GetString(Const.NameKey));
		Debug.Log(PlayerPrefs.GetFloat(Const.GoalHealthKey));

		messageMaster.Load();

	}

	public MessageMasterTable messageMaster = new MessageMasterTable ();

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
			if ( CheckAbs() ) {
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
			var currentCntMessage = messageMaster.All.FirstOrDefault(n => n.Message == CurrentCount.ToString());
			Audio.instance.PlaySE(string.Format(VoicePathFormat, currentCntMessage.SEName));

			if ( CurrentCount == MaxCount) {
				CurrentMode = Mode.Result;
				CurrentCount = 0;
				// ほめてくれるボイス再生
				var message = messageMaster.All.FirstOrDefault(n => n.Message == "おめでとう！");
				Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));

				// リザルト表示
				ShowResult();

				Debug.Log ("目標カウントに到達しました！");
			}
		}
	}

	enum AbsState
	{
		None,
		Lie,
		GetUp
	}

	private AbsState absState = AbsState.None;
	//------------------------------------------
	// ジャイロセンサーチェック？
	//------------------------------------------
	private bool CheckAbs() {
		float rad = Mathf.Abs( (int)q.eulerAngles.y % 180 - 90 );
		if ( absState != AbsState.Lie )
		{
			if ( rad > 80 )
			{
				absState = AbsState.Lie;

			}
			return false;
		}
		else
		{
			if ( rad < 10 )
			{
				absState = AbsState.GetUp;
				return true;
			}
			return false;
		}
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
		Debug.Log("腹筋モードへ移行");
	}

	//------------------------------------------
	// リザルトOKボタン
	//------------------------------------------
	public void OnResultOKButton() {
		ResultDialog.gameObject.SetActive(false);
		CurrentMode = Mode.Home;
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
