open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats

let sphere = Sphere(0, 0, 0, 3)

let p0 = vector [1; 2; 3]
let r0 = vector [-1; -1; -1]
(sphere :> Mirror<float>).reflect p0 r0
|> printfn "%A"