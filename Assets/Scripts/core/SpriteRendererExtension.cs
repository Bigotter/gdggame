﻿using System;
using UnityEngine;

public static class SpriteRendererExtension
{
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}


