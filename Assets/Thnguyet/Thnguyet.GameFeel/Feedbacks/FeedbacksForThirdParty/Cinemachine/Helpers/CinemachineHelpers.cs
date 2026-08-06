using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
#if GAMEFEEL_CINEMACHINE
using Cinemachine;
#elif GAMEFEEL_CINEMACHINE3
using Unity.Cinemachine;
#endif

namespace Thnguyet.GameFeel.ThirdParty
{
	public class CinemachineHelpers : MonoBehaviour
	{
		public static GameObject AutomaticCinemachineShakersSetup(FeedbackPlayer owner, string feedbackName)
		{
			GameObject virtualCameraGo = null;
			
			
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			bool newVcam = false;
			string additions = owner.name + " "+feedbackName+" feedback automatic shaker setup : ";
			#endif
			
			#if GAMEFEEL_CINEMACHINE 
				//looks for a Cinemachine Brain in the scene
				CinemachineBrain cinemachineBrain = (CinemachineBrain)Object.FindAnyObjectByType(typeof(CinemachineBrain));
				if (cinemachineBrain == null)
				{
					cinemachineBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
					additions += "Added a Cinemachine Brain to the scene. ";
				}
			
				// looks for a vcam in the scene
				CinemachineVirtualCamera virtualCamera = (CinemachineVirtualCamera)Object.FindAnyObjectByType(typeof(CinemachineVirtualCamera));
				if (virtualCamera == null)
				{
					GameObject newVirtualCamera = new GameObject("CinemachineVirtualCamera");
					if (Camera.main != null)
					{
						newVirtualCamera.transform.position = Camera.main.transform.position;	
					}
					virtualCamera = newVirtualCamera.AddComponent<CinemachineVirtualCamera>();
					additions += "Added a Cinemachine Virtual Camera to the scene. ";
					newVcam = true;
				}
				virtualCameraGo = virtualCamera.gameObject;
				CinemachineImpulseListener impulseListener = virtualCamera.GetComponent<CinemachineImpulseListener>();
				if (impulseListener == null)
				{
					impulseListener = virtualCamera.gameObject.AddComponent<CinemachineImpulseListener>();
					additions += "Added an impulse listener. ";
			}
			#elif GAMEFEEL_CINEMACHINE3
				//looks for a Cinemachine Brain in the scene
				CinemachineBrain cinemachineBrain = (CinemachineBrain)Object.FindAnyObjectByType(typeof(CinemachineBrain));
				if (cinemachineBrain == null)
				{
					cinemachineBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
					additions += "Added a Cinemachine Brain to the scene. ";
				}
				// looks for a vcam in the scene
				CinemachineCamera virtualCamera = (CinemachineCamera)Object.FindAnyObjectByType(typeof(CinemachineCamera));
				if (virtualCamera == null)
				{
					GameObject newVirtualCamera = new GameObject("CinemachineCamera");
					if (Camera.main != null)
					{
						newVirtualCamera.transform.position = Camera.main.transform.position;	
					}
					virtualCamera = newVirtualCamera.AddComponent<CinemachineCamera>();
					additions += "Added a Cinemachine Camera to the scene. ";
					newVcam = true;
				}
				virtualCameraGo = virtualCamera.gameObject;
				CinemachineImpulseListener impulseListener = virtualCamera.GetComponent<CinemachineImpulseListener>();
				if (impulseListener == null)
				{
					impulseListener = virtualCamera.gameObject.AddComponent<CinemachineImpulseListener>();
					additions += "Added an impulse listener. ";
				}
			#endif

			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			if (newVcam)
			{
				virtualCameraGo.GetOrAddComponent<CinemachineCameraShaker>();
				virtualCameraGo.GetOrAddComponent<CinemachineZoom>();
				virtualCameraGo.GetOrAddComponent<CinemachinePriorityListener>();
				virtualCameraGo.GetOrAddComponent<CinemachineClippingPlanesShaker>();
				virtualCameraGo.GetOrAddComponent<CinemachineFieldOfViewShaker>();	
				additions += "Added camera shaker, zoom, priority listener, clipping planes shaker and field of view shaker to the Cinemachine Camera. ";
			}
			
			FeelDebug.DebugLogInfo( additions + "You're all set.");
			#endif
			return virtualCameraGo;
		}
	}
}
