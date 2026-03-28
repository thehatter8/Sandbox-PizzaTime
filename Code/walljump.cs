using System;
using Sandbox;

public sealed class WallJumpSlide : Component
{
	[Property] public PlayerController Controller { get; set; }
	[Property] public float WallJumpVelocityMultiplier { get; set; } = 400f;
	[Property] public float WallJumpUpVelocity { get; set; } = 300f;
	[Property] public int MaxWallJumps { get; set; } = 5;
	[Property] public float AirRegenDelay { get; set; } = 0.3f;
	[Property] public float WallSlideDistance { get; set; } = 10f;
	[Property] public float SlideDownSpeed { get; set; } = 60f;
	[Property] public float SlideAcceleration { get; set; } = 40f;
	public bool IsWallSliding { get; private set; }
	[Property] public float Stamina { get; private set; } = 100f;
	[Property] public float MaxStamina { get; set; } = 100f;
	[Property] public float StaminaDrainPerSecond { get; set; } = 60f;
	[Property] public float StaminaRegenPerSecond { get; set; } = 15f;

	private Rigidbody _rigidBody;
	private float _currentSlideZ;
	private Vector3 _lastWallNormal;
	private bool _mustTouchGroundForRegen;
	private float _airRegenDelayTimer;
	private int _wallJumpCount;
	private bool _mustTouchGroundForWallJumpReset;
	private float _wallJumpAirResetTimer;
	private bool _wallJumpReleaseActive;
	private float _wallJumpReleaseTimer;
	[Property] public float WallJumpReleaseDuration { get; set; } = 0.15f;
	protected override void OnStart()
	{
		if ( Controller == null )
			Controller = Components.Get<PlayerController>( FindMode.InSelf );
		_rigidBody = Components.Get<Rigidbody>( FindMode.InSelf );
		Stamina = MaxStamina;
	}
	protected override void OnUpdate()
	{
		if ( Controller == null ) return;
		if ( Controller.IsOnGround )
		{
			_mustTouchGroundForRegen = false;
			_airRegenDelayTimer = 0f;

			_wallJumpCount = 0;
			_mustTouchGroundForWallJumpReset = false;
			_wallJumpAirResetTimer = 0f;
		}
		else
		{
			if ( _mustTouchGroundForRegen && _airRegenDelayTimer > 0f )
			{
				_airRegenDelayTimer -= Time.Delta;
				if ( _airRegenDelayTimer <= 0f )
					_mustTouchGroundForRegen = false;
			}
			if ( _mustTouchGroundForWallJumpReset && _wallJumpAirResetTimer > 0f )
			{
				_wallJumpAirResetTimer -= Time.Delta;
				if ( _wallJumpAirResetTimer <= 0f )
					_wallJumpCount = 0;
			}
		}
		if ( Input.Pressed( "jump" ) )
		{
			TryWallJump();
		}
		if ( _wallJumpReleaseActive )
		{
			_wallJumpReleaseTimer -= Time.Delta;
			if ( _wallJumpReleaseTimer <= 0f )
				_wallJumpReleaseActive = false;
		}
	}
	protected override void OnFixedUpdate()
	{
		if ( Controller == null || _rigidBody == null ) return;

		if ( IsWallSliding && Stamina <= 0f )
		{
			StopSlide();
			return;
		}
		RegenStamina();
		if ( Controller.IsOnGround )
		{
			StopSlide();
			return;
		}
		if ( _wallJumpReleaseActive ) return;
		var forward = Transform.Rotation.Forward;
		var right = Transform.Rotation.Right;
		var traceForward = DoTrace( forward );
		var traceBackward = DoTrace( -forward );
		var traceRight = DoTrace( right );
		var traceLeft = DoTrace( -right );
		SceneTraceResult validTrace = default;
		if ( IsValidTrace( traceForward ) ) validTrace = traceForward;
		else if ( IsValidTrace( traceBackward ) ) validTrace = traceBackward;
		else if ( IsValidTrace( traceRight ) ) validTrace = traceRight;
		else if ( IsValidTrace( traceLeft ) ) validTrace = traceLeft;
		bool shiftHeld = Input.Down( "Run" );
		bool wallFound = validTrace.Hit;
		if ( wallFound && shiftHeld && Stamina > 0f )
		{
			if ( !IsWallSliding )
			{
				IsWallSliding = true;
				_currentSlideZ = 0f;
				Stamina = Math.Max( 0f, Stamina - 10f );
			}
			_currentSlideZ = Math.Min( _currentSlideZ + SlideAcceleration * Time.Delta, SlideDownSpeed );
			_lastWallNormal = validTrace.Normal;
			var horizontal = StripWallPenetration( _rigidBody.Velocity.WithZ( 0 ), validTrace.Normal );
			_rigidBody.Velocity = horizontal.WithZ( -_currentSlideZ );
			Stamina = Math.Max( 0f, Stamina - StaminaDrainPerSecond * Time.Delta );
		}
		else
		{
			if ( IsWallSliding )
				StopSlide();
		}
	}
	private SceneTraceResult DoTrace( Vector3 dir )
	{
		var start = Transform.Position + Vector3.Up * 40f;
		var end = start + dir * WallSlideDistance;
		return Scene.Trace.Ray( start, end )
			.Size( 8f )
			.IgnoreGameObjectHierarchy( GameObject )
			.WithoutTags( "player" )
			.Run();
	}
	private static bool IsValidTrace( SceneTraceResult trace )
	{
		if ( !trace.Hit ) return false;
		float verticalDot = Math.Abs( Vector3.Dot( trace.Normal, Vector3.Up ) );
		return verticalDot < 0.3f;
	}
	private static Vector3 StripWallPenetration( Vector3 horizontal, Vector3 wallNormal )
	{
		float intoWall = Vector3.Dot( horizontal, -wallNormal );
		if ( intoWall > 0f )
			horizontal += wallNormal * intoWall;
		return horizontal;
	}
	private void StopSlide()
	{
		if ( !IsWallSliding ) return;
		IsWallSliding = false;
		_currentSlideZ = 0f;
		_mustTouchGroundForRegen = true;
		_airRegenDelayTimer = AirRegenDelay;
		if ( _lastWallNormal.Length > 0f )
		{
			var pushForce = _lastWallNormal.WithZ( 0 ) * 120f;
			_rigidBody.Velocity += pushForce;
		}
	}
	private void RegenStamina()
	{
		if ( _mustTouchGroundForRegen ) return;
		if ( !IsWallSliding )
			Stamina = Math.Min( MaxStamina, Stamina + StaminaRegenPerSecond * Time.Delta );
	}
	private bool CanWallJumpNow()
	{
		return _wallJumpCount < MaxWallJumps;
	}
	private bool TryWallJump()
	{
		if ( !CanWallJumpNow() ) return false;
		if ( _lastWallNormal.Length == 0f ) return false;
		_wallJumpReleaseActive = true;
		_wallJumpReleaseTimer = WallJumpReleaseDuration;
		var forward = Transform.Rotation.Forward;
		var wallPlaneForward = forward - Vector3.Dot( forward, _lastWallNormal ) * _lastWallNormal;
		wallPlaneForward = wallPlaneForward.Normal;
		var jumpVel = wallPlaneForward * WallJumpVelocityMultiplier;
		jumpVel.z = WallJumpUpVelocity;
		_rigidBody.Velocity += jumpVel;
		_wallJumpCount++;
		_mustTouchGroundForWallJumpReset = true;
		_wallJumpAirResetTimer = AirRegenDelay;
		_lastWallNormal = Vector3.Zero;
		Log.Info( $"Walljump executed! Count: {_wallJumpCount}" );
		return true;
	}
}
