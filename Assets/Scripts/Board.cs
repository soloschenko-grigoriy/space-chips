using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject _nodePrefab = default;

    [SerializeField, Range(1, 10)]
    private int _size = default;

    private Node[] _nodes;

    void Start()
    {
        _nodes = new Node[_size * _size];

        for (int z = 0, i = 0; z < _size; z++)
        {
            for (int x = 0; x < _size; x++, i++)
            {
                var node = Instantiate(_nodePrefab);
                node.transform.parent = transform;
                node.transform.localPosition = new Vector3(x, 0, z);

                _nodes[i] = node.GetComponent<Node>();
            }
        }
    }

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
                        _nodes[i].Click();
                    }
                }
            }
        }
    }
}
