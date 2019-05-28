/*
UVic CSC 305, 2019 Spring

Helping lab for assignment03
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{

    public GameObject boid;
    public GameObject marker;
    int boidCount = 50;

    private class Boids
    {
        public GameObject boidObject;
        public Vector3 velocity = Random.insideUnitSphere*Random.Range(0.2f,0.3f);
        public Vector3 acceleration = new Vector3 (0,0,0);
        public float maxForce = 0.5f;
        public float maxSpeed = 0.3f;
    }
    private List<Boids> boidList;

    void Start()
    {
        //boidList = new List<GameObject>();
        boidList = new List<Boids>();

        for (int i = 0; i < boidCount; ++i)
        {
            GameObject newBoid = new GameObject();
            newBoid.transform.parent = gameObject.transform;
            newBoid.name = "boid No." + i.ToString();

            GameObject instPrefab = Instantiate(boid);
            instPrefab.transform.parent = newBoid.transform;

            Boids boidd = new Boids();
            boidd.boidObject = newBoid;
            boidd.boidObject.transform.position = new Vector3(boid.transform.position.x + i+10, boid.transform.position.y, boid.transform.position.z+1+10);
            boidList.Add(boidd);

        }
    }


    // Update is called once per frame
    void Update()
    {
        
        foreach(var boid in boidList)
        {
            Vector3 allignment = Allign(boid)*1f;
            Vector3 coh = Cohesion(boid) * 1f;
            Vector3 sep = Seperation(boid) * 10f;


            boid.acceleration = boid.acceleration + allignment + coh + sep;
            boid.velocity = boid.velocity+boid.acceleration;
            if (boid.velocity.magnitude>boid.maxSpeed)
            {
                boid.velocity = boid.velocity.normalized * boid.maxSpeed;
            }
            Vector3 directiona = (marker.transform.position - boid.boidObject.transform.GetChild(0).position).normalized*0.4f + boid.velocity;
            //Vector3 directiona = boid.velocity;
            boid.boidObject.transform.GetChild(0).position += directiona.normalized * boid.velocity.magnitude;
            boid.boidObject.transform.GetChild(0).LookAt(marker.transform);

            Quaternion toRotationa = Quaternion.FromToRotation(transform.up, directiona);
            boid.boidObject.transform.GetChild(0).rotation = toRotationa;
            boid.acceleration = new Vector3(0, 0, 0);
        }

    }

    private Vector3 Allign(Boids boid)
    {
        float perceptionRadius = 30f;
        Vector3 steering = new Vector3(0, 0, 0);
        int total = 0;
        foreach (var boidd in boidList)
        {
            float distance = Vector3.Distance(boid.boidObject.transform.GetChild(0).position,boidd.boidObject.transform.GetChild(0).position);
            if(boidd != boid && distance < perceptionRadius)
            {
                steering = steering + boidd.velocity;
                total = total + 1;
            }
        }

        if (total>0)
        {
            steering = steering / total;
            steering = steering.normalized * boid.maxSpeed;
            steering = steering - boid.velocity;
            if (steering.magnitude>boid.maxForce)
            {
                steering = steering.normalized * boid.maxForce;
            }

        }
        return steering;
    }

    private Vector3 Cohesion(Boids boid)
    {
        float perceptionRadius = 100f;
        Vector3 steering = new Vector3(0, 0, 0);
        int total = 0;
        foreach (var boidd in boidList)
        {
            float distance = Vector3.Distance(boid.boidObject.transform.GetChild(0).position, boidd.boidObject.transform.GetChild(0).position);
            if (boidd != boid && distance < perceptionRadius)
            {
                steering = steering + boidd.boidObject.transform.GetChild(0).position;
                total = total + 1;
            }
        }

        if (total > 0)
        {
            steering = steering / total;
            steering = steering - boid.boidObject.transform.GetChild(0).position;
            steering = steering.normalized * boid.maxSpeed;
            steering = steering - boid.velocity;
            if (steering.magnitude > boid.maxForce)
            {
                steering = steering.normalized * boid.maxForce;
            }

        }
        return steering;
    }

    private Vector3 Seperation(Boids boid)
    {
        float perceptionRadius = 20f;
        Vector3 steering = new Vector3(0, 0, 0);
        int total = 0;
        foreach (var boidd in boidList)
        {
            float distance = Vector3.Distance(boid.boidObject.transform.GetChild(0).position, boidd.boidObject.transform.GetChild(0).position);
            if (boidd != boid && distance < perceptionRadius)
            {
                Vector3 diff = boid.boidObject.transform.GetChild(0).position - boidd.boidObject.transform.GetChild(0).position;
                diff = diff / (distance * distance);
                steering = steering + diff;
                total = total + 1;
            }
        }

        if (total > 0)
        {
            steering = steering / total;
            steering = steering.normalized * boid.maxSpeed;
            steering = steering - boid.velocity;
            if (steering.magnitude > boid.maxForce)
            {
                steering = steering.normalized * boid.maxForce;
            }

        }
        return steering;
    }


}