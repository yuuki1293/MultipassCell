open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats
open Plotly.NET

let sphere_r = Sphere(0, 0, -4, 9, 1)
let sphere_l = Sphere(0, 0, 4, 9, -1)
let p0 = vector [5.1231; 0; -4]
let r0 = vector [4.1231; 4.1231; -8]

let mutable laser = [(p0, r0)]

for _ in [0..49] do
    laser <- reflect sphere_r laser.Head :: laser
    // printfn $"%A{laser}"
    laser <- reflect sphere_l laser.Head :: laser
    // printfn $"%A{laser}"
    
Chart.Line3D(
    x = [ for p, _ in laser -> p[0]],
    y = [ for p, _ in laser -> p[1]],
    z = [ for p, _ in laser -> p[2]]
)
|> Chart.withSize(1000, 1000)
|> Chart.show
