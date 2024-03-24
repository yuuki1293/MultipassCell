namespace MultipassCell.Core

open FSharp.Stats

type Mirror<'T> =
    /// <summary>
    /// Calculate the vector of the reflected laser.
    /// </summary>
    /// <param name="p0">starting point</param>
    /// <param name="r0">incidence vector</param>
    abstract reflect: p0:Vector<'T> -> r0:Vector<'T> -> Vector<'T> * Vector<'T>
    
    /// <summary>
    /// Normal vector at a point.
    /// </summary>
    /// <param name="p">point</param>
    abstract normal: p:Vector<'T> -> Vector<'T>