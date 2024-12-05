using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float baseSpeed = 0.5f;
    [SerializeField]
    private float maxSpeed = 3f;
    [SerializeField]
    private float speedIncreaseRate = 0.1f;

    private float currentSpeed;
    [SerializeField]
    private GameObject GameManager;

    private Camera cameraComponent;

    private void Start()
    {
        currentSpeed = baseSpeed;
        cameraComponent = Camera.main;
    }

    private void LateUpdate()
    {
        if (GameManager.GetComponent<GameManager>().IsPlaying)
        {
            MoveCamera();
            UpdateBackgroundColor();
        }
    }

    private void MoveCamera()
    {
        currentSpeed = Mathf.Min(currentSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);
        if (player.position.y > transform.position.y)
        {
            float newYPosition = Mathf.Lerp(transform.position.y, player.position.y, currentSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
        transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);
    }

    private void UpdateBackgroundColor()
    {
        float height = transform.position.y;
        Color targetColor = GetBackgroundColorForHeight(height);
        cameraComponent.backgroundColor = Color.Lerp(cameraComponent.backgroundColor, targetColor, Time.deltaTime * currentSpeed);
    }

    private Color GetBackgroundColorForHeight(float height)
    {
        if (height <= 100f)
        {
            return Color.Lerp(new Color(0.149f, 0.343f, 0f), new Color(0.273f, 0.135f, 0.037f), height / 100f);
        }
        else if (height <= 200f)
        {
            return Color.Lerp(new Color(0.273f, 0.135f, 0.037f), new Color(0.5f, 0.422f, 0f), (height - 100f) / 100f);
        }
        else if (height <= 300f)
        {
            return Color.Lerp(new Color(0.5f, 0.422f, 0f), new Color(0.265f, 0.404f, 0.461f), (height - 200f) / 100f);
        }
        else if (height <= 350f)
        {
            return Color.Lerp(new Color(0.265f, 0.404f, 0.461f), new Color(0.137f, 0.319f, 0.447f), (height - 300f) / 50f);
        }
        else if (height <= 400f)
        {
            return Color.Lerp(new Color(0.137f, 0.319f, 0.447f), new Color(0.287f, 0.22f, 0.429f), (height - 350f) / 50f);
        }
        else if (height <= 450f)
        {
            return Color.Lerp(new Color(0.287f, 0.22f, 0.429f), new Color(0.057f, 0.114f, 0.27f), (height - 400f) / 50f);
        }
        else if (height <= 500f)
        {
            return Color.Lerp(new Color(0.057f, 0.114f, 0.27f), Color.black, (height - 450f) / 50f);
        }
        else
        {
            return Color.black;
        }
    }
}