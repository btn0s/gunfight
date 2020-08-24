/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using System.Collections;
using UnityEngine;

public class WeaponSniperSightHandler : MonoBehaviour
{
    [SerializeField] private Camera sightCamera;
    [SerializeField] private GameObject renderTextureObj;
    [SerializeField] private GameObject sightTextureObj;
    [SerializeField] private float durationUp;
    [SerializeField] private AnimationCurve upCurve;
    [SerializeField] private float durationDown;
    [SerializeField] private AnimationCurve downCurve;

    private CameraControl cameraControl;
    private Material renderTextureMaterial;
    private Material sightMaterial;

    private CoroutineObject<bool> sigthEaseInOutCoroutine;

    

    private void Awake()
    {
        FPController controller = transform.root.GetComponent<FPController>();
        if (controller != null)
            cameraControl = controller.GetCameraControl();

        Renderer renderTextureObjRenderer = renderTextureObj.GetComponent<Renderer>();
        if (renderTextureObjRenderer != null)
            renderTextureMaterial = renderTextureObjRenderer.sharedMaterial;

        Renderer sightObjRenderer = sightTextureObj.GetComponent<Renderer>();
        if (sightObjRenderer != null)
            sightMaterial = sightObjRenderer.sharedMaterial;

        if (cameraControl != null && renderTextureMaterial != null && sightMaterial != null)
        {
            sigthEaseInOutCoroutine = new CoroutineObject<bool>(this);
            cameraControl.OnZoomCallback += (isZooming, fov) =>
            {
                if (gameObject.activeSelf) 
                    sigthEaseInOutCoroutine.Start(SightEaseInOut, isZooming, true);
            };
        }
    }

    private void OnEnable()
    {
        if (cameraControl.IsZooming())
            sigthEaseInOutCoroutine.Start(SightEaseInOut, true, true);
    }

    protected virtual IEnumerator SightEaseInOut(bool isZooming)
    {
        if (isZooming)
            SightEnabled(true);

        float time = 0;
        float speed = 1 / (isZooming ? durationUp : durationDown);
        AnimationCurve curve = isZooming ? upCurve : downCurve;
        Color targetColor = isZooming ? Color.white : Color.black;

        while (time < 1.0f)
        {
            time += Time.deltaTime * speed;

            float smoothLerp = curve.Evaluate(time);
            renderTextureMaterial.color = Color.Lerp(renderTextureMaterial.color, targetColor, smoothLerp);
            yield return null;
        }

        if (!isZooming)
            SightEnabled(false);
    }

    public void SightEnabled(bool active)
    {
        sightCamera.gameObject.SetActive(active);
        renderTextureObj.SetActive(active);
        sightTextureObj.SetActive(active);
    }

    private void OnDisable()
    {
        SightEnabled(false);
        sigthEaseInOutCoroutine.Stop();
    }

    #region [Getter / Setter]
    public Camera GetSightCamera()
    {
        return sightCamera;
    }

    public void SetSightCamera(Camera value)
    {
        sightCamera = value;
    }

    public GameObject GetRenderTextureObj()
    {
        return renderTextureObj;
    }

    public void SetRenderTextureObj(GameObject value)
    {
        renderTextureObj = value;
    }

    public GameObject GetSightTextureObj()
    {
        return sightTextureObj;
    }

    public void SetSightTextureObj(GameObject value)
    {
        sightTextureObj = value;
    }

    public float GetDurationUp()
    {
        return durationUp;
    }

    public void SetDurationUp(float value)
    {
        durationUp = value;
    }

    public AnimationCurve GetUpCurve()
    {
        return upCurve;
    }

    public void SetUpCurve(AnimationCurve value)
    {
        upCurve = value;
    }

    public float GetDurationDown()
    {
        return durationDown;
    }

    public void SetDurationDown(float value)
    {
        durationDown = value;
    }

    public AnimationCurve GetDownCurve()
    {
        return downCurve;
    }

    public void SetDownCurve(AnimationCurve value)
    {
        downCurve = value;
    }
    #endregion
}
