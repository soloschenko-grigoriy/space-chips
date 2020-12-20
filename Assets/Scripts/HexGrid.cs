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

        // A way to set all cell static so it will produce less batches
        StaticBatchingUtility.Combine(this.gameObject);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
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

    private void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, raycastHits);

        for (int i = 0; i < hits; i++) {
            var cell = raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell) {
                cell.ToggleIsActive();
                var range = CellsInRange(cell.coordinates, 3);
                foreach (var c in range) {
                    c.IsInRange = true;
                }

                break;
            }
        }
    }

    private List<HexCell> CellsInRange(HexCoordinates center, int range) {
        var result = new List<HexCell>();
        foreach (HexCell cell in cells) {
            if (cell.coordinates == center) {
                continue;
            }

            if (Mathf.Abs(cell.coordinates.X - center.X) > range) {
                continue;
            }

            if (Mathf.Abs(cell.coordinates.Y - center.Y) > range) {
                continue;
            }

            if (Mathf.Abs(cell.coordinates.Z - center.Z) > range) {
                continue;
            }

            result.Add(cell);
        }

        return result;
    }
}
