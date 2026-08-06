using UnityEngine;
using System;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Float property setter
	/// </summary>
	public class PropertyLinkFloat : PropertyLink
	{
		public Func<float> GetFloatDelegate;
		public Action<float> SetFloatDelegate;

		protected float _initialValue;
		protected float _newValue;

		/// <summary>
		/// On init, grabs the initial float value
		/// </summary>
		/// <param name="property"></param>
		public override void Initialization(FeelProperty property)
		{
			base.Initialization(property);
			_initialValue = (float)GetPropertyValue(property);
		}

		/// <summary>
		/// Creates cached getter and setters for properties
		/// </summary>
		/// <param name="property"></param>
		public override void CreateGettersAndSetters(FeelProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == FeelProperty.MemberTypes.Property)
			{
				object firstArgument = (property.TargetScriptableObject == null) ? (object)property.TargetComponent : (object)property.TargetScriptableObject;

				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					GetFloatDelegate = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>),
						firstArgument,
						property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					SetFloatDelegate = (Action<float>)Delegate.CreateDelegate(typeof(Action<float>),
						firstArgument,
						property.MemberPropertyInfo.GetSetMethod());
				}
				_getterSetterInitialized = true;
			}
		}

		/// <summary>
		/// Gets the raw value of the property, a normalized float value, caching the operation if possible
		/// </summary>
		/// <param name="emitter"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public override object GetValue(PropertyEmitter emitter, FeelProperty property)
		{
			return GetValueOptimized(property);
		}

		/// <summary>
		/// Sets the raw property value, float normalized, caching the operation if possible
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="property"></param>
		/// <param name="level"></param>
		public override void SetValue(PropertyReceiver receiver, FeelProperty property, object newValue)
		{
			SetValueOptimized(property, (float)newValue);
		}

		/// <summary>
		/// Returns this property link's level between 0 and 1
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="property"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public override float GetLevel(PropertyEmitter emitter, FeelProperty property)
		{
			float returnValue = GetValueOptimized(property);

			returnValue = FeelMaths.Clamp(returnValue, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			returnValue = FeelMaths.Remap(returnValue, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);

			emitter.Level = returnValue;
			return returnValue;
		}
		
		public override float GetLevel(PropertyReceiver receiver, FeelProperty property)
		{
			float returnValue = GetValueOptimized(property);
			returnValue = FeelMaths.Remap(returnValue, receiver.FloatRemapZero, receiver.FloatRemapOne, 0f, 1f);
			return returnValue;
		}

		/// <summary>
		/// Sets the level 
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="property"></param>
		/// <param name="level"></param>
		public override void SetLevel(PropertyReceiver receiver, FeelProperty property, float level)
		{
			base.SetLevel(receiver, property, level);

			_newValue = FeelMaths.Remap(level, 0f, 1f, receiver.FloatRemapZero, receiver.FloatRemapOne);

			if (receiver.RelativeValue)
			{
				_newValue = _initialValue + _newValue;
			}

			SetValueOptimized(property, _newValue);
		}

		/// <summary>
		/// Gets either the cached value or the raw value
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected virtual float GetValueOptimized(FeelProperty property)
		{
			return _getterSetterInitialized ? GetFloatDelegate() : (float)GetPropertyValue(property);
		}

		/// <summary>
		/// Sets either the cached value or the raw value
		/// </summary>
		/// <param name="property"></param>
		/// <param name="newValue"></param>
		protected virtual void SetValueOptimized(FeelProperty property, float newValue)
		{
			if (_getterSetterInitialized)
			{
				SetFloatDelegate(_newValue);
			}
			else
			{
				SetPropertyValue(property, _newValue);
			}
		}
	}
}
