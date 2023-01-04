using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//stupid test
public class Tester : MonoBehaviour
{
    private void Start()
    {
        //Yousef
        var Placeholders = GameObject.FindGameObjectsWithTag("Placeholder").ToList();
        var Cups = GameObject.FindGameObjectsWithTag("Moveable").ToList();

        List<GameObject> placedObjs = new List<GameObject>();
        print(Placeholders.Count);
        foreach(var holder in Placeholders)
        {
            int random = Random.Range(0, Cups.Count - 1);
            if (!placedObjs.Exists(obj => obj == Cups[random].gameObject))
            {
                Cups[random].transform.position = holder.transform.position;
                placedObjs.Add(Cups[random].gameObject);
            }

        }
    }
}
