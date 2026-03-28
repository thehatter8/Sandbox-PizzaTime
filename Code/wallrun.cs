using System;
using Sandbox;

/// <summary>
/// Wall-slide component — hold Shift while airborne next to a wall to slide down it.
/// Attach to the same GameObject as your PlayerController.
/// </summary>
public sealed class WallSlide : Component
{
	// -------------------------------------------------------------------------
	// Inspector properties
	// -------------------------------------------------------------------------

	[Property]
	public PlayerController Controller { get; set; }

	[Property]
	public float WallSlideDistance { get; set; } = 10f;

	[Property]
	public float SlideDownSpeed { get; set; } = 60f;

	[Property]
	public float SlideAcceleration { get; set; } = 40f;

	[Property]
	public float MaxStamina { get; set; } = 100f;

	[Property]
	public float InitialStaminaCost { get; set; } = 10f;

	[Property]
	public float StaminaDrainPerSecond { get; set; } = 60f;

	[Property]
	public float StaminaRegenPerSecond { get; set; } = 15f;

	// -------------------------------------------------------------------------
	// Public read-only state
	// -------------------------------------------------------------------------

	public bool IsWallSliding { get; private set; }
	public float Stamina { get; private set; }

	// -------------------------------------------------------------------------
	// Private state
	// -------------------------------------------------------------------------

	private Rigidbody _rigidBody;
	private float _currentSlideZ;

	// -------------------------------------------------------------------------
	// Lifecycle
	// -------------------------------------------------------------------------

	protected override void OnStart()
	{
		if ( Controller is null )
		{
			Controller = Components.Get<PlayerController>( FindMode.InSelf );
			if ( Controller is null )
				Log.Warning( "WallSlide: No PlayerController found on this GameObject." );
		}

		_rigidBody = Components.Get<Rigidbody>( FindMode.InSelf );
		if ( _rigidBody is null )
			Log.Warning( "WallSlide: No Rigidbody found on this GameObject." );

		Stamina = MaxStamina;
	}

	protected override void OnFixedUpdate()
	{
		if ( Controller is null || _rigidBody is null ) return;

		// -----------------------------------------------------------------
		// HARD STOP: prevent sliding at 0 stamina
		// -----------------------------------------------------------------
		if ( IsWallSliding && Stamina <= 0f )
		{
			StopSlide();
			return;
		}

		RegenStamina();

		// Must be airborne
		if ( Controller.IsOnGround )
		{
			StopSlide();
			return;
		}

		// -----------------------------------------------------------------
		// WALL DETECTION (player-relative, all directions)
		// -----------------------------------------------------------------

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

		// -----------------------------------------------------------------
		// WALL SLIDE LOGIC
		// -----------------------------------------------------------------

		if ( wallFound && shiftHeld && Stamina > 0f )
		{
			// Start slide
			if ( !IsWallSliding )
			{
				IsWallSliding = true;
				_currentSlideZ = 0f;
				Stamina = Math.Max( 0f, Stamina - InitialStaminaCost );
			}

			// Accelerate downward
			_currentSlideZ = Math.Min(
				_currentSlideZ + SlideAcceleration * Time.Delta,
				SlideDownSpeed
			);

			// Apply velocity
			var horizontal = StripWallPenetration(
				_rigidBody.Velocity.WithZ( 0 ),
				validTrace.Normal
			);

			_rigidBody.Velocity = horizontal.WithZ( -_currentSlideZ );

			// Drain stamina
			Stamina = Math.Max( 0f, Stamina - StaminaDrainPerSecond * Time.Delta );
		}
		else
		{
			if ( IsWallSliding )
			{
				StopSlide();
			}
		}
	}

	// -------------------------------------------------------------------------
	// Helpers
	// -------------------------------------------------------------------------

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

		Log.Info( "Wall Slide Stopped" );
	}

	private void RegenStamina()
	{
		if ( !IsWallSliding )
		{
			Stamina = Math.Min(
				MaxStamina,
				Stamina + StaminaRegenPerSecond * Time.Delta
			);
		}
	}
}
