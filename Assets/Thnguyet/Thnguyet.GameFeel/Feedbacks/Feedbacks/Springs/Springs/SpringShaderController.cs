using Thnguyet.GameFeel;
using UnityEngine;

#if GAMEFEEL_UI
namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Shader Controller")]
	public class SpringShaderController : SpringFloatComponent<ShaderController>
	{
		public override float TargetFloat
		{
			get => Target.DrivenLevel;
			set => Target.DrivenLevel = value;
		}
	}
}
#endif