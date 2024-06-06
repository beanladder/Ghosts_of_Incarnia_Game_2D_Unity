using UnityEngine;
using UnityEngine.UI;
public class WaypointIndicator : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform wayPointRectTransform;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    private Image pointerImage;
    void Start(){
        if(AssetWarmup.Instance!=null &&AssetWarmup.Instance.centerObject!=null){
            targetPosition = AssetWarmup.Instance.centerObject.transform.position;
        }
        wayPointRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();
    }
    void FixedUpdate()
    {
        float borderSize = 100f;
        Vector3 targetPostionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPostionScreenPoint.x<=borderSize||targetPostionScreenPoint.x>=Screen.width-borderSize||targetPostionScreenPoint.y<=borderSize||targetPostionScreenPoint.y>Screen.height-borderSize;
        Debug.Log(isOffScreen + " " + targetPostionScreenPoint);
        if (isOffScreen)
        {
            RotatePointer();
            pointerImage.sprite = arrowSprite;
            Vector3 cappedTargetScreenPosition = targetPostionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if(cappedTargetScreenPosition.x>=Screen.width-borderSize) cappedTargetScreenPosition.x = Screen.width-borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if(cappedTargetScreenPosition.y>=Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;
            Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            wayPointRectTransform.position = pointerWorldPosition;
            wayPointRectTransform.localPosition = new Vector3(wayPointRectTransform.localPosition.x, wayPointRectTransform.localPosition.y, 0f);
        }
        else
        {
            pointerImage.sprite = crossSprite;
            Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(targetPostionScreenPoint);
            wayPointRectTransform.position = pointerWorldPosition;
            wayPointRectTransform.localPosition = new Vector3(wayPointRectTransform.localPosition.x, wayPointRectTransform.localPosition.y, 0f);
            wayPointRectTransform.localEulerAngles = Vector3.zero;
        }
    }
    void RotatePointer()
    {
        Vector3 toPostion = targetPosition;
        Vector3 fromPostion = Camera.main.transform.position;
        fromPostion.z = 0;
        Vector3 dir = (toPostion - fromPostion).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        
        wayPointRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
