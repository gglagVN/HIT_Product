using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    public float distance = 3f;

    [SerializeField] private LayerMask mask;

    private PlayerUI playerUI;
    private InputManager inputManager;

    private Outline currentOutline;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

        // Tắt outline của object cũ
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
#endif

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.TryGetComponent(out Interactable interactable))
            {
                // Bật outline
                if (interactable.TryGetComponent(out Outline outline))
                {
                    outline.enabled = true;
                    currentOutline = outline;
                }

                // Hiện text tương tác
                playerUI.UpdateText(interactable.promtMessage);

                // Nhấn E để tương tác
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}