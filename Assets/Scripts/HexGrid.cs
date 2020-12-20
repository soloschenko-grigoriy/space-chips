using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    [SerializeField]
    int rows = 6;

    [SerializeField]
    int cols = 6;

    [SerializeField] HexCell hexCellPrefab;
    [SerializeField] Text cellLabelPrefab;

    HexCell[] cells;
    Canvas canvas;

    RaycastHit[] raycastHits = new RaycastHit[100];

    void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[cols * rows];

        for (int i = 0, z = 0; i < cols; i++, z++) {
            for (int x = 0; x < rows; x++) {
                cells[i] = CreateCell(x, z);
            }
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
    }

    HexCell CreateCell(int x, int z) {
        var offset = (z % 2 == 0) ? 0f : HexCell.Width / 2;
        var position = new Vector3(x * HexCell.Width + offset, 0, z * HexCell.Height * 0.75f);

        var cell = Instantiate<HexCell>(hexCellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        cell.SetCoordinates(x, z);

        var label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(canvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.GetCoordinates();

        return cell;
    }


    private void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, raycastHits);

        for (int i = 0; i < hits; i++) {
            var cell = raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell) {
                cell.ToggleIsActive();
                Debug.Log(cell.GetCoordinates());
            }
        }
    }
}
