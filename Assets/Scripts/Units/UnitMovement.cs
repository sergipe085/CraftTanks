using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [Header("CORE")]
    [SerializeField] private NavMeshAgent agent = null;

    #region Server

    [ServerCallback]
    private void Update() {
        if (agent.remainingDistance < agent.stoppingDistance && agent.hasPath) {
            agent.ResetPath();
        }
    }

    [Command]
    public void CmdMove(Vector3 position) {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
            return;
        }

        agent.SetDestination(hit.position);
    }

    #endregion
}