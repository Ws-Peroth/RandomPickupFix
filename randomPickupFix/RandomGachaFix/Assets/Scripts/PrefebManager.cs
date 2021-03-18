using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefebManager : MonoBehaviour
{
    public static PrefebManager manager;
    
    Queue<GameObject> perfebQueue = new Queue<GameObject>();
    public GameObject texPrefeb;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        manager = this;

        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(texPrefeb);
            PerfebSetParent(obj);
            obj.SetActive(false);
        }
    }

    public void GachaPrefebInstactiate()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(texPrefeb);
            PerfebSetParent(obj);
            obj.SetActive(false);
        }
    }

    public GameObject ObjDeque()
    {
        if (manager.perfebQueue.Count != 0)
        {
            return perfebQueue.Dequeue();
        }
        return null;
    }

    public void ObjEnque(GameObject obj)
    {
        perfebQueue.Enqueue(obj);
    }

    public void PerfebSetParent(GameObject obj)
    {
        obj.transform.SetParent(gameObject.transform);
    }
}
    