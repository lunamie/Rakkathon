using UnityEngine;
using System.Collections;

// 定数
public class Const {
	public static readonly string SEPathFormat = "Voice/{0}";
	public static readonly string NameKey = "Name";
	public static readonly string GoalHealthKey = "Health";
	public static readonly string TodayHealthKey = "TodayHealth";
	public static readonly string RecordKey = "Record_{0}";
	public static readonly string CurrentDay = "CurrentDay";
	public static readonly string CurrentWeek = "CurrentWeek";

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
}
