using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
	[SerializeField] new Rigidbody2D rigidbody2D;
	[Space]
	[SerializeField, Min(0)] float acceleration = 10;
	[SerializeField, Min(0)] float maxSpeed = 10;
	[SerializeField, Min(0)] float angularSpeed = 180;

	[SerializeField] ParticleStateInterpolator[] emitters;

	public float AccelerationMultiplier { get; internal set; } = 1;
	public float AngularSpeedMultiplier { get; internal set; } = 1;

	void OnValidate()
	{
		if (rigidbody2D == null)
			rigidbody2D = GetComponentInChildren<Rigidbody2D>();
	}

	void Start()
	{
		rigidbody2D.velocity = Vector3.zero;
		rigidbody2D.position = Vector3.zero;
		transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		gameObject.SetActive(true);
		AccelerationMultiplier = 1;
		AngularSpeedMultiplier = 1;
	}

	public float Drive { get; private set; }
	public bool UseEngine { get; private set; }
	public Vector2 Velocity => rigidbody2D.velocity;

	public float Drag
	{
		get => rigidbody2D.drag;
		set => rigidbody2D.drag = value;
	}

	void FixedUpdate()
	{
		float forward = Input.GetAxisRaw("Vertical");
		float turn = Input.GetAxisRaw("Horizontal");
		forward = Mathf.Clamp01(forward);

		if (forward > 0)
		{
			rigidbody2D.velocity += acceleration * Time.fixedDeltaTime * (Vector2)transform.up;
			rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);
		}

		if (turn != 0)
		{
			float r = rigidbody2D.rotation;
			r += AngularSpeedMultiplier * angularSpeed * Time.fixedDeltaTime * -turn;
			rigidbody2D.rotation = r;
		}

		// Emission
		foreach (ParticleStateInterpolator emitter in emitters)
		{
			emitter.SetValue(forward);
		}
	}
}