namespace MultipassCell.Core

open FSharp.Stats

type Mirror =
    /// <summary>
    /// Calculate the vector of the reflected laser.
    /// </summary>
    /// <param name="p0">starting point</param>
    /// <param name="r0">incidence vector</param>
    abstract reflect: p0:Vector<float> -> r0:Vector<float> -> Vector<float> * Vector<float>
    
    /// <summary>
    /// Normal vector at a point.
    /// </summary>
    /// <param name="p">point</param>
    abstract normal: p:Vector<float> -> Vector<float>
    
    /// <summary>
    /// Solve x, y and z from x and y
    /// </summary>
    /// <param name="p"></param>
    abstract solve: p:Vector<float> -> Vector<float>