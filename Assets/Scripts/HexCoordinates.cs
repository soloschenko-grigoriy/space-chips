using UnityEngine;
using System;

[System.Serializable]
public class HexCoordinates : IEquatable<HexCoordinates> {
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

    public override string ToString() {
        return $"({X.ToString()}, {Y.ToString()}, {Z.ToString()})";
    }

    public override bool Equals(object obj) {
        return base.Equals(obj as HexCoordinates);
    }

    public bool Equals(HexCoordinates obj) {
        if (obj.X != x || obj.Y != Y || obj.Z != Z) {
            return false;
        }

        return true;
    }

    public static bool operator ==(HexCoordinates lhs, HexCoordinates rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(HexCoordinates lhs, HexCoordinates rhs) {
        return !lhs.Equals(rhs);
    }

    public override int GetHashCode() {
        return X * Y * Z;
    }
}
