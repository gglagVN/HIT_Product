using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitive = 20f;
    public float ySensitive = 20f;
    private bool canLook = true;

    public void SetLookEnabled(bool value)
    {
        canLook = value;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột vào giữa màn hình
        Cursor.visible = false; // Ẩn con trỏ
    }
    private void Update()
    {
        if (!canLook)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        if (!canLook)
        {
            return;
        }

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= mouseY * Time.deltaTime * ySensitive;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * xSensitive);
    }
}