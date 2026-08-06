using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;
#endif
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Thnguyet.GameFeel
{
	public class PlotterAxis : MonoBehaviour
	{
		#if GAMEFEEL_UI
		public Text Label;
		public Text TimeLabel;
		#endif
		public Transform PlotterCurvePoint;

		public Transform PositionPoint;
		public Transform PositionPointVertical;
		public Transform RotationPoint;
		public Transform ScalePoint;
        
		public virtual void SetLabel(string newLabel)
		{
			#if GAMEFEEL_UI
			Label.text = newLabel;
			#endif
		}
	}
}