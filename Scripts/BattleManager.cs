using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject player;
    public Player playerComponent;
    public Transform playerTransform;
    public WeaponsController weaponsController;
    public Transform aimPositionTransform;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerComponent = player.GetComponent<Player>();
        weaponsController = player.GetComponent<WeaponsController>();
        aimPositionTransform = GameObject.FindGameObjectWithTag("AimPosition").transform;
    }
}
