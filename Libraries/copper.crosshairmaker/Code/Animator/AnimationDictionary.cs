using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Sandbox;

#nullable enable
namespace CrosshairMaker.Animator
{
	public sealed class AnimationDictionary : IDictionary<int,CrosshairAnimation> , IDictionary
	{
		//Init
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public void Init(AnimationValueSetter setterCollection )
		{
			if (AnimationSize.Length != 0) this.Clear();
			ValueSetters = setterCollection;

		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Init

		//General features
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		/// <summary>
		/// Add an animation the same size as the others in this AnimationDictionary with given key , returns the key if sucessful
		/// throws an error if AnimationDictionary is empty
		/// throws an error if key is already contained in this AnimationDictionary
		/// </summary>
		/// <param name="key">key of animation</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public int AddNewAnimation( int key )
		{
			if ( AnimationSize.Length == 0 ) throw new InvalidOperationException( "Cannot create an animation, no setters found" );
			if ( _internalDic.ContainsKey( key ) ) throw new InvalidOperationException( $"Cannot create an animation in an already existing key, animation {key} is {_internalDic[key]}" );
			CrosshairAnimation newAnim = CrosshairAnimation.FromPropertyCount( AnimationSize );
			_internalDic.Add( key, newAnim );
			return key;
		}
		/// <summary>
		/// Add an animation the same size as the others in this AnimationDictionary in first avaliable key between 0 -> (int.MaxValue - 1) , returns the key if sucessful
		/// throws an exception if AnimationDictionary is empty
		/// throws an exception if all keys from 0 to (int.MaxValue - 1) are filled
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public int AddNewAnimation()
		{
			_internalDic ??= new();
			if ( AnimationSize.Length == 0 ) throw new InvalidOperationException( "Cannot create an animation, no setters found" );
			for ( int i = 0; true; i++ )
			{
				if ( i == int.MaxValue ) throw new InvalidOperationException( $"AnimationDictionary could not find a key smaller than {int.MaxValue} to add a new animation" );
				if ( _internalDic.ContainsKey( i ) ) continue;
				CrosshairAnimation newAnim = CrosshairAnimation.FromPropertyCount( AnimationSize );
				_internalDic.Add( i, newAnim ); //Skips the implemented Add method, since both validity conditions are met
				return i;
			}
		}
		/// <summary>
		/// Returns the expected animation size based on the setters
		/// </summary>
		public int[] AnimationSize => ValueSetters?.Sizes ?? Array.Empty<int>();
		public override string ToString()
		{
			int[] s = AnimationSize;
			return (s.Length == 3) ? 
				$"AnimationSize : [float:{s[0]}, int:{s[1]}, Color:{s[2]}]" : 
				"No AnimationSize set : length = " + s.Length;
		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//General features
		//Overriden Dictionary behaviour
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public void Add( int key, CrosshairAnimation value )
		{
			_internalDic ??= new Dictionary<int,CrosshairAnimation>();
			if ( CrosshairAnimation.SameSize( AnimationSize,value )) _internalDic.Add( key, value );
		}
		public CrosshairAnimation this[int key]
		{
			get => _internalDic[key];
			set
			{
				if ( CrosshairAnimation.SameSize( value,AnimationSize ) ) _internalDic[key] = value;
				else throw new InvalidOperationException( "Cannot insert an animation of a different size from the others" );
			}
		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Overriden Dictionary behaviour

		//Editor features
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
#pragma warning disable
#pragma warning enable
		public string[] FloatsPropertyNames;
		public string[] IntsPropertyNames;
		public string[] ColorsPropertyNames;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Editor features

		//Internal properties
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public AnimationValueSetter? ValueSetters { get; private set; }
		private Dictionary<int, CrosshairAnimation> _internalDic;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Internal properties

		//Unchanged IDictionary Implementations
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public void Add( KeyValuePair<int, CrosshairAnimation> item ) => Add( item.Key, item.Value );
		public bool ContainsKey( int key ) => _internalDic.ContainsKey( key );
		public bool Remove( int key ) => _internalDic.Remove( key );
		public bool TryGetValue( int key, [MaybeNullWhen( false )] out CrosshairAnimation value ) => _internalDic.TryGetValue( key, out value );
		public void Clear() => _internalDic.Clear();
		public bool Contains( KeyValuePair<int, CrosshairAnimation> item ) => _internalDic.Contains( item );
		private void CopyTo( KeyValuePair<int, CrosshairAnimation>[] array, int index )
		{
			if ( array == null ) throw new ArgumentNullException();
			if ( (uint)index > (uint)array.Length ) throw new ArgumentOutOfRangeException( "Array is too short to handle offset" );
			if ( array.Length - index < Count ) throw new ArgumentOutOfRangeException( "Array is too short to handle offset" );

			foreach ( var kvp in _internalDic )
			{
				array[index++] = new KeyValuePair<int, CrosshairAnimation>( kvp.Key, kvp.Value );
			}
		}
		public bool Remove( KeyValuePair<int, CrosshairAnimation> item )
		{
			bool success = _internalDic.TryGetValue( item.Key, out CrosshairAnimation correct );
			if ( !success ) return false;
			success = correct == item.Value;
			if ( success ) _internalDic.Remove( item.Key );
			return success;
		}
		public IEnumerator<KeyValuePair<int, CrosshairAnimation>> GetEnumerator() => _internalDic.GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		void ICollection<KeyValuePair<int, CrosshairAnimation>>.CopyTo( KeyValuePair<int, CrosshairAnimation>[] array, int arrayIndex ) => CopyTo( array, arrayIndex );

		public void Add( object key, object value )
		{
			if(key is not int ik) throw new InvalidCastException();
			if(value is not CrosshairAnimation av) throw new InvalidCastException();
			Add( ik, av );
		}

		public bool Contains( object key ) => key is int ik ? _internalDic.ContainsKey( ik ) : false;

		IDictionaryEnumerator IDictionary.GetEnumerator() => _internalDic.GetEnumerator();

		public void Remove( object key )
		{
			if( key is int ik) _internalDic.Remove( ik );
		}

		public void CopyTo( Array array, int index )
		{
			if(array == null) throw new ArgumentNullException();
			if(array.Rank != 1) throw new ArgumentException("Array cannot be multi-dimentional");
			if ( array.GetLowerBound( 0 ) != 0 ) throw new ArgumentException( "Array's lower bound must be 0" );
			if ( (uint)index > (uint)array.Length ) throw new ArgumentOutOfRangeException( "Array is too short to handle offset" );
			if ( array.Length - index < Count ) throw new ArgumentOutOfRangeException( "Array is too short to handle offset" );

			if ( array is KeyValuePair<int, CrosshairAnimation>[] kvpArr )
			{
				CopyTo( kvpArr, 0 );
			}
			else if ( array is DictionaryEntry[] deArr )
			{
				foreach(var kvp in _internalDic )
				{
					deArr[index++] = new DictionaryEntry( kvp.Key, kvp.Value );
				}
			}
			else if ( array is object[] objArr )
			{

			}
			else throw new ArgumentException( "Array type is not compatible" );

		}

		public ICollection<int> Keys => _internalDic.Keys;
		public ICollection<CrosshairAnimation> Values => _internalDic.Values;
		public int Count => _internalDic.Count;
		public bool IsReadOnly => false;

		public bool IsFixedSize => false;

		ICollection IDictionary.Keys => _internalDic.Keys;

		ICollection IDictionary.Values => _internalDic.Values;

		public bool IsSynchronized => false;

		public object SyncRoot => _internalDic;

		public object this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Unchanged IDictionary Implementations
	}
}
