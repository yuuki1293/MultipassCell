open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats
open Plotly.NET

let sphere_r = Sphere(0, 0, -4.79795897113, 10, 1)
let sphere_l = Sphere(0, 0, 4.79795897113, 10, -1)
let p0 = vector [2; 0; -4.79795897113]
let r0 = vector [-2; 2; 9.59591794226]

let mutable laser = [(p0, r0)]

for _ in [0..2] do
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