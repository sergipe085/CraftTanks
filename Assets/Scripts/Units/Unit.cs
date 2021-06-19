using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private UnityEvent   onSelected   = null;
    [SerializeField] private UnityEvent   onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartClient() {
        if (isClientOnly && hasAuthority) {
            AuthorityOnUnitSpawned?.Invoke(this);
        }
    }

    public override void OnStopClient() {
        if (isClientOnly && hasAuthority) {
            AuthorityOnUnitDespawned.Invoke(this);
        }
    }

    public UnitMovement GetMovement() {
        return unitMovement;
    }

    [Client]
    public void Select() {
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect() {
        onDeselected?.Invoke();
    }

    #endregion
}
