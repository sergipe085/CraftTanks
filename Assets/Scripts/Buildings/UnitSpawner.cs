using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;

    #region Server

    [Command]
    private void CmdSpawnUnit() {
        GameObject unitInstance = Instantiate(unitPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left || !hasAuthority) {
            return;
        }

        CmdSpawnUnit();
    }

    #endregion
}
