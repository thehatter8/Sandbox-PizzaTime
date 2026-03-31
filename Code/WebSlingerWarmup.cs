using Sandbox;
using System.Collections.Generic;
using System.Linq;

public sealed class WebSlingerWarmup : Component
{
	[Property] public int Duration { get; set; } = 10;
	[Property] public int TimeRemaining { get; private set; }
	[Property] public SoundEvent PizzaSound { get; set; }

	public PlayerController PizzaPlayer { get; private set; }
	public bool ShowPizzaText { get; private set; } = false;
	public bool IsFinished => _finished;

	private float _startTime;
	private float _postPickTimer = -1f;
	private bool _finished = false;

	public List<PlayerController> GetAlivePlayers()
	{
		return Scene.GetAllComponents<PlayerController>()
			.Where( p => p.IsValid() )
			.ToList();
	}

	protected override void OnStart()
	{
		_startTime = Time.Now;
		TimeRemaining = Duration;
	}

	protected override void OnUpdate()
	{
		// AFTER FINISHED (UI TIMER CLEANUP)
		if ( _finished )
		{
			if ( _postPickTimer >= 0f )
			{
				_postPickTimer -= Time.Delta;

				if ( _postPickTimer <= 0f )
					ShowPizzaText = false;
			}

			return;
		}

		TimeRemaining = Duration - (int)(Time.Now - _startTime);

		if ( TimeRemaining <= 0 )
		{
			TimeRemaining = 0;
			_finished = true;

			Log.Info( "Warmup finished!" );

			var alive = GetAlivePlayers();

			Log.Info( $"Alive players ({alive.Count}): {string.Join( ", ", alive.Select( p => p.GameObject.Name ) )}" );

			if ( alive.Count > 0 )
			{
				PizzaPlayer = alive[Game.Random.Int( 0, alive.Count - 1 )];

				Log.Info( $"Pizza Time! {PizzaPlayer.GameObject.Name} has been picked!" );

				ShowPizzaText = true;
				_postPickTimer = 5f;

				Sound.Play( PizzaSound );
			}
		}
	}

	// 🔁 CALLED BY GAMEPLAY AFTER DELIVERY
	public void ResetWarmup()
	{
		Log.Info( "Warmup restarting..." );

		_finished = false;
		_startTime = Time.Now;
		TimeRemaining = Duration;

		PizzaPlayer = null;
		ShowPizzaText = false;
		_postPickTimer = -1f;
	}
}