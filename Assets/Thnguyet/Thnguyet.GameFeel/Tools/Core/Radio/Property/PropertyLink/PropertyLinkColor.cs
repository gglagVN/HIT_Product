using UnityEngine;
using System;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Color property setter
	/// </summary>
	public class PropertyLinkColor : PropertyLink
	{
		public Func<Color> GetColorDelegate;
		public Action<Color> SetColorDelegate;

		protected Color _initialValue;
		protected Color _newValue;
		protected Color _color;

		/// <summary>
		/// On init we grab our initial color
		/// </summary>
		/// <param name="property"></param>
		public override void Initialization(FeelProperty property)
		{
			base.Initialization(property);
			_initialValue = (Color)GetPropertyValue(property);
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
					GetColorDelegate = (Func<Color>)Delegate.CreateDelegate(typeof(Func<Color>),
						firstArgument,
						property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					SetColorDelegate = (Action<Color>)Delegate.CreateDelegate(typeof(Action<Color>),
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
			SetValueOptimized(property, (Color)newValue);
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
			_color = _getterSetterInitialized ? GetColorDelegate() : (Color)GetPropertyValue(property);
			return _color.MeanRGB();
		}
		
		public override float GetLevel(PropertyReceiver receiver, FeelProperty property)
		{
			_color = _getterSetterInitialized ? GetColorDelegate() : (Color)GetPropertyValue(property);
			return _color.MeanRGB();
		}

		/// <summary>
		/// Sets the level, lerping between ColorRemapZero and One
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="property"></param>
		/// <param name="level"></param>
		public override void SetLevel(PropertyReceiver receiver, FeelProperty property, float level)
		{
			base.SetLevel(receiver, property, level);

			_newValue = Color.LerpUnclamped(receiver.ColorRemapZero, receiver.ColorRemapOne, level);

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
		protected virtual Color GetValueOptimized(FeelProperty property)
		{
			return _getterSetterInitialized ? GetColorDelegate() : (Color)GetPropertyValue(property);
		}

		/// <summary>
		/// Sets either the cached value or the raw value
		/// </summary>
		/// <param name="property"></param>
		/// <param name="newValue"></param>
		protected virtual void SetValueOptimized(FeelProperty property, Color newValue)
		{
			if (_getterSetterInitialized)
			{
				SetColorDelegate(_newValue);
			}
			else
			{
				SetPropertyValue(property, _newValue);
			}
		}
	}
}