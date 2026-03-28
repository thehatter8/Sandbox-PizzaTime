using Sandbox;

public sealed class ScrollWheelJump : Component
{
	[Property] public float JumpStrength { get; set; } = 300f;

	protected override void OnUpdate()
	{
		if ( Input.MouseWheel.y != 0 )
		{
			Log.Info( "Scroll detected" );
			var character = GetComponentInParent<CharacterController>();
			var body = GetComponentInChildren<Rigidbody>();
			if ( character is null ) return;
			if ( !character.IsOnGround ) return;
			body.Velocity = body.Velocity.WithZ( JumpStrength );
			character.Punch( Vector3.Up * JumpStrength );
		}
	}
}
