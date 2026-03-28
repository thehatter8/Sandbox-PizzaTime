using Sandbox;

public sealed class BunnyHop : Component
{
	[Property]
	public PlayerController Controller { get; set; }
	[Property, Range( 0f, 0.2f )]
	public float CoyoteTime { get; set; } = 0.05f;
	[Property]
	public float JumpVelocity { get; set; } = 300f;
	private TimeSince _timeSinceGrounded;

	protected override void OnStart()
	{
		if ( Controller is null )
		{
			Controller = Components.Get<PlayerController>( FindMode.InSelf );

			if ( Controller is null )
				Log.Warning( "BunnyHop: No PlayerController found on this GameObject." );
		}
	}

	protected override void OnUpdate()
	{
		if ( Controller is null )
			return;
		if ( Controller.IsOnGround )
			_timeSinceGrounded = 0f;
		bool jumpHeld = Input.Down( "Jump" );
		bool withinCoyote = _timeSinceGrounded <= CoyoteTime;
		if ( jumpHeld && withinCoyote )
		{
			Controller.Jump( Vector3.Up * JumpVelocity );
			_timeSinceGrounded = CoyoteTime + 1f;
		}
	}
}
