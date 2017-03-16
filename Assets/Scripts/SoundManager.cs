using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource backgroundSource;

	public void PlayBackground ()
	{
		if (!backgroundSource.isPlaying)
		{
			backgroundSource.Play ();
		}
	}

	public void StopBackground ()
	{
		if (backgroundSource.isPlaying)
		{
			backgroundSource.Stop ();
		}
	}
}
