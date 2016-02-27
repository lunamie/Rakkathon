using UnityEngine;
using System.Collections.Generic;

public class Audio : MonoBehaviour
{
	/// <summary>
	/// インスタンス。シングルトン
	/// </summary>
	static private Audio instance_ = null;
	static public Audio instance
	{
		get
		{
			return instance_ = instance_ ?? new GameObject("Audio").AddComponent<Audio>();
		}
	}

	/// <summary>
	/// オーディオソース
	/// </summary>
	AudioSource source;

	/// <summary>
	/// 読み込み済みクリップ
	/// </summary>
	Dictionary<string,AudioClip> clips = new Dictionary<string, AudioClip>();

	/// <summary>
	/// 初期化
	/// </summary>
	void Awake()
	{
		source = gameObject.AddComponent<AudioSource>();
	}

	/// <summary>
	/// SE再生
	/// </summary>
	/// <param name="fileName"></param>
	public void PlaySE( string fileName )
	{
		if ( !clips.ContainsKey(fileName) )
		{
			clips[fileName] = Resources.Load<AudioClip>(fileName);
		}
		source.PlayOneShot(clips[fileName]);
	}


	/// <summary>
	/// BGM再生
	/// </summary>
	/// <param name="fileName"></param>
	public void PlayBGM( string fileName )
	{
		if ( !clips.ContainsKey(fileName) )
		{
			clips[fileName] = Resources.Load<AudioClip>(fileName);
		}
		source.clip = clips[fileName];
		source.Play();
	}
}