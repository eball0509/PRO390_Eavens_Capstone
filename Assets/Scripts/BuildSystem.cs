using UnityEngine;
using System.Linq;

public class BuildSystem : MonoBehaviour
{
    [Header("Build Settings")]
    public GameObject[] buildPrefabs;       // Prefabs you can build
    public float gridSize = 1f;             // Grid snapping
    public float previewYOffset = 0.1f;     // Offset for preview
    public float snapRadius = 1.0f;         // How far to look for nearby objects
    public float snapThreshold = 0.2f;      // Minimum distance to snap
    public float rotationStep = 90f;        // Rotate preview 90 degrees per press

    [Header("Layers")]
    public LayerMask buildableLayer;        // Floors, walls, already placed objects

    private GameObject currentPreview;
    private int currentPrefabIndex = 0;
    private bool buildMode = false;
    private Camera mainCamera;
    private float currentRotation = 0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleBuildModeToggle();

        if (buildMode)
        {
            UpdatePreview();
            HandlePlacement();
            HandlePrefabSwitch();
            HandleRotation();
        }
        else
        {
            DestroyPreview();
        }
    }

    private void HandleBuildModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = !buildMode;
            Debug.Log("Build Mode: " + (buildMode ? "ON" : "OFF"));
        }
    }

    private void HandlePrefabSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            currentPrefabIndex = (currentPrefabIndex + 1) % buildPrefabs.Length;
            ReplacePreviewPrefab();
        }
        else if (scroll < 0f)
        {
            currentPrefabIndex = (currentPrefabIndex - 1 + buildPrefabs.Length) % buildPrefabs.Length;
            ReplacePreviewPrefab();
        }
    }

    private void HandleRotation()
    {
        if (!currentPreview) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation += rotationStep;
            currentPreview.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
    }

    private void UpdatePreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildableLayer))
        {
            Vector3 targetPosition = hit.point + Vector3.up * previewYOffset;
            targetPosition = SnapToGrid(targetPosition);
            targetPosition = SnapToNearbyObjects(targetPosition);

            if (!currentPreview)
            {
                currentPreview = Instantiate(buildPrefabs[currentPrefabIndex], targetPosition, Quaternion.Euler(0f, currentRotation, 0f));
                SetPreviewMaterial(currentPreview, true);
                DisablePreviewColliders(currentPreview);
            }
            else
            {
                currentPreview.transform.position = targetPosition;
            }
        }
    }

    private void HandlePlacement()
    {
        if (Input.GetMouseButtonDown(0) && currentPreview != null)
        {
            GameObject placedObject = Instantiate(currentPreview, currentPreview.transform.position, currentPreview.transform.rotation);
            SetPreviewMaterial(placedObject, false);
            EnablePlacedObjectColliders(placedObject);
        }
    }

    private void ReplacePreviewPrefab()
    {
        if (!currentPreview) return;
        Vector3 pos = currentPreview.transform.position;
        Destroy(currentPreview);
        currentPreview = Instantiate(buildPrefabs[currentPrefabIndex], pos, Quaternion.Euler(0f, currentRotation, 0f));
        SetPreviewMaterial(currentPreview, true);
        DisablePreviewColliders(currentPreview);
    }

    private void DestroyPreview()
    {
        if (currentPreview)
        {
            Destroy(currentPreview);
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }

    private Vector3 SnapToNearbyObjects(Vector3 position)
    {
        Collider[] nearby = Physics.OverlapSphere(position, snapRadius, buildableLayer);

        foreach (Collider col in nearby)
        {
            Vector3 closestPoint = col.ClosestPoint(position);

            if (Vector3.Distance(position, closestPoint) <= snapThreshold)
            {
                position = closestPoint + Vector3.up * previewYOffset;
                break;
            }
        }

        return position;
    }

    private void SetPreviewMaterial(GameObject obj, bool isPreview)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            if (isPreview)
            {
                Material previewMat = new Material(rend.sharedMaterial);
                previewMat.color = new Color(previewMat.color.r, previewMat.color.g, previewMat.color.b, 0.5f);
                rend.material = previewMat;
            }
            else
            {
                rend.sharedMaterial = rend.sharedMaterial;
            }
        }
    }

    private void DisablePreviewColliders(GameObject preview)
    {
        Collider[] colliders = preview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void EnablePlacedObjectColliders(GameObject placedObject)
    {
        Collider[] colliders = placedObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true; 
        }
    }
}
