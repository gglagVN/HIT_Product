using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Image loadingFill;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentText;

    [Header("Loading Settings")]
    [SerializeField] private string sceneToLoad = "MAIN";
    [SerializeField] private float dotSpeed = 0.4f;

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        // Bắt đầu loading thật
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneToLoad);

        // Không chuyển scene ngay khi đạt 90%
        operation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (displayedProgress < 1f)
        {
            // Unity trả progress từ 0 -> 0.9
            float targetProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // Làm thanh loading chạy mượt
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                targetProgress,
                Time.deltaTime * 0.5f
            );

            // -------------------------
            // UPDATE LOADING BAR
            // -------------------------

            if (loadingFill != null)
            {
                loadingFill.fillAmount = displayedProgress;
            }

            // -------------------------
            // UPDATE PERCENT
            // -------------------------

            if (percentText != null)
            {
                int percent =
                    Mathf.RoundToInt(displayedProgress * 100f);

                percentText.text = percent + "%";
            }

            // -------------------------
            // UPDATE LOADING...
            // -------------------------

            UpdateLoadingText();

            yield return null;
        }

        // Đảm bảo hiển thị 100%
        if (loadingFill != null)
            loadingFill.fillAmount = 1f;

        if (percentText != null)
            percentText.text = "100%";

        if (loadingText != null)
            loadingText.text = "LOADING . . .";

        // Chờ một chút để người chơi nhìn thấy 100%
        yield return new WaitForSeconds(0.5f);

        // -------------------------
        // LOAD MAIN
        // -------------------------

        operation.allowSceneActivation = true;
    }

    private void UpdateLoadingText()
    {
        if (loadingText == null)
            return;

        float time = Time.time;

        int dots =
            Mathf.FloorToInt(time / dotSpeed) % 4;

        switch (dots)
        {
            case 0:
                loadingText.text = "LOADING";
                break;

            case 1:
                loadingText.text = "LOADING .";
                break;

            case 2:
                loadingText.text = "LOADING . .";
                break;

            case 3:
                loadingText.text = "LOADING . . .";
                break;
        }
    }
}