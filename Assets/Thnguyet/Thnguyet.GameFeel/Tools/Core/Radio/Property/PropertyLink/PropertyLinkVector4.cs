using UnityEngine;
using System;

namespace Thnguyet.GameFeel
{
	public class PropertyLinkVector4 : PropertyLink
	{
		public Func<Vector4> GetVector4Delegate;
		public Action<Vector4> SetVector4Delegate;

		protected Vector4 _initialValue;
		protected Vector4 _newValue;
		protected Vector4 _vector4;

		public override void Initialization(FeelProperty property)
		{
			base.Initialization(property);
			_initialValue = (Vector4)GetPropertyValue(property);
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
					GetVector4Delegate = (Func<Vector4>)Delegate.CreateDelegate(typeof(Func<Vector4>),
						firstArgument,
						property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					SetVector4Delegate = (Action<Vector4>)Delegate.CreateDelegate(typeof(Action<Vector4>),
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
			SetValueOptimized(property, (Vector4)newValue);
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
			_vector4 = GetValueOptimized(property);

			float newValue = 0f;

			switch (emitter.Vector4Option)
			{
				case PropertyEmitter.Vector4Options.X:
					newValue = _vector4.x;
					break;
				case PropertyEmitter.Vector4Options.Y:
					newValue = _vector4.y;
					break;
				case PropertyEmitter.Vector4Options.Z:
					newValue = _vector4.z;
					break;
				case PropertyEmitter.Vector4Options.W:
					newValue = _vector4.w;
					break;
			}

			float returnValue = newValue;
			returnValue = FeelMaths.Clamp(returnValue, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			returnValue = FeelMaths.Remap(returnValue, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);

			emitter.Level = returnValue;
			return returnValue;
		}
		
		public override float GetLevel(PropertyReceiver receiver, FeelProperty property)
		{
			_vector4 = _getterSetterInitialized ? GetVector4Delegate() : (Vector4)GetPropertyValue(property);

			float newValue = 0f;

			if (receiver.ModifyX)
			{
				newValue = _vector4.x;
			}
			else if (receiver.ModifyY)
			{
				newValue = _vector4.y;
			}
			else if (receiver.ModifyZ)
			{
				newValue = _vector4.z;
			}
			else if (receiver.ModifyW)
			{
				newValue = _vector4.w;
			}

			float returnValue = newValue;
			returnValue = FeelMaths.Remap(returnValue, receiver.FloatRemapZero, receiver.FloatRemapOne, 0f, 1f);

			return returnValue;
		}

		public override void SetLevel(PropertyReceiver receiver, FeelProperty property, float level)
		{
			base.SetLevel(receiver, property, level);

			_newValue.x = receiver.ModifyX ? FeelMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.x, receiver.Vector4RemapOne.x) : 0f;
			_newValue.y = receiver.ModifyY ? FeelMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.y, receiver.Vector4RemapOne.y) : 0f;
			_newValue.z = receiver.ModifyZ ? FeelMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.z, receiver.Vector4RemapOne.z) : 0f;
			_newValue.w = receiver.ModifyW ? FeelMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.w, receiver.Vector4RemapOne.w) : 0f;

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
		protected virtual Vector4 GetValueOptimized(FeelProperty property)
		{
			return _getterSetterInitialized ? GetVector4Delegate() : (Vector4)GetPropertyValue(property);
		}

		/// <summary>
		/// Sets either the cached value or the raw value
		/// </summary>
		/// <param name="property"></param>
		/// <param name="newValue"></param>
		protected virtual void SetValueOptimized(FeelProperty property, Vector4 newValue)
		{
			if (_getterSetterInitialized)
			{
				SetVector4Delegate(_newValue);
			}
			else
			{
				SetPropertyValue(property, _newValue);
			}
		}
	}
}
