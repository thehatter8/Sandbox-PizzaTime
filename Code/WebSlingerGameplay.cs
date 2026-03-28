using Sandbox;
using System.Collections.Generic;

public sealed class WebSlingerGameplay : Component
{
	[Property] public int Duration { get; set; } = 600;
	[Property] public Model PizzaModel { get; set; }
	public int TimeRemaining { get; private set; }
	public Vector3 PizzaLocation { get; private set; }
	[Property] public WebSlingerWarmup Warmup { get; set; }
	public bool PlayerHasPizza { get; private set; } = false;
	[Property] public float PickupDistance { get; set; } = 100f;
	public bool IsStarted => _started;

	private float _startTime;
	private bool _started = false;
	private bool _finished = false;
	private GameObject _spawnedPizza;
	private DeliveryDestination _currentDestination;
	private struct DeliveryDestination
	{
		public string Name;
		public Vector3 Location;

		public DeliveryDestination( string name, Vector3 location )
		{
			Name = name;
			Location = location;
		}
	}
	private static readonly List<Vector3> PizzaSpots = new()
	{
		new Vector3( -1135f,  32f,  -10800f ),
		new Vector3( -1135f,  32f,  -10800f ),
		new Vector3( -1135f,  32f,  -10800f ),
		new Vector3( -1135f,  32f,  -10800f ),
		new Vector3( -1135f,  32f,  -10800f ),
		new Vector3( -1135f, -300f, -10800f ),
		new Vector3( -1135f, -300f, -10800f ),
		new Vector3( -1135f, -300f, -10800f ),
		new Vector3( -1135f, -300f, -10800f ),
		new Vector3( -1135f, -300f, -10800f ),
	};
	private static readonly List<DeliveryDestination> DeliveryDestinations = new()
	{
		new DeliveryDestination( "City Hall",        new Vector3(  500f,  200f, -11036f ) ),
		new DeliveryDestination( "Fire Station",     new Vector3( -800f,  150f, -11036f ) ),
		new DeliveryDestination( "Rooftop Apartment",new Vector3(  200f, -400f,  -9500f ) ),
		new DeliveryDestination( "Corner Shop",      new Vector3( -300f,  600f, -11036f ) ),
		new DeliveryDestination( "Police Station",   new Vector3(  900f, -100f, -11036f ) ),
	};
	protected override void OnStart()
	{
		_startTime = Time.Now;
		TimeRemaining = Duration;
	}
	protected override void OnUpdate()
	{
		if ( !_started )
		{
			if ( Warmup is null || !Warmup.IsFinished ) return;
			_started = true;
			_startTime = Time.Now;
			TimeRemaining = Duration;
			PickPizzaLocation();
			return;
		}
		if ( _started && !_finished && _spawnedPizza.IsValid() && Warmup.PizzaPlayer.IsValid() )
		{
			float dist = Vector3.DistanceBetween(
				Warmup.PizzaPlayer.GameObject.WorldPosition,
				PizzaLocation
			);
			if ( dist < PickupDistance && !PlayerHasPizza )
			{
				PlayerHasPizza = true;
				var renderer = _spawnedPizza.GetComponent<ModelRenderer>();
				if ( renderer is not null )
					renderer.Enabled = false;
				PickDeliveryDestination();
				Log.Info( "Pizza picked up!" );
			}
		}
		if ( _finished ) return;
		TimeRemaining = Duration - (int)(Time.Now - _startTime);
		if ( TimeRemaining <= 0 )
		{
			TimeRemaining = 0;
			_finished = true;
			Log.Info( "Gameplay timer finished!" );
		}
	}
	private void PickPizzaLocation()
	{
		var spot = PizzaSpots[Game.Random.Int( 0, PizzaSpots.Count - 1 )];
		PizzaLocation = spot;
		Log.Info( $"Pizza location set to: {PizzaLocation}" );
		if ( PizzaModel is null )
		{
			Log.Warning( "No model assigned to PizzaModel!" );
			return;
		}
		_spawnedPizza = new GameObject();
		_spawnedPizza.WorldPosition = PizzaLocation;
		_spawnedPizza.WorldRotation = Rotation.From( 0f, 0f, 90f );
		_spawnedPizza.WorldScale = Vector3.One * 2f;
		_spawnedPizza.Name = "PizzaModel";
		var renderer = _spawnedPizza.AddComponent<ModelRenderer>();
		renderer.Model = PizzaModel;
		renderer.RenderOptions.Overlay = true;
	}
	private void PickDeliveryDestination()
	{
		_currentDestination = DeliveryDestinations[Game.Random.Int( 0, DeliveryDestinations.Count - 1 )];
		Log.Info( $"Deliver to: {_currentDestination.Name} at {_currentDestination.Location}" );
	}
}
