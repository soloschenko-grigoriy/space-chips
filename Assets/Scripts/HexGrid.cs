using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    [SerializeField]
    int rows = 6;

    [SerializeField]
    int cols = 6;

    [SerializeField] HexCell hexCellPrefab = default;
    [SerializeField] Text cellLabelPrefab = default;

    HexCell[] cells;
    Canvas canvas;

    RaycastHit[] raycastHits = new RaycastHit[100];

    void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[cols * rows];

        for (int i = 0, col = 0; col < cols; i++, col++) {
            for (int row = 0; row < rows; row++) {
                cells[i] = CreateCell(col, row);
            }
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
    }

    HexCell CreateCell(int col, int row) {
        // var offset = (row % 2 == 0) ? 0f : HexCell.Width / 2;
        // if(row % 2 == 0){
        //     col -= 1
        // }
        // * HexCell.Width
        // (row + col * 0.5f - row / 2) + HexCell.Width
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
            }
        }
    }
}
