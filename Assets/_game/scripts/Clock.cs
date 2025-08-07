using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public RectTransform kim;
    public float currentAngleZ;
    public float moveDuration;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartRotate();
    }

    public void StartRotate()
    {
        StartCoroutine(IERotate());
        IEnumerator IERotate()
        {
            kim.rotation = Quaternion.identity;
            currentAngleZ = 0;
            while (true)
            {
                DOVirtual.Float(currentAngleZ, currentAngleZ - 90, moveDuration, v =>
                {
                    kim.rotation = Quaternion.Euler(new Vector3(0, 0, v));
                });
                currentAngleZ -= 90f;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

}
