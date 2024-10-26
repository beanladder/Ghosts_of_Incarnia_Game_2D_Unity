using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaypointIndicator : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform wayPointRectTransform;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    private Image pointerImage;
    [SerializeField] private Camera cam;

    private string currentSceneName;

    void Start()
    {
        if (AssetWarmup.Instance != null && AssetWarmup.Instance.centerObject != null)
        {
            targetPosition = AssetWarmup.Instance.centerObject.transform.position;
        }

        if (transform != null)
        {
            wayPointRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
            pointerImage = transform.Find("Pointer").GetComponent<Image>();
        }

        currentSceneName = SceneManager.GetActiveScene().name;
        HandleSceneChange();
    }

    void Update()
    {
        string newSceneName = SceneManager.GetActiveScene().name;

        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;
            HandleSceneChange();
        }

        if (pointerImage.enabled)
        {
            UpdateWaypointIndicator();
        }
    }

    void HandleSceneChange()
    {
        if (currentSceneName == "MapConvert")
        {
            ShowPointer();
        }
        else
        {
            HidePointer();
        }
    }

    void UpdateWaypointIndicator()
    {
        float borderSize = 100f;
        Vector3 targetPostionScreenPoint = cam.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPostionScreenPoint.x <= borderSize || targetPostionScreenPoint.x >= Screen.width - borderSize || targetPostionScreenPoint.y <= borderSize || targetPostionScreenPoint.y > Screen.height - borderSize;

        if (isOffScreen)
        {
            RotatePointer();
            pointerImage.sprite = arrowSprite;
            Vector3 cappedTargetScreenPosition = targetPostionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;
            Vector3 pointerWorldPosition = cam.ScreenToWorldPoint(cappedTargetScreenPosition);
            wayPointRectTransform.position = pointerWorldPosition;
            wayPointRectTransform.localPosition = new Vector3(wayPointRectTransform.localPosition.x, wayPointRectTransform.localPosition.y, 0f);
        }
        else
        {
            pointerImage.sprite = crossSprite;
            Vector3 pointerWorldPosition = cam.ScreenToWorldPoint(targetPostionScreenPoint);
            wayPointRectTransform.position = pointerWorldPosition;
            wayPointRectTransform.localPosition = new Vector3(wayPointRectTransform.localPosition.x, wayPointRectTransform.localPosition.y, 0f);
            wayPointRectTransform.localEulerAngles = Vector3.zero;
        }
    }

    void RotatePointer()
    {
        Vector3 toPostion = targetPosition;
        Vector3 fromPostion = cam.transform.position;
        fromPostion.z = 0;
        Vector3 dir = (toPostion - fromPostion).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        wayPointRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    void HidePointer()
    {
        pointerImage.enabled = false;
    }

    void ShowPointer()
    {
        pointerImage.enabled = true;
    }
}
