namespace MultipassCell.Core

open FSharp.Stats
open MultipassCell.Core.Types

type Surface =
    /// <summary>
    /// Returns information about the surface at the given XY coordinates.
    /// </summary>
    abstract surface: resolution: int -> corner1:Vector<float> -> corner2:Vector<float> -> XYZ<float>