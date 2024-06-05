using UnityEngine;
public class WaypointIndicator : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform wayPointRectTransform;
    void Start(){
        if(AssetWarmup.Instance!=null &&AssetWarmup.Instance.centerObject!=null){
            targetPosition = AssetWarmup.Instance.centerObject.transform.position;
        }
        wayPointRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }
    void Update()
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
        wayPointRectTransform.localEulerAngles = new Vector3 (0, 0, angle);
        Vector3 targetPostionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPostionScreenPoint.x<=0||targetPostionScreenPoint.x>=Screen.width||targetPostionScreenPoint.y<=0||targetPostionScreenPoint.y>Screen.height;
        Debug.Log(isOffScreen + " " + targetPostionScreenPoint);
    }
}
