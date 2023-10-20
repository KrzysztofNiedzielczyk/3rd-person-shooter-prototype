using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Weapon : MonoBehaviour
{
    public AudioSource shootAudio;
    public Transform muzzle;
    public Transform gunSocket;
    public ParentConstraint parentConstraint;
    public ConstraintSource constraintSource;
    public float fireRate = 15f;
    public float nextTimeToFire = 0f;
    public ParticleSystem muzzleFlash;

    private void Awake()
    {
        gunSocket = transform.parent;
        constraintSource = parentConstraint.GetSource(0);
        parentConstraint.RemoveSource(0);
        constraintSource.sourceTransform = gunSocket;
        parentConstraint.AddSource(constraintSource);
        parentConstraint.constraintActive = true;
    }
}
