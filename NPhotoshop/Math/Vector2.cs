﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Maths
{
    internal class Vector2
    {
        public float x;
        public float y;
        public static float Dot(Vector2 a,  Vector2 b)
        {
            return (float)(a.x * b.x + a.y * b.y);
        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, float scalar)
        {
            return new Vector2(a.x * scalar, a.y * scalar);
        }
        public static Vector2 operator *(float scalar, Vector2 a)
        {
            return  a * scalar;
        }
        public static Vector2 operator /(Vector2 a, float scalar)
        {
            if (scalar == 0)
            {
                throw new DivideByZeroException("Division by zero");
            }

            return new Vector2(a.x / scalar, a.y / scalar);
        }

        public float Magnitude()
        {
            return (float)System.Math.Sqrt(x * x + y * y);
        }

        public Vector2 Normalize()
        {
            float mag = Magnitude();
            if (mag == 0)
            {
                throw new DivideByZeroException("Vector has zero magnitude");
            }

            return new Vector2(x / mag, y / mag);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
