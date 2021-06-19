using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] private LayerMask            layerMask            = new LayerMask();

    private Camera mainCamera = null;

    private void Start() {
        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update() {
        if (Mouse.current.rightButton.wasPressedThisFrame) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                TryMove(hit.point);
            }
        }
    }

    private void TryMove(Vector3 point) {
        foreach (Unit selectedUnit in unitSelectionHandler.SelectedUnits) {
            selectedUnit.GetMovement().CmdMove(point);
        }
    }
}
