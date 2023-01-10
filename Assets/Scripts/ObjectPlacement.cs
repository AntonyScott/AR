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
    Quaternion rot;
    public TMPro.TextMeshProUGUI prefabInfoText;
    private GameObject currentPrefab;
    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        //ChangePrefabTo("Xbox 360");
        xbox360Btn.onClick.AddListener(() => ChangePrefabTo("Xbox 360"));
        xboxOneBtn.onClick.AddListener(() => ChangePrefabTo("Xbox One"));
        xboxSeriesBtn.onClick.AddListener(() => ChangePrefabTo("Xbox Series X"));
    }

    void Update()
    {
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

                    if (currentPrefab != null)
                    {
                        Destroy(currentPrefab);
                        //Destroy(prefabInfoText);
                    }

                    if (placedPrefab == Resources.Load<GameObject>($"Xbox 360"))
                    {
                        Quaternion rot = Quaternion.Euler(0, 180, 0);

                        prefabInfoText.text = "The Xbox 360 is a home video game console developed by Microsoft. " +
                            "As the successor to the original Xbox, it is the second console in the Xbox series. " +
                            "It competed with Sony's PlayStation 3 and Nintendo's Wii as part of the seventh generation of video game consoles. " +
                            "It was officially unveiled on MTV on May 12, 2005, " +
                            "with detailed launch and game information announced later that month at the 2005 Electronic Entertainment Expo (E3).";
                    }
                    if (placedPrefab == Resources.Load<GameObject>($"Xbox One"))
                    {
                        Quaternion rot = Quaternion.Euler(-90, 180, 0);

                        prefabInfoText.text = "The Xbox One is a home video game console developed by Microsoft. " +
                            "Announced in May 2013, it is the successor to Xbox 360 and the third base console in the Xbox series of video game consoles. " +
                            "It was first released in North America, parts of Europe, Australia, and South America in November 2013 and in Japan, China, and other European countries in September 2014. " +
                            "It is the first Xbox game console to be released in China, specifically in the Shanghai Free-Trade Zone. " +
                            "Microsoft marketed the device as an \"all-in-one entertainment system\", hence the name \"Xbox One\". " +
                            "An eighth-generation console, it mainly competed against Sony's PlayStation 4 and Nintendo's Wii U and later the Switch.";
                    }
                    if (placedPrefab == Resources.Load<GameObject>($"Xbox Series X"))
                    {
                        Quaternion rot = Quaternion.Euler(-90, 0, 0);

                        prefabInfoText.text = "The Xbox Series X/S are home video game consoles developed by Microsoft. " +
                            "They were both released on November 10, 2020, as the fourth generation Xbox, succeeding the Xbox One. " +
                            "Along with Sony's PlayStation 5, also released in November 2020, the Xbox Series X/S consoles are part of the ninth generation of video game consoles. " +
                            "Rumors regarding the consoles first emerged in early 2019, with the line as a whole codenamed \"Scarlett\" and consisting of high-end and lower-end models codenamed \"Anaconda\" and \"Lockhart\" respectively; " +
                            "\"Anaconda\" was teased by Microsoft during E3 2019 under the codename \"Project Scarlett\", and unveiled during The Game Awards in December as Xbox Series X. " +
                            "On September 8, 2020, Microsoft unveiled the lower-end model, Xbox Series S.";
                    }
                    currentPrefab = Instantiate(placedPrefab, hitPose.position, rot);
                }
            }
        }
    }

    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"{prefabName}");

        if (placedPrefab == null)
        {
            Debug.LogError($"Prefab with {prefabName} could not be loaded, make sure you check the naming of prefabs");
        }

        Color colBtn1 = xbox360Btn.image.color;
        Color colBtn2 = xboxOneBtn.image.color;
        Color colBtn3 = xboxSeriesBtn.image.color;

        switch (prefabName)
        {
            case "Xbox 360":
                colBtn1.a = 1f;
                colBtn2.a = 0.5f;
                colBtn3.a = 0.5f;
                Debug.Log("Xbox 360");
                //Instantiate(Xbox360, Vector3.zero, Quaternion.identity);
                break;

            case "Xbox One":
                colBtn1.a = 0.5f;
                colBtn2.a = 1f;
                colBtn3.a = 0.5f;
                Debug.Log("Xbox One");
                //Instantiate(XboxOne, Vector3.zero, Quaternion.identity);
                break;

            case "Xbox Series X":
                colBtn1.a = 0.5f;
                colBtn2.a = 0.5f;
                colBtn3.a = 1f;
                Debug.Log("Xbox Series X");
                //Instantiate(XboxSeriesX, Vector3.zero, Quaternion.identity);
                break;
        }

        xbox360Btn.image.color = colBtn1;
        xboxOneBtn.image.color = colBtn2;
        xboxSeriesBtn.image.color = colBtn3;
    }
}
