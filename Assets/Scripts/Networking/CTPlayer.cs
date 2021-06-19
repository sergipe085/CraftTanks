﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CTPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    #region Server

    public override void OnStartServer() { 
        Unit.ServerOnUnitSpawned   += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer() {
        Unit.ServerOnUnitSpawned   -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;    
    }

    private void ServerHandleUnitSpawned(Unit unit) {
        if (unit.connectionToClient.connectionId == connectionToClient.connectionId) {
            myUnits.Add(unit);
        }
    }

    private void ServerHandleUnitDespawned(Unit unit) {
        if (unit.connectionToClient.connectionId == connectionToClient.connectionId) {
            myUnits.Remove(unit);
        }
    }

    #endregion

    #region Client

    public override void OnStartClient() {
        if (!isClientOnly) return;

        Unit.AuthorityOnUnitSpawned   += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient(){
        if (!isClientOnly) return;

        Unit.AuthorityOnUnitSpawned   -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    private void AuthorityHandleUnitSpawned(Unit unit) {
        if (hasAuthority) {
            myUnits.Add(unit);
        }
    }

    private void AuthorityHandleUnitDespawned(Unit unit) {
        if (hasAuthority) {
            myUnits.Remove(unit);
        }
    }

    #endregion
}
