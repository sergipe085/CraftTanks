using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CTNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, Quaternion.identity);
        NetworkServer.Spawn(unitSpawnerInstance, conn);
    }
}
