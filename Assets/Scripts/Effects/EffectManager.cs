using System.Collections.Generic;
using UnityEngine;

class EffectManager : MonoBehaviour
{
	public void Play(GameObject effect, Vector2 position, float angle, Transform parent = null)
	{
		if (effect == null) return;
		GameObject newEffect = Instantiate(effect, position, Quaternion.Euler(0, 0, angle));

		if (parent != null)
			newEffect.transform.SetParent(parent, true);

		StartEffect(newEffect);
	}

	static void StartEffect(GameObject effectInstance)
	{
		float duration = 0;

		ParticleSystem[] particleSystems = effectInstance.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in particleSystems)
		{
			ParticleSystem.MainModule main = ps.main;
			main.loop = false;
			duration = Mathf.Max(duration, ps.main.duration);
			ps.Play();
		}

		AudioSource[] audioSources = effectInstance.GetComponentsInChildren<AudioSource>();
		List<AudioSource> disabledSources = new();
		List<AudioSource> enabledSources = new();

		for (int i = 0; i < audioSources.Length; i++)
		{
			if (!audioSources[i].enabled)
				disabledSources.Add(audioSources[i]);
			else
				enabledSources.Add(audioSources[i]);
		}

		if (disabledSources.Count != 0)
		{
			int selectedClip = Random.Range(0, disabledSources.Count);
			disabledSources[selectedClip].enabled = true;
			enabledSources.Add(disabledSources[selectedClip]);
		}

		foreach (AudioSource audioSource in enabledSources)
		{
			audioSource.Play();
			duration = Mathf.Max(duration, audioSource.clip.length);
		}
		 
		Destroy(effectInstance, duration);
	}
}
