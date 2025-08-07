using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UFO : Singleton<UFO>
{
    public FloatingJoystick joystick;
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float rotateSpeed = 50f; // Tốc độ xoay của aircraft
    public GameObject aircraft;
    public Tween scaleTween;
    public Transform leaner;
    public float leanAngle = 30f; // Góc nghiêng tối đa (độ)
    public float leanSpeed = 5f; // Tốc độ nghiêng (điều chỉnh độ mượt)

 
    private void Update()
    {
        // Rotate aircraft
        aircraft.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        // Move UFO and apply lean effect

        if (joystick == null) joystick = FindAnyObjectByType<FloatingJoystick>();
        if (joystick != null)
        {
            Vector3 moveDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;
            this.transform.position += moveDirection * Time.deltaTime * moveSpeed;

            // Lean effect
            if (moveDirection.magnitude > 0.1f)
            {
                Vector3 leanDirection = moveDirection;
                Quaternion targetRotation = Quaternion.Euler(leanDirection.z * leanAngle, 0, -leanDirection.x * leanAngle);
                leaner.transform.localRotation = Quaternion.Slerp(
                    leaner.transform.localRotation,
                    targetRotation,
                    Time.deltaTime * leanSpeed
                );
            }
            else
            {
                // Trở về trạng thái không nghiêng khi không di chuyển
                leaner.transform.localRotation = Quaternion.Slerp(
                    leaner.transform.localRotation,
                    Quaternion.identity,
                    Time.deltaTime * leanSpeed
                );
            }

        }
    }

    public void CollectObj(Transform targetTF)
    {
        StartCoroutine(IECollect());

        IEnumerator IECollect()
        {
            float initialDistance = Vector3.Distance(aircraft.transform.position, targetTF.position);

            // Generate random rotation speeds for X, Y, Z axes (e.g., between 50 and 360 degrees per second)
            float rotationSpeedX = Random.Range(50f, 360f);
            float rotationSpeedY = Random.Range(50f, 360f);
            float rotationSpeedZ = Random.Range(50f, 360f);

            while (Vector3.Distance(aircraft.transform.position, targetTF.position) > 0.3f)
            {
                // Move towards the aircraft
                targetTF.position = Vector3.MoveTowards(targetTF.position, aircraft.transform.position, Time.deltaTime * 23f);

                // Rotate around all three axes with random speeds
                targetTF.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);

                // Scale the object based on distance
                float currentDistance = Vector3.Distance(aircraft.transform.position, targetTF.position);
                targetTF.transform.localScale = Vector3.one * (0.4f + 0.6f * currentDistance / initialDistance);

                yield return null;
            }

            // Deactivate the target object
            targetTF.gameObject.SetActive(false);

            // Reset and animate aircraft scale
            scaleTween?.Kill();
            aircraft.transform.localScale = Vector3.one;
            scaleTween = aircraft.transform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);

            //fly item
            if (UIManager.Ins.GetUI<GamePlay>().targetObjects.FirstOrDefault(x => x.TargetObjectType == TargetObjectType.Box).amount>0){
                FlyItem flyItem = PoolManager.Ins.Spawn<FlyItem>(PoolType.FlyItem);
                flyItem.OnInitt(TargetObjectType.Box, Camera.main.WorldToScreenPoint(targetTF.position));
            }

            yield return null;
        }
    }

    [Button]
    public void ShowJoystick()
    {
        Transform joystickBg = joystick.transform.GetChild(0);
        joystickBg.gameObject.SetActive(true);
        joystickBg.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(UFO.Ins.transform.position+Vector3.back*10f);

    }
}