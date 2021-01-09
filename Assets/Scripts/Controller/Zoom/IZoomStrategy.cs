using UnityEngine;

public interface IZoomStrategy {
    void ZoomIn(Camera cam, float delta, float limit);
    void ZoomOut(Camera cam, float delta, float limit);
}
