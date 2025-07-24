using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtility 
{
	public static float GetHorizontal()
	{
		return Input.GetAxis("Horizontal");
	}
	public static float GetVertical()
	{
		return Input.GetAxis("Vertical");
	}

	public static float GetMouseScrollWheel()
	{
		return Input.GetAxis("Mouse ScrollWheel");
	}
}
