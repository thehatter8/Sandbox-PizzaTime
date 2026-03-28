using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

using SandEase = Sandbox.Utility.Easing;
#nullable enable
namespace CrosshairMaker.Animator
{
	[Icon( "auto_awesome_motion" )]
	public sealed partial class CrosshairAnimator : Component
	{
		//static
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		private const string GroupAnim = "Animations";
		private const string GroupProperties = "Properties";
		private const string GroupCrossInfo = "Crosshair info";
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//static

		//Events
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public delegate void CrosshairAnimatorEvent(object sender);
		/// <summary>
		/// Event fired when animation starts
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationStarting;
		/// <summary>
		/// Event fired when animation ends
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationEnd;
		/// <summary>
		/// Event fired when animation starts to reset (when ResetAnimation is called)
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationReseting;
		/// <summary>
		/// Event fired when animation is done reseting
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationReset;
		/// <summary>
		/// Event fired when animation or PlaybackSpeed is changed before completing, the old Progress will still be used to complete the new animation
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationInterupted;
		/// <summary>
		/// Event fired when the animation is interrupted and both Progress and Playbackspeed are reset
		/// </summary>
		public event CrosshairAnimatorEvent? OnAnimationCancelled;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Events

		//Actions
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		/// <summary>
		/// Scans the current GameObject for a IAnimatableCrosshair component
		/// </summary>
		[Button( "Scan GO. for Crosshair", "my_location" ), Group( GroupCrossInfo )]
		public void ScanForCrosshair()
		{
			IEnumerable<IAnimatableCrosshair> crosshairs = this.GameObject.GetComponents<IAnimatableCrosshair>();
			int count = crosshairs.Count();
			if ( count == 0 )
			{
				Target = null;
				return;
			}
			_scanIndex = (_scanIndex + 1) % count;
			Target = crosshairs.ElementAt( _scanIndex );
		}
		private int _scanIndex = 0;
		/// <summary>
		/// Adds a new configurable animation to the crosshair
		/// </summary>
		[Button("Add CrosshairAnimation", "library_add" ),Group(GroupAnim)]
		[HideIf(nameof(Target),null)]
		private void AddAnimation() => AnimationIndex = AnimationData?.AddNewAnimation() ?? 0;
		[Button("Delete CrosshairAnimation","delete"),Group(GroupAnim)]
		[HideIf(nameof(_hideDelButton),true)]
		private void DelAnimation()
		{
			if ( AnimationData == null ) return;
			AnimationData.Remove( AnimationIndex );
			if ( AnimationData.Count == 0 ) AnimationIndex = 0;
			AnimationIndex = AnimationData.First().Key;
		}
		private bool _hideDelButton => (AnimationData == null) ? true : AnimationData.Count <= 1;
		[Button( "Play anim", "my_location" ), Group( GroupAnim )]
		[ShowIf( nameof( _gameIsRunning ), true )]
		public void PlayAnimation()
		{
			if ( ProgressMultiplier == 0 ) Log.Warning( "Progress cap is 0, this prevents the animation from playing" );
			else if ( float.IsNaN(ProgressMultiplier) ) Log.Warning( "Progress cap is NaN, this prevents the animation from playing" );
			if ( float.IsNaN( _progress ) ) _progress = 0;
			if ( _progress == 0 ) UpdateStartingProperties();
			if(AnimationDuration == 0) PlaybackSpeed = float.MaxValue;
			else PlaybackSpeed = 1 / AnimationDuration;
			OnAnimationStarting?.Invoke( this );
		}
		public void ResetAnimation()
		{
			if ( AnimationDuration == 0 ) PlaybackSpeed = float.MinValue;
			if ( float.IsNaN( _progress ) ) _progress = 1;
			PlaybackSpeed = 1 / - AnimationResetDuration;
			OnAnimationReseting?.Invoke( this );
		}
		public void CancelAnimation()
		{
			PlaybackSpeed = 0;
			_progress = 0;
			OnAnimationCancelled?.Invoke( this );
		}
		private bool _gameIsRunning => Game.IsPlaying;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Actions

		//Properties
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		/// <summary>
		/// Current animation Index
		/// </summary>
		[Property, Group( GroupAnim )]
		public int AnimationIndex
		{
			get => _animationIndex;
			set
			{
				if ( Target == null )
				{
					_animationIndex = 0;
					return;
				}
				_animationIndex = value;
			}
		}
		private int _animationIndex = 0;
		/// <summary>
		/// Animation progress from 0 to 1
		/// </summary>
		[Property, Group( GroupAnim )]
		[HideIf(nameof(_gameIsRunning),false)]
		[ReadOnly]
		public float Progress
		{
			get
			{
				switch ( Easing )
				{
					default:
					case EasingMode.Linear: return _progress * ProgressMultiplier;
					case EasingMode.QuadOut: return SandEase.EaseOut( _progress ) * ProgressMultiplier;
					case EasingMode.QuadIn: return SandEase.EaseIn( _progress ) * ProgressMultiplier;
					case EasingMode.QuadInOut: return SandEase.EaseInOut( _progress ) * ProgressMultiplier;
					case EasingMode.BounceIn: return SandEase.BounceIn( _progress ) * ProgressMultiplier;
					case EasingMode.BounceOut: return SandEase.BounceOut( _progress ) * ProgressMultiplier;
					case EasingMode.BounceInOut: return SandEase.BounceInOut( _progress ) * ProgressMultiplier;
					case EasingMode.ExpIn: return SandEase.ExpoIn( _progress ) * ProgressMultiplier;
					case EasingMode.ExpOut: return SandEase.ExpoOut( _progress ) * ProgressMultiplier;
					case EasingMode.ExpInOut: return SandEase.ExpoInOut( _progress ) * ProgressMultiplier;
					case EasingMode.SinIn: return SandEase.SineEaseIn( _progress ) * ProgressMultiplier;
					case EasingMode.SinOut: return SandEase.SineEaseOut( _progress ) * ProgressMultiplier;
					case EasingMode.SinInOut: return SandEase.SineEaseInOut( _progress ) * ProgressMultiplier;
				}
			}
		}
		private float _progress = 0f;
		/// <summary>
		/// Sets the progress multiplier to change the animation amplitude, default is 1
		/// </summary>
		[Property, Group( GroupAnim )]
		[Range( 0f, 2f, 0.05f )]
		public float ProgressMultiplier
		{
			get => _progressCap;
			set => _progressCap = ( float.IsNegative(value) )? (value *= -1) : value;
		}
		private float _progressCap = 1f;
		/// <summary>
		/// Easing of animation, used to calculate Progress
		/// </summary>
		[Property, Group( GroupAnim )]
		public EasingMode Easing { get => _easing; set => _easing = value; }
		private EasingMode _easing = EasingMode.Linear;
		public enum EasingMode : byte
		{
			Linear,
			QuadIn,
			QuadOut,
			QuadInOut,
			BounceIn,
			BounceOut,
			BounceInOut,
			ExpIn,
			ExpOut,
			ExpInOut,
			SinIn,
			SinOut,
			SinInOut,
		}
		/// <summary>
		/// Duration in seconds for progress to go from 0 to 1 when PlayAnimation is called
		/// </summary>
		[Property, Group( GroupAnim )]
		public float AnimationDuration = 0.075f;
		/// <summary>
		/// Duration in seconds for the between the end of the animation and calling ResetAnimation, if set to -1 ResetAnimation is never called automatically
		/// </summary>
		[Property, Group( GroupAnim )]
		public float AnimationWait = 0.025f;
		/// <summary>
		/// Duration in seconds for progress to go from 1 to 0 when ResetAnimation is called
		/// </summary>
		[Property, Group( GroupAnim )]
		public float AnimationResetDuration = 0.5f;
		/// <summary>
		/// Current playback speed
		/// </summary>
		[Property, Group( GroupAnim )]
		[ReadOnly]
		[HideIf(nameof(_gameIsRunning),false)]
		public float PlaybackSpeed
		{
			get => _playbackSpeed;
			private set
			{
				if ( _playbackSpeed != 0 && _playbackSpeed != value )
				{
					OnAnimationInterupted?.Invoke( this );
				}
				_playbackSpeed = value;
			}
		}
		private float _playbackSpeed = 0;
		/// <summary>
		/// The crosshair animated by this component
		/// </summary>
		[Property, Group( GroupCrossInfo )]
		public IAnimatableCrosshair? Target 
		{
			get => _target; 
			set
			{
				if ( _target == value ) return;
				if( value == null )
				{
					if ( _target is Component chComp )
						chComp.OnComponentDestroy -= _OnDelCallback;
					_target = null;
					_animationData = null;
				}
				else
				{
					if ( _target is Component oldComp )
						oldComp.OnComponentDestroy -= _OnDelCallback;
					if ( value is Component newComp )
						newComp.OnComponentDestroy += _OnDelCallback;

					_target = value;
					_animationData = value.GetAnimationDictionary();
				}
				UpdateStartingProperties();
				
				void _OnDelCallback() => this.Target = null;
				
			} 
		}
		public IAnimatableCrosshair? _target = null;

		/// <summary>
		/// Target as string
		/// </summary>
		[Property, Group( GroupCrossInfo )]
		[ReadOnly]
		public string CurrentCrosshair => Target?.ToString() ?? "No crosshair found";

		/// <summary>
		/// Target id
		/// </summary>
		[Property, Group( GroupCrossInfo )]
		[ReadOnly]
		public string CurrentCrosshairID => (Target is not Component tc) ? "null" : tc.Id.ToString();
		/// <summary>
		/// AnimationDictionary
		/// </summary>
		[Property, Group( GroupCrossInfo ),Title("Animation data")]
		[HideIf(nameof(AnimationData),null)]
		[ReadOnly]
		public string _AnimationDataStr => AnimationData?.ToString() ?? "null";
		public AnimationDictionary? AnimationData => _animationData;
		public AnimationDictionary? _animationData = null;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Properties

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if(PlaybackSpeed != 0 )
			{
				_progress += Time.Delta * PlaybackSpeed;
				//Progress is between 0 and 1
				// (PlaybackSpeed = 1) => _progress reaches 1 in 1 second
				if ( _progress >= 1 )
				{
					_progress = 1;
					PlaybackSpeed = 0;
					OnAnimationEnd?.Invoke( this );
					PauseTimer = (AnimationWait,(AnimationWait >= 0)); //Activates PauseTimer, unless wait time in negative
				}
				if(_progress <= 0 )
				{
					_progress = 0;
					PlaybackSpeed = 0;
					OnAnimationReset?.Invoke( this );
				}
				UpdateTargetValues();
			}
			else if(PauseTimer.active && PauseTimer.timer) //If PauseTimer is complete
			{
				PauseTimer = (0,false);
				ResetAnimation();
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			ScanForCrosshair();
		}

		
		private void UpdateTargetValues()
		{
			if ( Target == null ) _StartingProperties = null;
			if ( _StartingProperties == null ) return;
			if ( AnimationData == null ) return;
			CrosshairAnimation current = AnimationData[AnimationIndex];
			AnimationValueSetter setters = AnimationData.ValueSetters!;

			for(int f = 0; f < setters.FloatSetters.Count; f++ )
			{
				float val = (current.Floats[f] * Progress) + _StartingProperties.Floats[f];
				Action<IAnimatableCrosshair, float> action = setters.FloatSetters[f];
				action.Invoke( Target! , val );
			}
			for(int i = 0; i < setters.IntSetters.Count; i++ )
			{
				int val = (int)((float)current.Ints[i] * Progress) + _StartingProperties.Ints[i];
				setters.IntSetters[i].Invoke( Target! , val );
			}
			for(int c = 0; c < setters.ColorSetters.Count; c++ )
			{
				InterpolatableColor currentColor = current.Colors[c];
				Color val = ColorInterpolator.Interpolate( _StartingProperties.Colors[c].Color, currentColor.Color, Progress, currentColor.Interp );
				setters.ColorSetters[c].Invoke( Target! , val );
			}
		}
		private void UpdateStartingProperties()
		{
			if(Target == null )
			{
				_StartingProperties = null;
				return;
			}
			if ( AnimationData == null ) return;
			CrosshairAnimation ca = CrosshairAnimation.Empty;
			AnimationValueSetter avs = AnimationData.ValueSetters!;
			for(int f= 0; f < avs.FloatGetters.Count; f++ )
			{
				ca.Floats.Add(avs.FloatGetters[f].Invoke(Target));
			}
			for(int i= 0; i < avs.IntGetters.Count; i++ )
			{
				ca.Ints.Add(avs.IntGetters[i].Invoke(Target));
			}
			for(int c= 0; c < avs.ColorGetters.Count; c++ )
			{
				ca.Colors.Add(new(avs.ColorGetters[c].Invoke(Target)));
			}
			
			_StartingProperties = ca;
		}
		private CrosshairAnimation? _StartingProperties;
		private (TimeUntil timer,bool active) PauseTimer = (0,false);

		
	}
}
