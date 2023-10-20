using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsController : MonoBehaviour
{
    [SerializeField] private Transform gun1;
    [SerializeField] private Transform gun2;
    [SerializeField] private Transform gunsocket1;
    [SerializeField] private Transform gunsocket2;
    [SerializeField] private LayerMask ignoreMask;

    private bool fireing1 = false;
    private bool fireing2 = false;
    [SerializeField] private float rotationSpeed = 5f;

    private void Awake()
    {
        foreach (Transform child in gunsocket1)
        {
            if (child.gameObject.activeInHierarchy)
            {
                gun1 = child;
                break;
            }
        }

        foreach (Transform child in gunsocket2)
        {
            if (child.gameObject.activeInHierarchy)
            {
                gun2 = child;
                break;
            }
        }
    }

    private void Update()
    {
        Fireing();
        AdjustGunRotation();
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        //get if fire input was cancelled or else allow fireing
        if (context.canceled)
        {
            fireing1 = false;
        }
        else
        {
            fireing1 = true;
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        //get if fire input was cancelled or else allow fireing
        if (context.canceled)
        {
            fireing2 = false;
        }
        else
        {
            fireing2 = true;
        }
    }

    void Fireing()
    {
        //if input is stopped then do not execute code
        if (fireing1)
        {
            if(gun1.TryGetComponent(out IShootable shootable))
            {
                shootable.Fire();
            }
        }

        if (fireing2)
        {
            if(gun2.TryGetComponent(out IShootable shootable))
            {
                shootable.Fire();
            }
        }
            
    }

    void AdjustGunRotation()
    {
        //rotate a rigidbody torwards a mouse position using raycast but without rotating on y axis
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, 5000, ~ignoreMask))
        {
            Quaternion toRotation = Quaternion.LookRotation(hit.point - gun1.position, gun1.transform.up);
            gun1.rotation = Quaternion.RotateTowards(gun1.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (Physics.Raycast(castPoint, out hit, 5000, ~ignoreMask))
        {
            Quaternion toRotation = Quaternion.LookRotation(hit.point - gun2.position, gun2.transform.up);
            gun2.rotation = Quaternion.RotateTowards(gun2.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
