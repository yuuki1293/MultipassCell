open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats

let sphere_r = Sphere(0, 0, 1, 3)
let sphere_l = Sphere(0, 0, -1, 3)

let p0 = vector [1; 2; 4]
let r0 = vector [-1; -1; -1]

let mutable laser = (p0, r0)

for _ in [0..100] do
    laser <- reflect sphere_r laser
    printfn $"%A{laser}"
    laser <- reflect sphere_l laser
    printfn $"%A{laser}"