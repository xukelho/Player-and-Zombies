using System;
using System.Collections;
using UnityEngine;

public class ManagerCamera : MonoBehaviour
{
    #region Fields

    public static ManagerCamera Instance;

    public event EventHandler FlyToHumanFinished;

    public Transform LevelBoundsMin;
    public Transform LevelBoundsMax;

    public bool ShouldCameraOrbit;
    public float CameraRotationSpeed = .1f;

    public Camera CameraComponent;

    #endregion Fields

    #region Unity

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CameraComponent = GetComponent<Camera>();

        FrameLevel();

        StartCoroutine(KeepLevelBoundsInFrame());

        if (ShouldCameraOrbit)
            StartCoroutine(OrbitAroundLevel());
    }

    #endregion Unity

    #region Methods

    public void FlyToHuman()
    {
        StartCoroutine(PlayFinalAnimation());
    }

    private IEnumerator PlayFinalAnimation()
    {
        Vector3 humanPosition = Main.Instance.Player.transform.position;
        Vector3 mainCamPositionIgnoreY = new Vector3(transform.position.x, humanPosition.y, transform.position.z);
        Main.Instance.Player.transform.LookAt(mainCamPositionIgnoreY);

        Vector3 cameraToHumanDirection = (humanPosition - transform.position).normalized;

        while (GetCameraToHumanDistance() > 7)
        {
            Vector3 mainCamNewPosition = transform.position + (cameraToHumanDirection * .3f);
            transform.position = mainCamNewPosition;

            yield return 0;
        }

        yield return new WaitForSeconds(2);

        FlyToHumanFinished(null, null);
    }

    private IEnumerator OrbitAroundLevel()
    {
        for (; ; )
        {
            transform.LookAt(Vector3.Lerp(LevelBoundsMin.position, LevelBoundsMax.position, .5f));
            transform.Translate(Vector3.right * CameraRotationSpeed);
            yield return 0;
        }
    }

    private IEnumerator KeepLevelBoundsInFrame()
    {
        float savedScreenWidth = Screen.width;
        float savedScreenHeight = Screen.height;

        for (; ; )
        {
            bool hasCameraBeenResized = savedScreenWidth != Screen.width || savedScreenHeight != Screen.height;
            if (hasCameraBeenResized)
            {
                FrameLevel();

                savedScreenWidth = Screen.width;
                savedScreenHeight = Screen.height;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private void FrameLevel()
    {
        Vector3 levelCenter = Vector3.Lerp(LevelBoundsMin.position, LevelBoundsMax.position, .5f);

        Vector3 cameraPos = new Vector3(levelCenter.x, transform.position.y, transform.position.z);
        transform.position = cameraPos;
        transform.LookAt(levelCenter);

        Vector2 minBounds = CameraComponent.WorldToScreenPoint(LevelBoundsMin.position);

        while (!IsPointAlignedWithinCameraMargin(minBounds))
        {
            if (minBounds.x < 0)
                ZoomOut();
            else
                ZoomIn();

            minBounds = CameraComponent.WorldToScreenPoint(LevelBoundsMin.position);
        }
    }

    private bool IsPointAlignedWithinCameraMargin(Vector2 point)
    {
        bool isCameraWellPositioned;

        if (point.x < 0)
            isCameraWellPositioned = false;
        else
        {
            if (point.x > Screen.width * .1f)
                isCameraWellPositioned = false;
            else
                isCameraWellPositioned = true;
        }

        return isCameraWellPositioned;
    }

    private void ZoomOut() => ApplyZoom(-transform.forward);

    private void ZoomIn() => ApplyZoom(transform.forward);

    private void ApplyZoom(Vector3 direction) => transform.position = transform.position + direction * .1f;

    private float GetCameraToHumanDistance() => Vector3.Distance(transform.position, Main.Instance.Player.transform.position);

    #endregion Methods
}