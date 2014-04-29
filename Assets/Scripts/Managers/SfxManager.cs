using UnityEngine;
using System.Collections;

public class SfxManager : MonoBehaviour {

	private static SfxManager instance;
	
	private SfxManager() {}
	
	public static SfxManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SfxManager)) as SfxManager;
				if (instance == null)
				{
					SfxManager prefab = Resources.Load("Managers/_SfxManager", typeof(SfxManager)) as SfxManager;
					instance = Instantiate(prefab) as SfxManager;
				}
			}
			return instance;
		}
	}

	public AudioSource backgroundMusic;
	public AudioListener audioListener;
	public bool isBackgroundMuted;

	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
	}

	public void PlaySoundtrack(AudioClip clip)
	{
		backgroundMusic.clip = clip;
		backgroundMusic.loop = true;
		backgroundMusic.Play();
	}

	public void MuteBackgroundMusic()
	{
		if(backgroundMusic.volume != 0)
		{
			backgroundMusic.volume = 0;
			isBackgroundMuted = true;
		}
		else
		{
			backgroundMusic.volume = 1;
			isBackgroundMuted = false;
		}
	}
}
