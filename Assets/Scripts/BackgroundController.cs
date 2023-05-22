using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float valueX = 0.05f;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(valueX, 0) * Time.deltaTime, rawImage.uvRect.size);
    }
}
