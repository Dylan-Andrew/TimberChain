using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D targetCursor;

    void Start()
    {
        Vector2 hotspot = new Vector2(targetCursor.width / 2, targetCursor.height / 2);
        Cursor.SetCursor(targetCursor, hotspot, CursorMode.Auto);
    }
}
