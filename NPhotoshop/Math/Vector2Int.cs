using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Math
{
    public class Vector2Int
    {
        public int x;
        public int y;
        public static int Dot(Vector2Int a, Vector2Int b)
        {
            return (int)(a.x * b.x + a.y * b.y);
        }
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator *(Vector2Int a, int scalar)
        {
            return new Vector2Int(a.x * scalar, a.y * scalar);
        }
        public static Vector2Int operator *(int scalar, Vector2Int a)
        {
            return a * scalar;
        }
        public static Vector2Int operator /(Vector2Int a, int scalar)
        {
            if (scalar == 0)
            {
                throw new DivideByZeroException("Division by zero");
            }

            return new Vector2Int(a.x / scalar, a.y / scalar);
        }

        public int Magnitude()
        {
            return (int)System.Math.Sqrt(x * x + y * y);
        }

        public Vector2Int Normalize()
        {
            int mag = Magnitude();
            if (mag == 0)
            {
                throw new DivideByZeroException("Vector has zero magnitude");
            }

            return new Vector2Int(x / mag, y / mag);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
