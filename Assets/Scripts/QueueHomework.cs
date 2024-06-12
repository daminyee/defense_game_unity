using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QueueHomework : MonoBehaviour
{
    public GameObject Object;
    Queue<GameObject> queue = new Queue<GameObject>();

    float instantiateCoolCount;

    void Start()
    {

    }

    void Update()
    {
        if (instantiateCoolCount > 3.5f)
        {
            GameObject instantiatedObject = Instantiate(Object, transform.position, Quaternion.identity);
            queue.Enqueue(instantiatedObject);
            if (queue.Count > 5)
            {
                var peekedObject = queue.Peek();
                queue.Dequeue();
                Destroy(peekedObject);
            }
            instantiateCoolCount = 0f;
        }
        instantiateCoolCount += Time.deltaTime * 1f;
    }
}
