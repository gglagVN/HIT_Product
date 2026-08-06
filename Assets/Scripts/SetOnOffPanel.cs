using System.Collections;
using UnityEngine;

public class SetOnOffPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float closeAnimationTime = 1f;

    private bool isOpen = false;
    private bool isAnimating = false;

    private void Awake()
    {
        foreach (GameObject go in gameObjects)
        {
            if (go == null) continue;

            go.SetActive(false);

            Animator anim = go.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("isOpened", false);
            }
        }
    }

    public void TogglePanels()
    {
        if (isAnimating)
            return;

        if (isOpen)
        {
            StartCoroutine(ClosePanelsRoutine());
        }
        else
        {
            OpenPanels();
        }
    }

    private void OpenPanels()
    {
        isOpen = true;

        foreach (GameObject go in gameObjects)
        {
            if (go == null) continue;

            go.SetActive(true);

            Animator anim = go.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetBool("isOpened", true);
            }
        }
    }

    private IEnumerator ClosePanelsRoutine()
    {
        isAnimating = true;
        isOpen = false;

        foreach (GameObject go in gameObjects)
        {
            if (go == null) continue;

            Animator anim = go.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetBool("isOpened", false);
            }
        }

        // Chờ animation đóng chạy xong
        yield return new WaitForSecondsRealtime(closeAnimationTime);

        foreach (GameObject go in gameObjects)
        {
            if (go == null) continue;

            go.SetActive(false);
        }

        isAnimating = false;
    }
    public void ForceClose()
    {
        StopAllCoroutines();

        isOpen = false;
        isAnimating = false;

        foreach (GameObject go in gameObjects)
        {
            if (go == null) continue;

            Animator anim = go.GetComponent<Animator>();

            if (anim != null)
                anim.SetBool("isOpened", false);

            go.SetActive(false);
        }
    }
}