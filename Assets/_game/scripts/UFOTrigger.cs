using UnityEngine;

public class UFOTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("box"))
        {
            UFO.Ins.CollectObj(other.transform);
        }
    }
}
