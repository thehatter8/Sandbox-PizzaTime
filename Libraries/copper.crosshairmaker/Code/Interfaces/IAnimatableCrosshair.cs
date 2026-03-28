using Sandbox;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using CrosshairMaker.Helpers;
using Sandbox.Helpers;
using System.Collections;
using CrosshairMaker.Animator;
using Sandbox.Rendering;
using CrosshairMaker.Abstract;

namespace CrosshairMaker.Interfaces
{
	public interface IAnimatableCrosshair : IHudPaintable
	{
		public AnimationDictionary GetAnimationDictionary();
		public static CrosshairAnimator? ScanForAnimator(BaseCrosshair self )
		{
			IEnumerable<CrosshairAnimator> animators = self.GameObject.GetComponents<CrosshairAnimator>();
			if(animators.Count() == 0) return null;
			foreach(CrosshairAnimator animator in animators )
			{
				if (animator.Target == self) return animator;
			}
			return null;
		}
	}
}
