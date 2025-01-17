using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate TResult InterpolationFunk<in T, out TResult>(T a, T b, float t);

public static partial class Extensions
{
    public static T Lerp<T>(T start, T end, float t, InterpolationFunk<T, T> lerpFunk, params InterpolationExtremumPoints<T>[] extremumPoints)
    {
        List<InterpolationExtremumPoints<T>> points = new List<InterpolationExtremumPoints<T>>(extremumPoints);
        points.Insert(0, new InterpolationExtremumPoints<T> { point = start, extremum = 0 });
        points.Insert(points.Count, new InterpolationExtremumPoints<T> { point = end, extremum = 1 });

        t = Mathf.Clamp01(t);
        int index = 0;
        for (int i = 0; i < points.Count - 1; i++)
            if (points[i].extremum < t && points[i + 1].extremum >= t)
                index = i;

        float newT = Mathf.InverseLerp(points[index].extremum, points[index + 1].extremum, t);
        return lerpFunk.Invoke(points[index].point, points[index + 1].point, newT);
    }

    public static float Interpolation(float oldMin, float oldMax, float oldT, float newMin, float newMax)
    {
        return ((oldT - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
    }

    public static float Sinusoida(float amplitude, float angularFrewquency, float t, float horizontalShift, float verticalShift)
        => amplitude * Mathf.Sin(angularFrewquency * (t - horizontalShift)) + verticalShift;

}

public class InterpolationExtremumPoints<T>
{
    public T point;
    public float extremum;
}