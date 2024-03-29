using System.Collections;
using UnityEngine;

public class SampleQueueEventA : MonoBehaviour, IQueueEvent
{
    [SerializeField] private bool m_activeThis = true;

    public void ISetInvoke()
    {
        if (m_activeThis)
            StartCoroutine(ISetEventA());
        else
            QueueEventManager.Instance.Data.SetInvoke();
    }

    private IEnumerator ISetEventA()
    {
        Debug.Log("[Sample] Event A called!");
        yield return new WaitForSeconds(1f);
        Debug.Log("[Sample] Event A ended!");
        //
        QueueEventManager.Instance.Data.SetInvoke();
    }
}