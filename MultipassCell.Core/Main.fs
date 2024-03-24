[<AutoOpen>]
module MultipassCell.Core.Main

open FSharp.Stats

let reflect (m: 'T :> Mirror<'U>) (p0: Vector<'U>) (r0: Vector<'U>) = m.reflect p0 r0
