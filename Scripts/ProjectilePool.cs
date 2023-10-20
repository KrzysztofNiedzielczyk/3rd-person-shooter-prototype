using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : Singleton<ProjectilePool>
{
    public List<GameObject> blueProjectilePool = new List<GameObject>();
    public List<GameObject> redProjectilePool = new List<GameObject>();
    public List<GameObject> redBigProjectilePool = new List<GameObject>();
    public List<GameObject> guidedMissilePool = new List<GameObject>();
}
