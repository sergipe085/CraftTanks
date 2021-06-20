using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : NetworkBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;
    [SerializeField] private LayerMask layerMask;

    private CTPlayer player       = null;
    private Camera   mainCamera   = null;
    private Vector2  startDragPos = Vector2.zero;

    public List<Unit> SelectedUnits { get; } = new List<Unit>();

    private void Start() {
        mainCamera = Camera.main;
        player     = NetworkClient.connection.identity.GetComponent<CTPlayer>();
    }

    [ClientCallback]
    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            StartSelectionArea();
        } 
        else if (Mouse.current.leftButton.wasReleasedThisFrame) {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed) {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea() {
        foreach (Unit selectedUnit in SelectedUnits) {
            selectedUnit.Deselect();
        }
        SelectedUnits.Clear();

        unitSelectionArea.gameObject.SetActive(true);
        startDragPos = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }

    private void UpdateSelectionArea() {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float width  = mousePos.x - startDragPos.x;
        float height = mousePos.y - startDragPos.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        unitSelectionArea.anchoredPosition = startDragPos + new Vector2(width / 2, height / 2);
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude < 0.1f) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

            if (!unit.hasAuthority) { return; }

            SelectedUnits.Add(unit);

            foreach (Unit u in SelectedUnits) {
                u.Select();
            }

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach(Unit unit in player.GetMyUnits()) {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }
    }
}
