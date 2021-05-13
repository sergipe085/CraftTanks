using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

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
            if (!hasAuthority || !Input.GetKeyDown(KeyCode.Mouse1))
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CmdMove(hit.point);
            }
        }

        #endregion
    }
}