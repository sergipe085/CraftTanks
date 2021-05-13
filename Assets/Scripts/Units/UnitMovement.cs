using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using UnityEngine.InputSystem;

namespace CraftTanks.Units 
{
    public class UnitMovement : NetworkBehaviour
    {
        [Header("CORE")]
        private NavMeshAgent agent = null;
        private Camera mainCamera;

        #region Server

        [Command]
        private void CmdMove(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                return;
            }

            agent.SetDestination(hit.position);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            agent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority || !Mouse.current.rightButton.IsPressed())
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CmdMove(hit.point);
            }
        }

        #endregion
    }
}