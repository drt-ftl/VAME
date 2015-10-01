using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSegment
{
	private Vector3 _p1;
	private Vector3 _p2;

    public List<VoxelClass> CrossesVoxels { get; set; }
	public Color LineColor
	{ 
		set
		{
 			if (Line != null)
			{
				Line.SetColors(value, value);
			}
		}
	}

	public float LineWidth
	{ 
		set
		{
			if (Line != null)
			{
				Line.SetWidth(value, value);
			}
		}
	}

	public Vector3 p1 
	{ 
		get{ return _p1; }
		set
		{ 
			_p1 = value;
			Line.SetPosition(0, _p1);
		}
	}
	public Vector3 p2 
	{ 
		get{ return _p2; }
		set
		{ 
			_p2 = value;
			Line.SetPosition(1, _p2);
		}
	}

	public int step {get; set; }


	public LineRenderer Line
	{ get; set; }


}
