﻿open System.IO
open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Data.JsonProvider
open FSharp.Stats
open Plotly.NET
open Plotly.NET.LayoutObjects
open Plotly.NET.StyleParam

let refCount = 5

type Sample = JsonProvider<"./data/sample.json", EmbeddedResource="MultipassCell.Test, MultipassCell.Test.data.sample.json">

let sample = File.ReadAllText "./data/sample.json" |> Sample.Parse 
let square = sample.SquareReal
let sphere_r = Sphere (square.SphereR |> Array.map float |> Array.toList)
let sphere_l = Sphere (square.SphereL |> Array.map float |> Array.toList)
let p0 = solve sphere_l (square.P0 |> Array.map float |> vector)
let r0 =
    let p1 = solve sphere_r (square.P1 |> Array.map float |> vector)
    p1 - p0

let rec proceed = function
    | _, 0, _ -> []
    | pr, count, true -> pr :: proceed (reflect sphere_r pr, count - 1, false)
    | pr, count, false -> pr :: proceed (reflect sphere_l pr, count - 1, true)

let laser = proceed((p0, r0), refCount, true)

let sphere_r_list = surface sphere_r 10 (vector [25; 25]) (vector [-25; -25])
let sphere_l_list = surface sphere_l 10 (vector [25; 25]) (vector [-25; -25])

let surface xyz =
    Chart.Surface(
        X = (let x, _, _ = xyz in x)
        ,Y = (let _, y, _ = xyz in y)
        ,zData = (let _, _, z = xyz in z)
        ,ColorScale = Colorscale.Custom(seq [0.0, Color.fromARGB 64 0 0 0; 1.0, Color.fromARGB 64 0 0 0])
        ,ShowScale = false
    )

let cameraEye = CameraEye.init(1.25, 1.25, -1.25)
let cameraUp = CameraUp.init(5, 0, 0)
let camera = Camera.init(Eye = cameraEye, Up = cameraUp)
[
    Chart.Line3D(
    x = [ for p, _ in laser -> p[0]]
    ,y = [ for p, _ in laser -> p[1]]
    ,z = [ for p, _ in laser -> p[2]]
    ,Camera = camera
    )
    surface sphere_r_list
    surface sphere_l_list
]
|> Chart.combine
// |> Chart.withSceneStyle(AspectMode = AspectMode.Data)
|> Chart.withSize(1000, 1000)
|> Chart.show

System.Console.ReadLine()
