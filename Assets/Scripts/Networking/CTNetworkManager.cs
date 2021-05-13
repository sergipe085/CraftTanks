using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CraftTanks.Networking 
{
    public class CTNetworkManager : NetworkManager
    {
        [Header("Units")]
        [SerializeField] private GameObject unitSpawner = null;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            GameObject unitSpawnerInstance = Instantiate(unitSpawner, conn.identity.transform.position, conn.identity.transform.rotation);
            NetworkServer.Spawn(unitSpawnerInstance, conn);
        }
    }
}
