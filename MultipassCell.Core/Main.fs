[<AutoOpen>]
module MultipassCell.Core.Main

open FSharp.Stats

let reflect (m: 'T :> Mirror<'U>) (p0: Vector<'U>, r0: Vector<'U>) = m.reflect p0 r0
let surface (m: 'T :> Surface<'U>) (resolution: int) (corner1:Vector<'U>) (corner2:Vector<'U>) = m.surface resolution corner1 corner2