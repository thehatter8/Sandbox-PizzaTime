using Sandbox;
using System.Collections.Generic;

public sealed class WebSlingerGameplay : Component
{
	[Property] public int Duration { get; set; } = 600;
	[Property] public Model PizzaModel { get; set; }
	[Property] public Model DestinationModel { get; set; }
	[Property] public WebSlingerWarmup Warmup { get; set; }

	[Property] public float PickupDistance { get; set; } = 100f;
	[Property] public float DeliveryDistance { get; set; } = 150f;

	public Vector3 CurrentDestination     => _currentDestination.Location;
	public string  CurrentDestinationName => _currentDestination.Name;
	public int TimeRemaining { get; private set; }
	public Vector3 PizzaLocation { get; private set; }
	public bool PlayerHasPizza { get; private set; } = false;

	public bool IsStarted => _started;

	private float _startTime;
	private bool _started = false;
	private bool _finished = false;

	private GameObject _spawnedPizza;
	private GameObject _spawnedDestination;

	private DeliveryDestination _currentDestination;
	private int _lastDestinationIndex = -1;
	public bool HasPizzaSpawned => _spawnedPizza.IsValid();
	public bool HasDestinationSpawned => _spawnedDestination.IsValid();
	private struct DeliveryDestination
	{
		public string Name;
		public Vector3 Location;
		public Rotation Rotation;

		public DeliveryDestination( string name, Vector3 location, Rotation rotation )
		{
			Name = name;
			Location = location;
			Rotation = rotation;
		}
	}

	private static readonly List<Vector3> PizzaSpots = new()
	{
		new Vector3( -1135f,  	32f,  	-11010f ),
		new Vector3( -6346,  	-1710f, -11010f ),
		new Vector3( 787f,  	-7711f, -11010f ),
		new Vector3( -12667f,  	-10544f,-11010f ),
		new Vector3( -1135f,  	32f,  	-11010f ), // keep
		new Vector3( -1135f, 	-300f, 	-11010f ), // keep
		new Vector3( -11485f, 	2622f, 	-11010f ),
		new Vector3( -8193f, 	7638f, 	-8706f ),
		new Vector3( 4610f, 	27f, 	-8514f ),
		new Vector3( -1135f, 	-300f, 	-11010f ),
		// Increase height from player height by about 25
	};
	private static readonly List<DeliveryDestination> DeliveryDestinations = new()
	{
		new DeliveryDestination( "High Rooftop",     new Vector3( 1922f,  -9360f, 548f ),      	Rotation.From( 0, 90, 0 ) ),
		new DeliveryDestination( "Factory Entrance", new Vector3( 12490f, -12156f, -11036f ),  	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Corner Apartment", new Vector3( -10760f, 3055f, -11036f ),   	Rotation.From( 0, 270, 0 ) ),
		new DeliveryDestination( "Back Alley",       new Vector3( -11710f, -11967f, -11036f ), 	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Sketchy Alley",    new Vector3( -8373f, 9534f, -11036f ), 	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Some Alley",    	 new Vector3( -8147f, -11745f, -11036f ), 	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Garbage Patch",    new Vector3( 242f, 11194f, -10745f ), 		Rotation.From( 0, 145, 0 ) ),
		new DeliveryDestination( "Courtyard",    	 new Vector3( -1183f, 4853f, -11036f ), 	Rotation.From( 0, 145, 0 ) ),
		new DeliveryDestination( "Sewer Entrance 1", new Vector3( 13100f, 4604f, -11320f ),    	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Sewer Entrance 2", new Vector3( 3290f, 2213f, -11320f ),     	Rotation.From( 0, 90, 0 ) ),
		new DeliveryDestination( "Sewer Entrance 3", new Vector3( 7806f, -13174f, -11316f ),   	Rotation.From( 0, 90, 0 ) ),
		new DeliveryDestination( "Sewer Entrance 4", new Vector3( 3292f, 13207f, -11316f ),   	Rotation.From( 0, 90, 0 ) ),
		new DeliveryDestination( "Shack 1", 		 new Vector3( 4428f, 5685f, -11036f ),     	Rotation.From( 0, 35, 0 ) ),
		new DeliveryDestination( "Warehouse 1", 	 new Vector3( 11521f, 1524f, -11036f ),    	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Warehouse 2", 	 new Vector3( 11235f, 6741f, -11036f ),    	Rotation.From( 0, 45, 0 ) ),
		new DeliveryDestination( "Skybridge 1", 	 new Vector3( 1919f, -11245f, -7788f ),    	Rotation.From( 0, 90, 0 ) ),
		new DeliveryDestination( "Skybridge 2", 	 new Vector3( -4379f, -9984f, -9372f ),    	Rotation.From( 0, 0, 0 ) ),
		new DeliveryDestination( "Some Apartments",  new Vector3( -7155f, -4441f, -10780f ),   	Rotation.From( 0, 180, 0 ) ),
		new DeliveryDestination( "Underground",      new Vector3( -9533f, -5304f, -11036f ),   	Rotation.From( 0, 45, 0 ) ),
		new DeliveryDestination( "Round Apartment",  new Vector3( -10995f, -11776f, -10140f ), 	Rotation.From( 0, 0, 0 ) ),
		new DeliveryDestination( "Square Apartment",  new Vector3( -8027f, -11390f, -9116f ), 	Rotation.From( 0, 0, 0 ) ),
	};

	protected override void OnStart()
	{
		_startTime = Time.Now;
		TimeRemaining = Duration;
	}

	protected override void OnUpdate()
	{
		// WAIT FOR WARMUP
		if ( !_started )
		{
			if ( Warmup is null || !Warmup.IsFinished ) return;

			_started = true;
			_startTime = Time.Now;
			TimeRemaining = Duration;

			PickPizzaLocation();
			return;
		}

		// PIZZA PICKUP
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
				if ( renderer != null )
					renderer.Enabled = false;

				PickDeliveryDestination();

				Log.Info( "Pizza picked up!" );
			}
		}

		// DELIVERY CHECK
		if ( _started && !_finished && PlayerHasPizza && _spawnedDestination.IsValid() && Warmup.PizzaPlayer.IsValid() )
		{
			float dist = Vector3.DistanceBetween(
				Warmup.PizzaPlayer.GameObject.WorldPosition,
				_currentDestination.Location
			);

			if ( dist < DeliveryDistance )
			{
				OnPizzaDelivered();
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

		// Cleanup old pizza
		if ( _spawnedPizza.IsValid() )
			_spawnedPizza.Destroy();

		_spawnedPizza = new GameObject();
		_spawnedPizza.WorldPosition = PizzaLocation;
		_spawnedPizza.WorldRotation = Rotation.From( 0f, 0f, 90f );
		_spawnedPizza.WorldScale = Vector3.One * 2f;
		_spawnedPizza.Name = "PizzaModel";

		var renderer = _spawnedPizza.AddComponent<ModelRenderer>();
		renderer.Model = PizzaModel;
	}

	private void PickDeliveryDestination()
	{
		int index;

		do
		{
			index = Game.Random.Int( 0, DeliveryDestinations.Count - 1 );
		}
		while ( index == _lastDestinationIndex );

		_lastDestinationIndex = index;
		_currentDestination   = DeliveryDestinations[index];

		Log.Info( $"Deliver to: {_currentDestination.Name} at {_currentDestination.Location}" );

		// Cleanup old destination
		if ( _spawnedDestination.IsValid() )
			_spawnedDestination.Destroy();

		if ( DestinationModel is null )
		{
			Log.Warning( "No model assigned to DestinationModel!" );
			return;
		}

		_spawnedDestination = new GameObject();
		_spawnedDestination.WorldPosition = _currentDestination.Location;
		_spawnedDestination.WorldRotation = _currentDestination.Rotation;
		_spawnedDestination.WorldScale = Vector3.One * 1.3f;
		_spawnedDestination.Name = "DeliveryDestination";
		//_spawnedDestination.Enabled = false;
		var renderer = _spawnedDestination.AddComponent<SkinnedModelRenderer>();
		renderer.Model = DestinationModel;
		//var dresser = _spawnedDestination.AddComponent<Dresser>();
		//dresser.BodyTarget = renderer;
		//dresser.Randomize();
		//dresser.Source = Dresser.ClothingSource.Randomize();
		
		//dresser.Apply();
	}

	private void OnPizzaDelivered()
	{
		Log.Info( "Pizza delivered!" );

		PlayerHasPizza = false;

		// Remove destination
		if ( _spawnedDestination.IsValid() )
			_spawnedDestination.Destroy();

		// Restart warmup
		if ( Warmup != null )
			Warmup.ResetWarmup();

		// Reset loop
		_started = false;
	}
}