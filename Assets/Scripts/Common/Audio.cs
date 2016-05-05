using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Audio : MonoBehaviour
{
    private static readonly string VoicePathFormat = "Voice/{0}";
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
	
	AudioSource bgmSource;

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
		bgmSource = gameObject.AddComponent<AudioSource>();
		gameObject.AddComponent<AudioListener>();

		if (Application.isPlaying) {
			DontDestroyOnLoad (gameObject);
		}
	}


    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="fileName"></param>
    public void PlayVoice(MessageMaster message)
    {
        if (message == null)
        {
            return;
        }
        PlayVoice(string.Format(VoicePathFormat, message.SEName, message.Delay));
    }


    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="fileName"></param>
    public void PlayVoice( string fileName, float delay = 0f)
	{
		if ( !clips.ContainsKey(fileName) )
		{
			clips[fileName] = Resources.Load<AudioClip>(fileName);
		}
        if (clips[fileName] == null)
        {
            Debug.LogError("そんなファイルないよ : " + fileName);
        }
        source.Stop();
        source.clip = clips[fileName];
        source.PlayDelayed(delay);
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
		bgmSource.clip = clips[fileName];
		bgmSource.Play();
	}
}