using UnityEngine;

public class OrthographicStrategy : IZoomStrategy {
    public OrthographicStrategy(Camera camera, float startingZoom) {
        camera.orthographicSize = startingZoom;
    }

    public void ZoomIn(Camera camera, float delta, float limit) {
        if (camera.orthographicSize == limit) {
            return;
        }

        camera.orthographicSize = Mathf.Max(camera.orthographicSize - delta, limit);
    }

    public void ZoomOut(Camera camera, float delta, float limit) {
        if (camera.orthographicSize == limit) {
            return;
        }

        camera.orthographicSize = Mathf.Min(camera.orthographicSize + delta, limit);
    }
}
