using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{ 
    float speed = 15f; 
    float camRotate = .4f; 
    private Vector3 prevM = new Vector3(Screen.width / 2, Screen.height / 2, 0); 

    void Update()
    {

       
        prevM = Input.mousePosition - prevM;
        prevM = new Vector3(-prevM.y * camRotate, prevM.x * camRotate, 0);
        prevM = new Vector3(transform.eulerAngles.x + prevM.x, transform.eulerAngles.y + prevM.y, 0);
        transform.eulerAngles = prevM;
        prevM = Input.mousePosition;

        Vector3 vDir = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            vDir = new Vector3(0, 0, 1) + vDir;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vDir = new Vector3(0, 0, -1) + vDir;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vDir = new Vector3(-1, 0, 0) + vDir;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vDir = new Vector3(1, 0, 0) + vDir;
        }
        vDir = vDir * speed* Time.deltaTime;
        Vector3 newPosition = transform.position;     
        transform.Translate(vDir);
        

    }
}