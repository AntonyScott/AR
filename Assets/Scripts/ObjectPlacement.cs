using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private Button xbox360Btn;

    [SerializeField]
    private Button xboxOneBtn;

    [SerializeField]
    private Button xboxSeriesBtn;

    private GameObject placedPrefab;
    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject prefabName;

    public GameObject Xbox360;
    public GameObject XboxOne;
    public GameObject XboxSeriesX;

    private State gameState;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        xbox360Btn.onClick.AddListener(() => ChangePrefabTo("Xbox 360"));
        xboxOneBtn.onClick.AddListener(() => ChangePrefabTo("Xbox One"));
        xboxSeriesBtn.onClick.AddListener(() => ChangePrefabTo("Xbox Series X"));
    }
    void Update()
    {
        Debug.Log(gameState);
        if (placedPrefab == null)
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;
                bool isOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                Debug.Log(isOverUI);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log(" blocked raycast");
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && (hit.transform.tag == "Xbox"))
                {
                    Debug.Log(" raycast");
                    if (Input.GetTouch(0).deltaTime > 0.2f)
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
                else if (!isOverUI && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    Debug.Log(" arraycast");
                    var hitPose = hits[0].pose;
                    if(gameState == State.Xbox360)
                    {
                        Instantiate(Xbox360, hitPose.position, hitPose.rotation);
                    }
                    if (gameState == State.XboxOne)
                    {
                        Instantiate(XboxOne, hitPose.position, hitPose.rotation);
                    }
                    if (gameState == State.XboxSeriesX)
                    {
                        Instantiate(XboxSeriesX, hitPose.position, hitPose.rotation);
                    }
                }
            }
        }
    }

    private enum State
    {
        Xbox360,
        XboxOne,
        XboxSeriesX
    }

        void ChangePrefabTo(string prefabName)
        {
        Color colBtn1 = xbox360Btn.image.color;
        Color colBtn2 = xboxOneBtn.image.color;
        Color colBtn3 = xboxSeriesBtn.image.color;

        switch (prefabName)
        {
            case "Xbox 360":
                colBtn1.a = 1f;
                colBtn2.a = 0.5f;
                colBtn3.a = 0.5f;
                gameState = State.Xbox360;
                break;

            case "Xbox One":
                colBtn1.a = 0.5f;
                colBtn2.a = 1f;
                colBtn3.a = 0.5f;
                gameState = State.XboxOne;
                break;

            case "Xbox Series X":
                colBtn1.a = 0.5f;
                colBtn2.a = 0.5f;
                colBtn3.a = 1f;
                gameState = State.XboxSeriesX;
                break;
        }

        xbox360Btn.image.color = colBtn1;
        xboxOneBtn.image.color = colBtn2;
        xboxSeriesBtn.image.color = colBtn3;
    }
}
