﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class HomeScene : MonoBehaviour {

	[SerializeField] Text CountUPText;
	[SerializeField] Text CountMaxText;

	[SerializeField]
	GameObject AbsRoot;
	[SerializeField]
	GameObject HomeRoot;
	[SerializeField]
	GameObject ResultRoot;

	private int MaxCount = 0;		// 目標値
	[SerializeField]
	Text ResultCountUPText;
	[SerializeField]
	Text ResultCountMaxText;
	private int CurrentCount = 0;
	private int CurrentDay = 0;

	// ホーム画面内でのモード
	private enum Mode{
		Home,
		Abs,
		Result,
		CountDown,
	};
	private Mode CurrentMode = Mode.Home;

	// 日付が変わったか見る用
	private static int Today = 0;

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

		// 継続日数によって目標値を設定
		CurrentDay = PlayerPrefs.GetInt(Const.CurrentDay,1);
		MaxCount = Const.RecordGoalList [CurrentDay - 1];
		// 今日の日付取得
		Today = System.DateTime.Now.Day;
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
			if ( CheckAbs() || Input.GetKeyDown(KeyCode.V)) {
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
				ResultCountUPText.text = CurrentCount.ToString();
				ResultCountMaxText.text = "/" + MaxCount.ToString();
				CurrentMode = Mode.Result;
				CurrentCount = 0;
				// ほめてくれるボイス再生
				var message = messageMaster.All.FirstOrDefault(n => n.Message == "おめでとう！");
				Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));

				// リザルト表示
				ShowResult();
				// １日の合計腹筋回数に加算して保存する
				int current = PlayerPrefs.GetInt (string.Format(Const.RecordKey,CurrentDay), 0);
				Debug.Log (CurrentDay + "日に加算しました");
				PlayerPrefs.SetInt (string.Format(Const.RecordKey,CurrentDay), MaxCount + current);

				Debug.Log ("目標カウントに到達しました！");
			}
			else if( Random.Range(0, 4) == 0 )
			{
				//ランダムで腹筋時にしゃべる
				var yaleMessages = messageMaster.All.Where(n => n.Timing == 2);
				var message = yaleMessages.ElementAtOrDefault(Random.Range(0, yaleMessages.Count()));
				Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));

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
		HomeRoot.gameObject.SetActive(false);
		AbsRoot.gameObject.SetActive(false);
		ResultRoot.gameObject.SetActive(true);
	}

	private void CountDown()
	{
		StartCoroutine(CountDownRoutine());
	}
	IEnumerator CountDownRoutine()
	{
		var message = messageMaster.All.FirstOrDefault(n => n.Message == "用意はいい？");
		Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));
        yield return new WaitForSeconds(2f);
		message = messageMaster.All.FirstOrDefault(n => n.Message == "3");
		Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));
		yield return new WaitForSeconds(1f);
		message = messageMaster.All.FirstOrDefault(n => n.Message == "2");
		Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));
		yield return new WaitForSeconds(1f);
		message = messageMaster.All.FirstOrDefault(n => n.Message == "1");
		Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));
		yield return new WaitForSeconds(1f);
		message = messageMaster.All.FirstOrDefault(n => n.Message == "スタート！");
		Audio.instance.PlaySE(string.Format(VoicePathFormat, message.SEName));
		CurrentMode = Mode.Abs;
	}
	
	//------------------------------------------
	// 腹筋ボタン
	//------------------------------------------
	public void OnAbsButton() {
		CurrentMode = Mode.CountDown;
		ResultRoot.gameObject.SetActive(false);
        AbsRoot.gameObject.SetActive(true);
		HomeRoot.gameObject.SetActive(false);
		CountDown();
		CountMaxText.text = "/" + MaxCount.ToString();
		if (Today != System.DateTime.Now.Day) {
			// 日付が変わったので
			PlayerPrefs.SetInt (Const.PrevDay, Today);
			// もし７日目だったら１日目からリセット　
			if (CurrentDay == 7) {
				CurrentDay = 0;
				for (int i = 0; i < 7; ++i) {
					PlayerPrefs.DeleteKey (string.Format (Const.RecordKey, i));
				}
				// ～週目を増やす
				int currentWeek = PlayerPrefs.GetInt (Const.CurrentWeek, 1);
				PlayerPrefs.SetInt (Const.CurrentWeek, ++currentWeek);
			}
			PlayerPrefs.SetInt (Const.CurrentDay, ++CurrentDay);
		}
		Debug.Log("腹筋モードへ移行　カウントダウン開始");
	}

	//------------------------------------------
	// リザルトOKボタン
	//------------------------------------------
	public void OnResultOKButton() {
		ResultRoot.gameObject.SetActive(false);
		AbsRoot.gameObject.SetActive(false);
		HomeRoot.gameObject.SetActive(true);
		CurrentMode = Mode.Home;
	}
	Quaternion q = Quaternion.identity;

	//------------------------------------------
	// リタイアボタン
	//------------------------------------------
	public void OnRetireButton()
	{
		if( CurrentMode != Mode.Abs )
		{
			return;
		}
		ResultRoot.gameObject.SetActive(false);
        AbsRoot.gameObject.SetActive(false);
		HomeRoot.gameObject.SetActive(true);
		CurrentMode = Mode.Home;
	}

	//------------------------------------------
	// 名前変更ボタン
	//------------------------------------------
	public void OnNameChange()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("InputScene");
	}

	//------------------------------------------
	// 今日の体重
	//------------------------------------------
	public void OnValueChanged()
	{
		PlayerPrefs.SetFloat (Const.TodayHealthKey, 0f);
	}

	//------------------------------------------
	// ときめき記録
	//------------------------------------------
	public void OnRecord()
	{
		SceneManager.LoadScene ("RecordScene");
	}

	/**
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
	*/
}
