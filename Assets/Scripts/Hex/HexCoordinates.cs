using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    [SerializeField]
    private int x, z;

    public int X => x;
    public int Z => z;
    public int Y => -x - z;

    public HexCoordinates(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z) {
        return new HexCoordinates(x - z / 2, z);
    }

    public static HexCoordinates FromVector2(Vector2Int coordinates) {
        return new HexCoordinates(coordinates.x, coordinates.y);
    }

    public override string ToString() {
        return $"({X.ToString()}, {Y.ToString()}, {Z.ToString()})";
    }

    public override bool Equals(object obj) =>
        obj is HexCoordinates coord && X == coord.X && Y == coord.Y && Z == coord.Z;

    public override int GetHashCode() => X ^ Y ^ Z;

    public static bool operator ==(HexCoordinates lhs, HexCoordinates rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(HexCoordinates lhs, HexCoordinates rhs) {
        return !lhs.Equals(rhs);
    }
}
