using UnityEngine;
using System.Collections;
using Thnguyet.GameFeel;
using System.Collections.Generic;
using UnityEditor;

namespace Thnguyet.GameFeel
{
	[CustomEditor(typeof(ConeOfVision), true)]
	public class ConeOfVisionInspector : Editor
	{
		protected ConeOfVision _coneOfVision;

		protected virtual void OnSceneGUI()
		{
			// draws a circle around the character to represent the cone of vision's radius
			_coneOfVision = (ConeOfVision)target;

			Handles.color = Color.yellow;
			Handles.DrawWireArc(_coneOfVision.Center, Vector3.up, Vector3.forward, 360f, _coneOfVision.VisionRadius);

			// draws two lines to mark the vision angle
			Vector3 visionAngleLeft = FeelMaths.DirectionFromAngle(-_coneOfVision.VisionAngle / 2f, _coneOfVision.EulerAngles.y);
			Vector3 visionAngleRight = FeelMaths.DirectionFromAngle(_coneOfVision.VisionAngle / 2f, _coneOfVision.EulerAngles.y);

			Handles.DrawLine(_coneOfVision.Center, _coneOfVision.Center + visionAngleLeft * _coneOfVision.VisionRadius);
			Handles.DrawLine(_coneOfVision.Center, _coneOfVision.Center + visionAngleRight * _coneOfVision.VisionRadius);

			foreach (Transform visibleTarget in _coneOfVision.VisibleTargets)
			{
				Handles.color = FeelColors.Orange;
				Handles.DrawLine(_coneOfVision.Center, visibleTarget.position);
			}
		}
	}
}
