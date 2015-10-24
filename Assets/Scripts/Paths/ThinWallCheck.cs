using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThinWallCheck
{
    public static float connectedDistance = 0.0015f;

    public ThinWallCheck(float _connectedDistance)
    {
        connectedDistance = _connectedDistance;
        var alreadyChecked = new List<LineSegment>();
        foreach (var line in LoadFile.gcdLines)
        {
            alreadyChecked.Add(line);
            foreach (var other in LoadFile.gcdLines)
            {
                Vector2 a = Vector2.zero;
                Vector2 b = Vector2.zero;
                var check = false;
                if (!alreadyChecked.Contains(other) && line.p1.z - other.p1.z <= connectedDistance)
                {
                    if (line.MovesInX && other.MovesInX)
                    {
                        if (line.p1.y != other.p1.y && line.p1.z != other.p1.z)
                        {
                            a = new Vector2(line.p1.y, line.p1.z);
                            b = new Vector2(other.p1.y, other.p1.z);
                            check = true; 
                        }
                    }
                    else if (line.MovesInZ && other.MovesInZ)
                    {
                        if (line.p1.x != other.p1.x && line.p1.y != other.p1.y)
                        {
                            a = new Vector2(line.p1.x, line.p1.y);
                            b = new Vector2(other.p1.x, other.p1.y);
                            check = true;
                        }
                    }
                    if (check)
                    {
                        if (Vector2.Distance(a, b) <= connectedDistance)
                        {

                        }
                    }
                }
            }
        }
    }
}
