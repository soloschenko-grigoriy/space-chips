using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    public Node[] Nodes;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < Nodes.Length; i++)
                {
                    if (hit.transform == Nodes[i].transform)
                    {
                        Nodes[i].ToggleActive();
                    }
                }
            }
        }
    }
}
