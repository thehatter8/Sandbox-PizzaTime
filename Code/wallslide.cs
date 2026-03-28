using System;
using Sandbox;

public sealed class WallSlide : Component
{
	[Property] public PlayerController Controller { get; set; }

	[Property] public float WallSlideDistance { get; set; } = 10f;
	[Property] public float SlideDownSpeed { get; set; } = 60f;
	[Property] public float SlideAcceleration { get; set; } = 40f;

	[Property] public float MaxStamina { get; set; } = 100f;
	[Property] public float InitialStaminaCost { get; set; } = 10f;
	[Property] public float StaminaDrainPerSecond { get; set; } = 60f;
	[Property] public float StaminaRegenPerSecond { get; set; } = 15f;
	[Property] public float AirRegenDelay { get; set; } = 3f;

	public bool IsWallSliding { get; private set; }

	// 🔹 Make Stamina a Property so UI updates automatically
	[Property] public float Stamina { get; private set; }

	private Rigidbody _rigidBody;
	private float _currentSlideZ;
	private Vector3 _lastWallNormal;

	private bool _mustTouchGroundForRegen;
	private float _airRegenDelayTimer;

	protected override void OnStart()
	{
		if ( Controller is null )
			Controller = Components.Get<PlayerController>( FindMode.InSelf );

		_rigidBody = Components.Get<Rigidbody>( FindMode.InSelf );

		Stamina = MaxStamina;
	}

	protected override void OnUpdate()
	{
		if ( Controller is null ) return;

		if ( Controller.IsOnGround )
		{
			_mustTouchGroundForRegen = false;
			_airRegenDelayTimer = 0f;
		}
		else
		{
			if ( _mustTouchGroundForRegen && _airRegenDelayTimer > 0f )
			{
				_airRegenDelayTimer -= Time.Delta;
				if ( _airRegenDelayTimer <= 0f )
					_mustTouchGroundForRegen = false;
			}
		}
	}

	protected override void OnFixedUpdate()
	{
		if ( Controller is null || _rigidBody is null ) return;

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
				Stamina = Math.Max( 0f, Stamina - InitialStaminaCost );
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
}
