using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thnguyet.GameFeel
{
	public enum TweenDefinitionTypes { FeelTween, AnimationCurve }

	[Serializable]
	public class TweenType
	{
		public static TweenType DefaultEaseInCubic { get; } = new TweenType(FeelTween.TweenCurve.EaseInCubic);
		public TweenDefinitionTypes TweenDefinitionType = TweenDefinitionTypes.FeelTween;
		public FeelTween.TweenCurve TweenCurve = FeelTween.TweenCurve.EaseInCubic;
		public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1f));
		public bool Initialized = false;
		
		public string ConditionPropertyName = "";
		public string EnumConditionPropertyName = "";
		public bool[] EnumConditions = new bool[32];

		public TweenType(FeelTween.TweenCurve newCurve, string conditionPropertyName = "", string enumConditionPropertyName = "", params int[] enumConditionValues)
		{
			TweenCurve = newCurve;
			TweenDefinitionType = TweenDefinitionTypes.FeelTween;
			ConditionPropertyName = conditionPropertyName;
			EnumConditionPropertyName = enumConditionPropertyName;
			for (int i = 0; i < enumConditionValues.Length; i++)
			{
				EnumConditions[enumConditionValues[i]] = true;
			}
		}
		public TweenType(AnimationCurve newCurve, string conditionPropertyName = "", string enumConditionPropertyName = "", params int[] enumConditionValues)
		{
			Curve = newCurve;
			TweenDefinitionType = TweenDefinitionTypes.AnimationCurve;
			ConditionPropertyName = conditionPropertyName;
			EnumConditionPropertyName = enumConditionPropertyName;
			for (int i = 0; i < enumConditionValues.Length; i++)
			{
				EnumConditions[enumConditionValues[i]] = true;
			}
		}

		public float Evaluate(float t)
		{
			return FeelTween.Evaluate(t, this);
		}
	}
}