using UnityEngine;
using System.Collections;


public class DebugInput : MonoBehaviour
{
	/// <summary>
	/// キー入力取得する
	/// </summary>
	void Update()
	{
		if ( Input.GetKeyDown(KeyCode.V) )
		{
			Audio.instance.PlayVoice("Voice/Sample");
		}
	}
}