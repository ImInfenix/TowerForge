//Script definissant un chemin

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform spawnPoint; //Point de depart
    public Transform[] waypoints; //Points a suivre

    //Initialisation
    void Start()
    {
        //Recupere les points du chemin
        GameObject[] waypointsObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        //Trie les points dans un ordre alphanumerique
        Array.Sort(waypointsObjects, (x, y) => String.Compare(x.name, y.name));

        //Defini les points a suivre a partir de ceux trouves
        waypoints = new Transform[waypointsObjects.Length];
        for (int i = 0; i < waypointsObjects.Length; i++)
        {
            waypoints[i] = waypointsObjects[i].transform;
        }
    }
}
