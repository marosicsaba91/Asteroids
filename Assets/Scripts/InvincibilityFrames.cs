using System.Collections;
using UnityEngine;

public class InvincibilityFrames : MonoBehaviour, IImpactHandler, IStartLevelHandler
{
	[SerializeField] float onNewLevelInvisibilityTime = 2;
	[SerializeField] new Collider2D collider;
	[SerializeField] SpriteRenderer spriteRenderer;
	void OnValidate()
	{
		if (collider == null)
			collider = GetComponentInChildren<Collider2D>();
		if (spriteRenderer == null)
			spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
	}
	public void OnStartLevel(int level) =>StartInvincibility();
	public void StartInvincibility() => StartCoroutine(Invisibility(onNewLevelInvisibilityTime));
	IEnumerator Invisibility(float time)  // TODO: Too Complicated
	{
		const int n = 50;
		collider.enabled = false;
		Color transparent1 = new(1, 1, 1, 0.3f);
		Color transparent2 = new(1, 1, 1, 0.6f);
		for (int i = 0; i < n; i++)
		{
			spriteRenderer.color = i % 2 == 0 ? transparent1 : transparent2;
			yield return new WaitForSeconds(time / n);
		}
		spriteRenderer.color = Color.white;
		collider.enabled = true;
	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push) =>
		StartInvincibility();
	public void OnAllHealthLost(ImpactReceiver receiver) { }

}

