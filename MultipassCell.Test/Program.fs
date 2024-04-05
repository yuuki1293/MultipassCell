open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats
open Plotly.NET

let sphere_r = Sphere(0, 0, -500, 1000, 1)
let sphere_l = Sphere(0, 0, 500, 1000, -1)
let p0 = vector [200; 0; -479]
let r0 = vector [-200; 200; 959.6]

let mutable laser = [(p0, r0)]

for _ in [0..100] do
    laser <- reflect sphere_r laser.Head :: laser
    // printfn $"%A{laser}"
    laser <- reflect sphere_l laser.Head :: laser
    // printfn $"%A{laser}"
    
Chart.Point3D(
    x = [ for p, _ in laser -> p[0]],
    y = [ for p, _ in laser -> p[1]],
    z = [ for p, _ in laser -> p[2]]
    )
|> Chart.show