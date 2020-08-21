using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]

public class PlanetManager : MonoBehaviour
{
    public List<PlanetGravity> planetList;
    public List<MovingBody> movingList;
    public static int SimulationPoints { get; } = 100;

    public static PlanetManager singleton;

    private void Awake()
    {
        singleton = GameObject.FindObjectOfType<PlanetManager>();
        

        planetList = new List<PlanetGravity>();
        planetList.Clear();
        movingList = new List<MovingBody>();
        movingList.Clear();

        planetList = GameObject.FindObjectsOfType<PlanetGravity>().ToList();
        movingList = GameObject.FindObjectsOfType<MovingBody>().ToList();
    }
}
