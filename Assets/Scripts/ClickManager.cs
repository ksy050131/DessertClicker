using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickManager : MonoBehaviour
{
    [Header("Punch Animation")]
    [SerializeField] private float punchScale = 0.85f;
    [SerializeField] private float punchDuration = 0.15f;

    private Vector3 originalScale;
    private Coroutine punchCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;

        // 버튼의 OnClick 이벤트에 자동 연결
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnJellyClick);
        }
    }

    /// <summary>
    /// 클릭 시 점수 추가 + 펀치 애니메이션을 실행합니다.
    /// </summary>
    public void OnJellyClick()
    {
        // 점수 추가
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPoints(GameManager.Instance.PointsPerClick);
        }

        // 펀치 애니메이션
        if (punchCoroutine != null)
        {
            StopCoroutine(punchCoroutine);
            transform.localScale = originalScale;
        }
        punchCoroutine = StartCoroutine(PunchAnimation());
    }

    private IEnumerator PunchAnimation()
    {
        float halfDuration = punchDuration * 0.5f;
        Vector3 targetScale = originalScale * punchScale;

        // 축소
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // 복원
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
        punchCoroutine = null;
    }
}
