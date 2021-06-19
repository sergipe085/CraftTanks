using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : NetworkBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    private Camera mainCamera = null;

    private List<Unit> selectedUnits = new List<Unit>();

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            //Start Selection Area
            foreach(Unit selectedUnit in selectedUnits) {
                selectedUnit.Deselect();
            }
            selectedUnits.Clear();
        } 
        else if (Mouse.current.leftButton.wasReleasedThisFrame) {
            ClearSelectedArea();
        }
    }

    private void ClearSelectedArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

        if (!unit.hasAuthority) { return; }

        selectedUnits.Add(unit);

        foreach (Unit u in selectedUnits) {
            u.Select();
        }
    }
}
