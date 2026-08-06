using UnityEngine;
using System.Collections;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Adds this class to particles to force their sorting layer
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Particles/Visible Particle")]
	public class VisibleParticle : MonoBehaviour {

		/// <summary>
		/// Sets the particle system's renderer to the Visible Particles sorting layer
		/// </summary>
		protected virtual void Start () 
		{
			GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "VisibleParticles";
		}		
	}
}