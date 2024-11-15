﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_SoloRing : MonoBehaviour
{

    [SerializeField] private GameObject SphereMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject ring;
    private float PercentOfPlusMode;
    private float PercentOfCrossMode;
    private float PercentOfBreathingMode;
    private float PercentOfLongitudinalMode;
    private float PercentOfXMode;
    private float PercentOfYMode;

    [SerializeField] public float phase;
    [SerializeField] public float ampIndex;

    private List<GameObject> sphere_array;
    private List<float> angles_array;
    private List<Vector3> sphere_pos_array;
    private GW_GravityScript gw_gravity;

    //public float phase;


    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {

        gw_gravity = GetComponent<GW_GravityScript>();

        //Initialize mesh and angles array, to create our coordinate system which we use to translate particles
        sphere_array = new List<GameObject>(numberOfMeshes);
        angles_array = new List<float>(numberOfMeshes);
        sphere_pos_array = new List<Vector3>(numberOfMeshes);

        //Center position of the GameObject this script is attached to, used to spawn particles around it
        Vector3 center = transform.position;

        //Setting up angular step for the ring of particles, based on the number of meshes
        float global_ang = 360.0f / numberOfMeshes;

        for (int i = 0; i < numberOfMeshes; i++)
        {
            //Set up angular distance of each particle in the ring
            float a = i * global_ang;

            //Arranges the system of particles and gives a coordinate of the particle based on a circle with a center and radius given, definition below
            Vector3 pos = SpawnCircle(center, radius, a);

            //Instantiate particle mesh prefab and store particle GameObject, particle position, and angular distance of the particle in our coordinate arrays
            GameObject instance = Instantiate(SphereMesh, pos, Quaternion.identity) as GameObject;
            sphere_array.Add(instance);
            sphere_pos_array.Add(pos);
            angles_array.Add(a);
            instance.tag = "tp";

            //Parents the particle to the ring GameObject
            instance.transform.parent = ring.transform;
        }

        PercentOfPlusMode = 0;
        PercentOfCrossMode = 0;
        PercentOfBreathingMode = 0;
        PercentOfLongitudinalMode = 0;
        PercentOfXMode = 0;
        PercentOfYMode = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(angles_array.Count);
    }

    private void FixedUpdate()
    {
        //Sanity check
        if (true)
        {
            for (int i = 0; i < numberOfMeshes; i++)
            {

                
                Vector3 pos = gw_gravity.CalculateOscillations(sphere_pos_array[i], ring.transform.position,true, 0.0f, 0.0f, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfXMode, PercentOfYMode);
                // pos.z = sphere_array[i].transform.position.z;

                //Translates particle to the calculated coordinate
                sphere_array[i].transform.position = pos;
            }
        }
    }

    Vector3 SpawnCircle(Vector3 center, float radius, float ang)
    {

        //Generates a coordinate based on a parametric equation of a circle based on the center, radius, and angular distance given
        Vector3 pos;

        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;

        return pos;
    }

    public void SetPlusMode(float percent) { PercentOfPlusMode = percent; }
    public void SetCrossMode(float percent) { PercentOfCrossMode = percent; }
    public void SetBreathingMode(float percent) { PercentOfBreathingMode = percent; }
    public void SetLongitudinalMode(float percent) { PercentOfLongitudinalMode = percent; }
    public void SetXMode(float percent) { PercentOfXMode = percent; }
    public void SetYMode(float percent) { PercentOfYMode = percent; }

    public float GetPlusMode() { return PercentOfPlusMode; }
    public float GetCrossMode() { return PercentOfCrossMode; }
    public float GetBreathingMode() { return PercentOfBreathingMode; }
    public float GetLongitudinalMode() { return PercentOfLongitudinalMode; }
    public float GetXMode() { return PercentOfXMode; }
    public float GetYMode() { return PercentOfYMode; }
}
