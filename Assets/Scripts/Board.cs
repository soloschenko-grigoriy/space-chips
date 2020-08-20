using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Node[] _nodes;

    [SerializeField]
    private Ship[] _ships;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < _nodes.Length; i++)
                {
                    if (hit.transform == _nodes[i].transform)
                    {
                        // _nodes[i].ToggleActive();
                        _ships[0].MoveTo(_nodes[i].transform.localPosition);
                    }
                }
            }
        }
    }
}
