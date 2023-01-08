using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementControllerWithMultiple : MonoBehaviour
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

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        xbox360Btn.onClick.AddListener(() => ChangePrefabTo("Xbox 360"));
        xboxOneBtn.onClick.AddListener(() => ChangePrefabTo("Xbox One"));
        xboxSeriesBtn.onClick.AddListener(() => ChangePrefabTo("Xbox Series X"));
    }
    void Update()
    {

    }

    void ChangePrefabTo()
    {
        placedPrefab = Resources.Load<GameObject>($"prefabs/xbox/{prefabName}");
    }
}
