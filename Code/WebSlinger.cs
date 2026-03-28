using Sandbox;

public sealed class WebSlinger : Component
{
    [Property] public float MaxDistance { get; set; } = 5000f;
    [Property] public float PullStrength { get; set; } = 3000f;
    [Property] public float StopDistance { get; set; } = 50f;

    private Vector3? grapplePoint;
    private bool isGrappling;

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

		body.Velocity += direction * PullStrength * Time.Delta;

		// Remove any velocity that pulls away from the grapple point
		float awaySpeed = Vector3.Dot( body.Velocity, -direction );
		if ( awaySpeed > 0 )
		{
			body.Velocity += direction * awaySpeed;
		}

		DebugOverlay.Line( body.WorldPosition, grapplePoint.Value, Color.Cyan );
	}

	private void StopGrapple()
    {
        isGrappling = false;
        grapplePoint = null;
    }
}
