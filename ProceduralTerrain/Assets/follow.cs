using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    [SerializeField]
    private Transform route;
    private float tParam;

    private Vector3 arrowPos;
    private float speedModifier;
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;
    private List<Vector2> posL;
    private List<Vector2> timeL;
    private int count;
    private float totalCurLen;
    // Start is called before the first frame update
    void Start()
    {
        totalCurLen = 0f;
        count = -1;
        posL = new List<Vector2>();
        timeL = new List<Vector2>();
        tParam = 0f;
        speedModifier = 0.1f;

        p0 = route.GetChild(0).position;
        p1 = route.GetChild(1).position;
        p2 = route.GetChild(2).position;
        p3 = route.GetChild(3).position;

        for(float i = 0; i <= 1; i = i + 0.000005f)
        {
            Vector2 temp = new Vector2(i, GetLen(i, i + 0.000005f));
            posL.Add(temp);
            totalCurLen = totalCurLen + GetLen(i, i + 0.000005f);
        }

        float parCurLen = totalCurLen / 500;
        float curLenCount = 0;

        foreach (var i in posL)
        {         
            curLenCount = curLenCount + i.y;

            if (curLenCount >= parCurLen)
            {
                Vector2 temp = new Vector2(i.x, curLenCount);
                timeL.Add(temp);
                curLenCount = 0;
            }
        }

        foreach (var i in timeL)
        {
            //Debug.Log(i.x.ToString() + " " + i.y.ToString());
        }

        Debug.Log("Length of posL" + posL.Count.ToString());
        Debug.Log("Length of timeL" + timeL.Count.ToString());
        Debug.Log("total curve length is" + totalCurLen.ToString());
        Debug.Log("1/200 curve length is" + parCurLen.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        count = count + 1;
        if(count == timeL.Count)
        {
            count = 0;
        }
        Vector3 arrowPos = new Vector3();

        tParam = timeL[count].x;

        arrowPos = Mathf.Pow(1 - tParam, 3) * p0 +
          3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 
          3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + 
          Mathf.Pow(tParam, 3) * p3;

    
        
        transform.position = arrowPos;
    }

    private float GetLen(float prev,float next)
    {
        Vector3 prevPos = Mathf.Pow(1 - prev, 3) * p0 +
            3 * Mathf.Pow(1 - prev, 2) * prev * p1 +
            3 * (1 - prev) * Mathf.Pow(prev, 2) * p2 +
            Mathf.Pow(prev, 3) * p3;

        Vector3 nextPos = Mathf.Pow(1 - next, 3) * p0 +
            3 * Mathf.Pow(1 - next, 2) * next * p1 +
            3 * (1 - next) * Mathf.Pow(next, 2) * p2 +
            Mathf.Pow(next, 3) * p3;

        return Vector3.Distance(nextPos, prevPos);
    }


  
}
