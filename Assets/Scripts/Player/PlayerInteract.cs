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
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Interactable interactable =
                hitInfo.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Bật outline
                Outline outline = interactable.GetComponent<Outline>();

                if (outline != null)
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