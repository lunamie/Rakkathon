using UnityEngine;
using System.Collections;

// 定数
public class Const {
	public static readonly string SEPathFormat = "Voice/{0}";
	public static readonly string NameKey = "Name";
	public static readonly string GoalHealthKey = "Health";
	public static readonly string TodayHealthKey = "TodayHealth";
	public static readonly string RecordKey = "Record_{0}";
	public static readonly string CurrentDay = "CurrentDay";	// 1～7が１週なので7が最大
	public static readonly string CurrentWeek = "CurrentWeek";
	public static readonly string PrevDay = "PrevDay";

	// messageマスターのTimingカラムに連動している
	public enum PlaytTiming {
		GameStart,				// 起動時
		AbsStart,				// 腹筋開始時
		Training,				// 腹筋中
		MissionGiveup,			// あきらめた
		MissionClear,			// 目標達成時
		ResultOkButtonClick,	// トレーニング完了時
		HomeModelTouch,			// ホームでモデルタッチ時
	}

	// １週の目標値
	public static readonly int[] RecordGoalList = new int[]{ 15,30,45,60,75,90,100 };
}
