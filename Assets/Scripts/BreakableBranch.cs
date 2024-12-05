using System.Collections;
using UnityEngine;

public class BreakableBranch : MonoBehaviour
{
    private static float baseDestructionDelay = 5f;
    private static float minimumDestructionDelay = 0.75f;
    private bool isBreaking = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hook") && !isBreaking)
        {
            isBreaking = true;
            float destructionDelay = Mathf.Max(baseDestructionDelay, minimumDestructionDelay);
            StartCoroutine(VibrateAndScaleUp(destructionDelay));
        }
    }

    private IEnumerator VibrateAndScaleUp(float totalDelay)
    {
        float vibrationStartTime = 0f;
        float scaleUpStartTime = totalDelay - 1f;
        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.position;

        while (vibrationStartTime < totalDelay)
        {
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.1f);

            if (vibrationStartTime >= scaleUpStartTime)
            {
                float scaleProgress = (vibrationStartTime - scaleUpStartTime) / 1f;
                transform.localScale = originalScale * (1f + scaleProgress * 0.5f);
            }

            vibrationStartTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        Destroy(gameObject);
    }

    public static void UpdateDestructionDelay(float newDelay)
    {
        baseDestructionDelay = Mathf.Max(newDelay, minimumDestructionDelay);
    }
}
