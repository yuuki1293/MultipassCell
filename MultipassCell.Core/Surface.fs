namespace MultipassCell.Core

open FSharp.Stats
open MultipassCell.Core.Types

type Surface<'T> =
    /// <summary>
    /// Returns information about the surface at the given XY coordinates.
    /// </summary>
    abstract surface: resolution: int -> corner1:Vector<'T> -> corner2:Vector<'T> -> XYZ<'T>