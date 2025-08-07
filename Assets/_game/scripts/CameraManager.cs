using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera cam;
    public Transform targetTF;
    public float camSpeed;
    public Vector3 offset;

    private void Start()
    {
        OnLoadLevelUFO();
    }

    public void OnLoadLevelUFO()
    {
        Vector3 targetCam = targetTF.transform.position + offset;
        cam.transform.position = targetCam;
        float oldFOV = cam.fieldOfView;
        float newFOV = cam.fieldOfView * 2 / 3f;
        cam.fieldOfView = newFOV;
        DOVirtual.Float(newFOV, oldFOV, 0.7f, v =>
        {
            cam.fieldOfView = v;
        }).SetEase(Ease.OutSine).SetDelay(1f).OnComplete(() =>
        {
            UFO.Ins?.ShowJoystick();
            FollowUFO();
            UIManager.Ins.GetUI<GamePlay>().InitTargetObjects();
        });
    }

    public void FollowUFO()
    {
        StopAllCoroutines();
        StartCoroutine(IEFollow());
        IEnumerator IEFollow()
        {
            while (true)
            {
                if (targetTF != null)
                {
                    Vector3 targetCam = targetTF.transform.position + offset;
                    cam.transform.position = Vector3.Lerp(cam.transform.position, targetCam, camSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }
    }
}
