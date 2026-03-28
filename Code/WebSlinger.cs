using Sandbox;

public sealed class WebSlinger : Component
{
	[Property] public float MaxDistance { get; set; } = 5000f;
	[Property] public float PullStrength { get; set; } = 3000f;
	[Property] public float StopDistance { get; set; } = 50f;
	[Property] public float BoostMultiplier { get; set; } = 2f;
	[Property] public float BoostDuration { get; set; } = 0.5f;
	[Property] public float BoostCooldown { get; set; } = 3f;

	private Vector3? grapplePoint;
	private bool isGrappling;
	private float boostTimeRemaining = 0f;
	private float cooldownTimeRemaining = 0f;

	protected override void OnUpdate()
	{
		if ( !Input.Down( "attack1" ) )
		{
			StopGrapple();
			return;
		}

		if ( !isGrappling )
		{
			StartGrapple();
		}

		// Tick down timers
		if ( boostTimeRemaining > 0f )
		{
			boostTimeRemaining -= Time.Delta;

			// Boost just ended, start cooldown
			if ( boostTimeRemaining <= 0f )
			{
				cooldownTimeRemaining = BoostCooldown;
			}
		}

		if ( cooldownTimeRemaining > 0f )
		{
			cooldownTimeRemaining -= Time.Delta;
		}

		// Activate boost on shift press, only if not boosting or on cooldown
		if ( Input.Pressed( "run" ) && isGrappling && boostTimeRemaining <= 0f && cooldownTimeRemaining <= 0f )
		{
			boostTimeRemaining = BoostDuration;
		}

		ContinueGrapple();
	}

	private void StartGrapple()
	{
		var camera = Scene.Camera;
		if ( camera is null ) return;

		var start = camera.WorldPosition;
		var end = start + camera.WorldRotation.Forward * MaxDistance;

		var trace = Scene.Trace.Ray( start, end )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();

		if ( !trace.Hit ) return;

		grapplePoint = trace.EndPosition;
		isGrappling = true;
	}

	private void ContinueGrapple()
	{
		if ( grapplePoint == null ) return;

		var body = GetComponent<Rigidbody>();
		if ( body == null ) return;

		Vector3 toPoint = grapplePoint.Value - body.WorldPosition;
		float distance = toPoint.Length;

		if ( distance < StopDistance )
		{
			StopGrapple();
			return;
		}

		Vector3 direction = toPoint.Normal;

		float currentPull = PullStrength;
		if ( boostTimeRemaining > 0f )
		{
			currentPull *= BoostMultiplier;
		}

		body.Velocity += direction * currentPull * Time.Delta;

		// Remove any velocity that pulls away from the grapple point
		float awaySpeed = Vector3.Dot( body.Velocity, -direction );
		if ( awaySpeed > 0 )
		{
			body.Velocity += direction * awaySpeed;
		}

		// Line color: yellow = boosting, red = on cooldown, cyan = ready
		Color lineColor = Color.Cyan;
		if ( boostTimeRemaining > 0f ) lineColor = Color.Yellow;
		else if ( cooldownTimeRemaining > 0f ) lineColor = Color.Red;

		DebugOverlay.Line( body.WorldPosition, grapplePoint.Value, lineColor );
	}

	private void StopGrapple()
	{
		isGrappling = false;
		grapplePoint = null;
		boostTimeRemaining = 0f;
		// Intentionally do NOT reset cooldownTimeRemaining here,
		// so the cooldown persists even between swings
	}
}
