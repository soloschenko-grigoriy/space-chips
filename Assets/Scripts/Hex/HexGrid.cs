using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public delegate void OnReady();
    public HexCell[] Cells { get; private set; }
    public bool IsReady { get; private set; } = false;

    enum HexShapeType {
        Hex, Rombus
    }

    [SerializeField] int rows = 6;
    [SerializeField] int cols = 6;
    [SerializeField] HexCell hexCellPrefab = default;
    [SerializeField] Text cellLabelPrefab = default;

    Canvas canvas;

    public void Generate(OnReady onReady) {
        canvas = GetComponentInChildren<Canvas>();
        GenerateAsHex();
        // GenerateAsRombus();

        SetNeighbors();

        // A way to set all cell static so it will produce less batches
        StaticBatchingUtility.Combine(this.gameObject);

        IsReady = true;
        onReady();
    }
    void GenerateAsHex() {
        var m = rows - 1;
        var cells = new List<HexCell>();

        for (int i = 0, row = -m; row < rows; row++) {
            var to = m;
            var from = -m;
            if (row < 0) {
                from = -m - row;
                to = m;
            }
            else if (row > 0) {
                from = -m;
                to = m - row;
            }

            for (int col = from; col <= to; col++, i++) {
                cells.Add(CreateCell(col, row, HexShapeType.Hex));
            }
        }

        Cells = cells.ToArray();
    }

    void GenerateAsRombus() {
        Cells = new HexCell[(cols + 1) * (rows + 1)];

        for (int i = 0, col = -cols / 2; col < cols / 2 + 1; col++) {
            for (int row = -rows / 2; row < rows / 2 + 1; row++, i++) {
                Cells[i] = CreateCell(col, row, HexShapeType.Rombus);
            }
        }
    }

    public HexCell FindBy(int x, int y, int z) {
        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].Coordinates.X != x) {
                continue;
            }

            if (Cells[i].Coordinates.Y != y) {
                continue;
            }

            if (Cells[i].Coordinates.Z != z) {
                continue;
            }

            return Cells[i];
        }

        return null;
    }

    public HexCell FindBy(HexCoordinates coordinates) {
        return FindBy(coordinates.X, coordinates.Y, coordinates.Z);
    }

    public HexCell FindBy(Vector2Int coordinates) {
        return FindBy(HexCoordinates.FromVector2(coordinates));
    }

    public HexCell[] FindAllOccupiedByEnemyInRange(HexCoordinates center, int range) =>
        Array.FindAll(Cells, (cell) => {
            if (cell.OccupiedBy == null) {
                return false;
            }

            if (cell.OccupiedBy.Fleet.Owner == FleetOwner.Player) {
                return false;
            }

            return ChecOnekInRange(cell.Coordinates, center, range);
        });

    public HexCell[] FindAllEmptyInRange(HexCoordinates center, int range) =>
        Array.FindAll(Cells, (cell) => {
            if (cell.OccupiedBy != null) {
                return false;
            }

            return ChecOnekInRange(cell.Coordinates, center, range);
        });

    void SetNeighbors() {
        for (int i = 0; i < Cells.Length; i++) {
            var cell = Cells[i];
            cell.Neighbors = new HexCell[6];
            // NE
            cell.Neighbors[0] = FindBy(cell.Coordinates.X, cell.Coordinates.Y - 1, cell.Coordinates.Z + 1);
            // E 
            cell.Neighbors[1] = FindBy(cell.Coordinates.X + 1, cell.Coordinates.Y - 1, cell.Coordinates.Z);
            // SE 
            cell.Neighbors[2] = FindBy(cell.Coordinates.X + 1, cell.Coordinates.Y, cell.Coordinates.Z - 1);
            // SW 
            cell.Neighbors[3] = FindBy(cell.Coordinates.X, cell.Coordinates.Y + 1, cell.Coordinates.Z - 1);
            // W 
            cell.Neighbors[4] = FindBy(cell.Coordinates.X - 1, cell.Coordinates.Y + 1, cell.Coordinates.Z);
            // NW
            cell.Neighbors[5] = FindBy(cell.Coordinates.X - 1, cell.Coordinates.Y, cell.Coordinates.Z + 1);
        }
    }

    HexCell CreateCell(int col, int row, HexShapeType type) {
        var position = new Vector3();
        if (type == HexShapeType.Rombus) {
            position.x = (col + row * 0.5f - row / 2) * HexCell.Width;
        }
        else {
            position.x = (col + row * 0.5f) * HexCell.Width;
        }

        position.y = 0f;
        position.z = row * HexCell.Height * 0.75f;

        var cell = Instantiate<HexCell>(hexCellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        if (type == HexShapeType.Rombus) {
            cell.Coordinates = HexCoordinates.FromOffsetCoordinates(col, row);
        }
        else {
            cell.Coordinates = new HexCoordinates(col, row);
        }

        var label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(canvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        // label.text = cell.Coordinates.ToString();

        return cell;
    }

    bool ChecOnekInRange(HexCoordinates cell, HexCoordinates center, int range) {
        if (cell == center) {
            return false;
        }

        if (Math.Abs(cell.X - center.X) > range) {
            return false;
        }

        if (Math.Abs(cell.Y - center.Y) > range) {
            return false;
        }

        if (Math.Abs(cell.Z - center.Z) > range) {
            return false;
        }

        return true;
    }
}
