using System.Collections;
using UnityEngine;
class EnemyGun : MonoBehaviour, IGun
{
	[SerializeField] float duration = 10;
	[SerializeField] float damage = 5;
	[SerializeField] float speed = 10;
	[SerializeField] float push = 1;
	[SerializeField] float magnetism = 0.5f;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] int projectileCount = 1;
	[SerializeField] float cooldown = 1;
	[SerializeField] Projectile projectilePrefab;
	[SerializeField] float startDistance = 0.5f;

	Player _player;
	public float Damage => damage;
	public float Duration => duration;
	public float TargetSpeed => speed;
	public float Push => push;
	public float Magnetism => magnetism;
	public Transform Transform => transform;
	void Awake()
	{
		_player = FindObjectOfType<Player>();
	}
	public void FireAll() => StartCoroutine(FireAllCoroutine());

	IEnumerator FireAllCoroutine()
	{
		for (int i = 0; i < projectileCount; i++)
		{
			yield return new WaitForSeconds(cooldown);
			Fire();
		}
	}
	public void Fire()
	{
		Vector2 target = _player.gameObject.transform.position;
		Vector2 direction = target - (Vector2)transform.position;
		direction.Normalize();

		Projectile newProjectile = Instantiate(projectilePrefab);
		Vector2 startPosition = (Vector2)transform.position + (direction * startDistance);

		newProjectile.Setup(startPosition, direction * projectileSpeed, Damage, Push, Magnetism, Duration);
	}
}