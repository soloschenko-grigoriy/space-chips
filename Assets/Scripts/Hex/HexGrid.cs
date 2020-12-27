using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    [SerializeField] int rows = 6;
    [SerializeField] int cols = 6;
    [SerializeField] HexCell hexCellPrefab = default;
    [SerializeField] Text cellLabelPrefab = default;

    public HexCell[] Cells { get; private set; }

    Canvas canvas;

    void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        Cells = new HexCell[(cols + 1) * (rows + 1)];

        for (int i = 0, col = -cols / 2; col < cols / 2 + 1; col++) {
            for (int row = -rows / 2; row < rows / 2 + 1; row++, i++) {
                Cells[i] = CreateCell(col, row);
            }
        }

        SetNeighbors();
        // A way to set all cell static so it will produce less batches
        StaticBatchingUtility.Combine(this.gameObject);
    }

    void SetNeighbors() {
        for (int i = 0; i < Cells.Length; i++) {
            var cell = Cells[i];
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

    public HexCell FindNeighborByCoordinates(int x, int y, int z) {
        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].coordinates.X != x) {
                continue;
            }

            if (Cells[i].coordinates.Y != y) {
                continue;
            }

            if (Cells[i].coordinates.Z != z) {
                continue;
            }

            return Cells[i];
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

    List<HexCell> CellsInRange(HexCoordinates center, int range) {
        var result = new List<HexCell>();
        foreach (HexCell cell in Cells) {
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

    public void SetTypeInRange(HexCell center, int range, HexCellHighlightType type) {
        foreach (var c in CellsInRange(center.coordinates, range)) {
            c.Type = type;
        }
    }

    public void ResetTypeForAll() {
        foreach (var cell in Cells) {
            cell.Type = HexCellHighlightType.Default;
        }
    }

    public void HighlightPath(HexCell from, HexCell to, HexCellHighlightType type) {
        var path = new AStarSearch(to, from);
        from.Type = HexCellHighlightType.Default;

        foreach (var item in path.cameFrom) {
            if (item.Value) {
                item.Value.Type = type;
            }
        }
    }
}
