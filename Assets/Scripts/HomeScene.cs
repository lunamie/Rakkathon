using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeScene : MonoBehaviour {

	[SerializeField] Text CountUPText;

	private int MaxCount = 5;		// 目標値
	private int CurrentCount = 0;

	// ホーム画面内でのモード
	private enum Mode{
		Home,
		Abs,
		Result,
	};
	private Mode CurrentMode = Mode.Home;

	//------------------------------------------
	// Mono -> Start
	//------------------------------------------
	void Start () {
		Debug.Log("ホーム画面");

		CountUPText.text = CurrentCount.ToString();
	}

	//------------------------------------------
	// Mono -> Update
	//------------------------------------------
	void Update () {
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
			// リザルト出したら自動でホームモードに戻る
			CurrentMode = Mode.Home;
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
			// @todo:ラスト一回になった時だけ違うボイス再生
			string seName = string.Format("count_voice_{0}",CurrentCount);
			//Audio.instance.PlaySE(seName);

			if (CurrentCount == MaxCount) {
				CurrentMode = Mode.Result;
				// ほめてくれるボイス再生
				//Audio.instance.PlaySE("");
				Debug.Log ("目標カウントに到達しました！");
			}
		}
	}

	//------------------------------------------
	// ジャイロセンサーチェック？
	//------------------------------------------
	private void CheckAbs() {

	}

	//------------------------------------------
	// リザルトダイアログ
	//------------------------------------------
	private void Result() {

	}

	//------------------------------------------
	// 腹筋ボタン
	//------------------------------------------
	public void OnAbsButton() {
		CurrentMode = Mode.Abs;
		Debug.Log("腹筋モードへ移行");
	}
}
