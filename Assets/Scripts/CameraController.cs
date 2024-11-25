using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float baseSpeed = 2f; // Initial speed of the camera
    [SerializeField]
    private float maxSpeed = 10f; // Maximum speed the camera can reach
    [SerializeField]
    private float speedIncreaseRate = 0.1f; // How much the speed increases over time

    private float currentSpeed;
    [SerializeField]
    private GameObject GameManager;

    private void Start()
    {
        // Initialize the current speed with the base speed
        currentSpeed = baseSpeed;
    }

    private void LateUpdate()
    {
        if (GameManager.GetComponent<GameManager>().IsPlaying)
        {
            MoveCamera();
        }

    }

    private void MoveCamera()
    {
        currentSpeed = Mathf.Min(currentSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);
        if (player.position.y > transform.position.y)
        {
            // Smoothly move the camera towards the player's Y position
            float newYPosition = Mathf.Lerp(transform.position.y, player.position.y, currentSpeed * Time.deltaTime);

            // Update the camera's position
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }

        // Move the camera upward by current speed
        transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);
    }
}
