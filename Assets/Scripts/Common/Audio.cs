using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
		gameObject.AddComponent<AudioListener>();

		if (Application.isPlaying) {
			DontDestroyOnLoad (gameObject);
		}
	}

	/// <summary>
	/// SE再生
	/// </summary>
	/// <param name="fileName"></param>
	public void PlaySE( string fileName, float delay = 0f)
	{
		if ( !clips.ContainsKey(fileName) )
		{
			clips[fileName] = Resources.Load<AudioClip>(fileName);
		}

		StartCoroutine(PlaySEWithDelay(clips[fileName], delay));
	}

	/// <summary>
	/// SE再生
	/// </summary>
	/// <param name="fileName"></param>
	public IEnumerator PlaySEWithDelay( AudioClip clip, float delay = 0f )
	{
		yield return new WaitForSeconds(delay);
		source.PlayOneShot(clip);
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