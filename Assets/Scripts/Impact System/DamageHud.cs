using TMPro;
using UnityEngine;

public class DamageHud : MonoBehaviour
{
	[SerializeField] TMP_Text text;
	[SerializeField] new MeshRenderer renderer;
	[SerializeField] AnimationCurve scaleOverAgeX;
	[SerializeField] AnimationCurve scaleOverAgeY;
	[SerializeField] float minimumDamageToShow;
	[SerializeField, Range(0, 1)] float positionAtDamagePoint = 0.5f;
	[SerializeField] float maxRotation = 20f;
	[SerializeField] int orderInLayer = 1;
	[SerializeField] float fadeTime = 1f;
	[SerializeField] float destroyTime = 2f;

	Vector3 _impactPosition;
	Quaternion _rotation;
	float _damage;

	public ImpactDamageHudPlayer Owner { get; set; }
	public DamageHud Prefab { get; set; }
	void OnValidate()
	{
		if (text == null)
			text = GetComponentInChildren<TMP_Text>();
		if (renderer == null)
			renderer = GetComponentInChildren<MeshRenderer>();

		if (renderer != null)
			renderer.sortingOrder = orderInLayer;
	}


	public void OnImpact(Vector2 position, float damage)
	{
		_damage += damage;
		_impactPosition = position;
		_rotation = Quaternion.identity;
		_rotation *= Quaternion.Euler(0, 0, Random.Range(-maxRotation, maxRotation));
		renderer.sortingOrder = orderInLayer;

		string message;
		if (_damage >= minimumDamageToShow)
			message = _damage.ToString("0");
		else
			return;

		text.text = message;
		startTime = Time.time;
		Destroy(gameObject, destroyTime);
	}

	float startTime;

	void Update()
	{
		float time = Time.time - startTime;
		Vector3 position = Owner == null ? _impactPosition
			: Vector2.Lerp(Owner.transform.position, _impactPosition, positionAtDamagePoint);

		transform.SetPositionAndRotation(position, _rotation);

		float scaleX = scaleOverAgeX.Evaluate(1 - time / fadeTime);
		float scaleY = scaleOverAgeY.Evaluate(1 - time / fadeTime);
		transform.localScale = new Vector3(scaleX, scaleY, 1);
	}

}
