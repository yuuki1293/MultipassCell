[<AutoOpen>]
module MultipassCell.Core.Main

open FSharp.Stats

let reflect (m: 'T :> Mirror) (p0: Vector<float>, r0: Vector<float>) = m.reflect p0 r0
let surface (m: 'T :> Surface) (resolution: int) (corner1:Vector<float>) (corner2:Vector<float>) = m.surface resolution corner1 corner2
let solve (m: 'T :> Mirror) (p: Vector<float>) = m.solve p