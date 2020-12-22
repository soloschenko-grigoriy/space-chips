using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    [SerializeField] int rows = 6;
    [SerializeField] int cols = 6;
    [SerializeField] HexCell hexCellPrefab = default;
    [SerializeField] Text cellLabelPrefab = default;

    HexCell[] cells;
    Canvas canvas;
    RaycastHit[] raycastHits = new RaycastHit[100];

    void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[(cols + 1) * (rows + 1)];

        for (int i = 0, col = -cols / 2; col < cols / 2 + 1; col++) {
            for (int row = -rows / 2; row < rows / 2 + 1; row++, i++) {
                cells[i] = CreateCell(col, row);
            }
        }

        SetNeighbors();
        // A way to set all cell static so it will produce less batches
        StaticBatchingUtility.Combine(this.gameObject);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
    }

    void SetNeighbors() {
        for (int i = 0; i < cells.Length; i++) {
            var cell = cells[i];
            cell.Neighbors = new HexCell[6];
            // NE
            cell.Neighbors[0] = FindNeighborByCoordinates(cell.coordinates.X, cell.coordinates.Y - 1, cell.coordinates.Z + 1);
            // E 
            cell.Neighbors[1] = FindNeighborByCoordinates(cell.coordinates.X + 1, cell.coordinates.Y - 1, cell.coordinates.Z);
            // SE 
            cell.Neighbors[2] = FindNeighborByCoordinates(cell.coordinates.X + 1, cell.coordinates.Y, cell.coordinates.Z - 1);
            // SW 
            cell.Neighbors[3] = FindNeighborByCoordinates(cell.coordinates.X, cell.coordinates.Y + 1, cell.coordinates.Z - 1);
            // W 
            cell.Neighbors[4] = FindNeighborByCoordinates(cell.coordinates.X - 1, cell.coordinates.Y + 1, cell.coordinates.Z);
            // NW
            cell.Neighbors[5] = FindNeighborByCoordinates(cell.coordinates.X - 1, cell.coordinates.Y, cell.coordinates.Z + 1);
        }
    }

    HexCell FindNeighborByCoordinates(int x, int y, int z) {
        for (int i = 0; i < cells.Length; i++) {
            if (cells[i].coordinates.X != x) {
                continue;
            }

            if (cells[i].coordinates.Y != y) {
                continue;
            }

            if (cells[i].coordinates.Z != z) {
                continue;
            }

            return cells[i];
        }

        return null;
    }

    HexCell CreateCell(int col, int row) {
        var position = new Vector3();
        position.x = (col + row * 0.5f - row / 2) * HexCell.Width;
        position.y = 0f;
        position.z = row * HexCell.Height * 0.75f;

        var cell = Instantiate<HexCell>(hexCellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(col, row);

        var label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(canvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToString();

        return cell;
    }

    void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, raycastHits);

        for (int i = 0; i < hits; i++) {
            var cell = raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell) {
                // HighlightPath(cells[cells.Length - 1], cell);
                HighlightInRange(cell, 3);
                cell.ToggleIsActive();
                break;
            }
        }
    }

    List<HexCell> CellsInRange(HexCoordinates center, int range) {
        var result = new List<HexCell>();
        foreach (HexCell cell in cells) {
            if (cell.coordinates == center) {
                continue;
            }

            if (Math.Abs(cell.coordinates.X - center.X) > range) {
                continue;
            }

            if (Math.Abs(cell.coordinates.Y - center.Y) > range) {
                continue;
            }

            if (Math.Abs(cell.coordinates.Z - center.Z) > range) {
                continue;
            }

            result.Add(cell);
        }

        return result;
    }

    void HighlightInRange(HexCell center, int range) {
        foreach (var c in CellsInRange(center.coordinates, range)) {
            c.IsInRange = true;
        }
    }

    void HighlightPath(HexCell from, HexCell to) {
        var path = new AStarSearch(to, from);
        from.IsInPath = true;
        foreach (var item in path.cameFrom) {
            if (item.Value) {
                item.Value.IsInPath = true;
            }
        }
    }
}
