using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils
{
    public class Direction
    {
        public static Vector3 UP = new Vector3(0, 1, 0);
        public static Vector3 DOWN = new Vector3(0, -1, 0);
        public static Vector3 LEFT = new Vector3(-1, 0, 0);
        public static Vector3 RIGHT = new Vector3(1, 0, 0);
        public static Vector3 NONE = Vector3.zero;

        public static Vector3 UP_RIGHT = new Vector3(1, 1, 0).normalized;
        public static Vector3 UP_LEFT = new Vector3(-1, 1, 0).normalized;
        public static Vector3 DOWN_RIGHT = new Vector3(1, -1, 0).normalized;
        public static Vector3 DOWN_LEFT = new Vector3(-1, -1, 0).normalized;

        public static Vector3[] FOUR_DIRECTIONS = new Vector3[] { UP, DOWN, LEFT, RIGHT };
        public static Vector3[] EIGHT_DIRECTIONS = new Vector3[] { UP, DOWN, LEFT, RIGHT,
                                                            UP_RIGHT, UP_LEFT, DOWN_RIGHT, DOWN_LEFT };
        public static Vector3[] FOUR_OTHER_DIRECTIONS = new Vector3[] { UP_RIGHT, UP_LEFT, DOWN_RIGHT, DOWN_LEFT };
    };


    public static Vector3[] getRandomDirections()
    {
        Vector3[] result = new Vector3[4];
        bool[] selected = new bool[4] { false, false, false, false };
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            int r = UnityEngine.Random.Range(0, 4);
            while (selected[i])
            {
                r = UnityEngine.Random.Range(0, 4);
            }
            switch (r)
            {
                case 0:
                    result[count] = Direction.UP;
                    break;
                case 1:
                    result[count] = Direction.DOWN;
                    break;
                case 2:
                    result[count] = Direction.LEFT;
                    break;
                case 3:
                    result[count] = Direction.RIGHT;
                    break;
            }
            count++;
        }
        return result;
    }

    public static Vector3[] getDirectionFromCoordinates(Vector3 v, Vector3[] directions)
    {
        Array.Sort(directions, (a, b) => (Vector3.Dot(a, v).CompareTo(Vector3.Dot(b, v))));
        Array.Reverse(directions);
        return directions;
    }

    public static Vector3 findAvailableDirection(Vector3[] desiredDirection, Vector3[] availableDirection,
                                     Vector3[] forbiddenDirection, Vector3[] leastDesiredDirection)
    {
        foreach (Vector3 v in desiredDirection)
        {
            if (Array.IndexOf(forbiddenDirection, v) == -1)
            {
                return v;
            }
        }
        foreach (Vector3 v in Shuffle(availableDirection))
        {
            if (Array.IndexOf(forbiddenDirection, v) == -1 && Array.IndexOf(leastDesiredDirection, v) == -1)
            {
                return v;
            }
        }
        foreach (Vector3 v in Shuffle(availableDirection))
        {
            if (Array.IndexOf(forbiddenDirection, v) == -1)
            {
                return v;
            }
        }
        return Direction.NONE;
    }

    static Vector3[] Shuffle(Vector3[] v)
    {
        for (int i = 0; i < v.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, v.Length);
            Vector3 tempGO = v[rnd];
            v[rnd] = v[i];
            v[i] = tempGO;
        }
        return v;
    }
}