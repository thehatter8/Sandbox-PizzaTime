using Sandbox;

public sealed class WebSlinger : Component
{
	[Property] public float MaxDistance { get; set; } = 3500;
	[Property] public float PullStrength { get; set; } = 1000;
	[Property] public float StopDistance { get; set; } = 50f;
	[Property] public float BoostMultiplier { get; set; } = 3f;
	[Property] public float BoostDuration { get; set; } = 0.5f;
	[Property] public float BoostCooldown { get; set; } = 3f;

	// VerletRope tuning
	[Property] public int RopeSegments { get; set; } = 12;
	[Property] public float RopeDampingFactor { get; set; } = 0.1f;
	[Property] public float RopeStiffness { get; set; } = 0.8f;
	[Property] public float RopeSlack { get; set; } = 20f;

	private Vector3? grapplePoint;
	private bool isGrappling;
	private float boostTimeRemaining = 0f;
	private float cooldownTimeRemaining = 0f;

	// The VerletRope sits on a child GO at the player's position.
	// Its Attachment is a second child GO pinned to the grapple anchor.
	private GameObject ropeObject;
	private GameObject anchorObject;
	private VerletRope rope;
	private LineRenderer ropeRenderer;

	protected override void OnUpdate()
	{
		if ( !Input.Down( "attack1" ) )
		{
			StopGrapple();
			return;
		}

		if ( !isGrappling )
			StartGrapple();

		if ( boostTimeRemaining > 0f )
		{
			boostTimeRemaining -= Time.Delta;
			if ( boostTimeRemaining <= 0f )
				cooldownTimeRemaining = BoostCooldown;
		}

		if ( cooldownTimeRemaining > 0f )
			cooldownTimeRemaining -= Time.Delta;

		if ( Input.Pressed( "run" ) && isGrappling && boostTimeRemaining <= 0f && cooldownTimeRemaining <= 0f )
			boostTimeRemaining = BoostDuration;

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
		CreateRope();
	}

	private void CreateRope()
	{
		var body = GetComponent<Rigidbody>();
		if ( body == null || grapplePoint == null ) return;

		// Anchor GO: a world-space child pinned at the hit point
		anchorObject = new GameObject( true, "WebAnchor" );
		anchorObject.WorldPosition = grapplePoint.Value;

		// Rope GO: starts at the player body, VerletRope simulates toward anchor
		ropeObject = new GameObject( true, "WebRope" );
		ropeObject.WorldPosition = body.WorldPosition;

		rope = ropeObject.Components.Create<VerletRope>();
		rope.Attachment = anchorObject;
		rope.SegmentCount = RopeSegments;
		rope.DampingFactor = RopeDampingFactor;
		rope.Stiffness = RopeStiffness;
		rope.Slack = RopeSlack;
		// LengthOverride 0 = use distance between the two points automatically
		rope.LengthOverride = 0f;

		// Wire up a LineRenderer for colour feedback
		ropeRenderer = ropeObject.Components.Create<LineRenderer>();
		rope.LinkedRenderer = ropeRenderer;
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

		// Pull player toward anchor
		Vector3 direction = toPoint.Normal;
		float currentPull = boostTimeRemaining > 0f ? PullStrength * BoostMultiplier : PullStrength;
		body.Velocity += direction * currentPull * Time.Delta;

		// Cancel velocity moving away from anchor
		float awaySpeed = Vector3.Dot( body.Velocity, -direction );
		if ( awaySpeed > 0 )
			body.Velocity += direction * awaySpeed;

		// Keep rope start tracking the player each frame
		if ( ropeObject != null )
		{
			ropeObject.WorldPosition = body.WorldPosition;

			// Update rope length to match actual distance so it doesn't pile up
			if ( rope != null )
				rope.LengthOverride = toPoint.Length;
		}

		// Update rope colour via the LinkedRenderer
		if ( ropeRenderer != null )
		{
			ropeRenderer.Color = boostTimeRemaining > 0f
				? Color.Yellow
				: cooldownTimeRemaining > 0f
					? Color.Red
					: Color.Cyan;
		}
	}

	private void StopGrapple()
	{
		isGrappling = false;
		grapplePoint = null;
		boostTimeRemaining = 0f;
		DestroyRope();
	}

	private void DestroyRope()
	{
		ropeObject?.Destroy();
		anchorObject?.Destroy();
		ropeObject = null;
		anchorObject = null;
		rope = null;
		ropeRenderer = null;
	}

	protected override void OnDisabled() => StopGrapple();
	protected override void OnDestroy() => DestroyRope();
}