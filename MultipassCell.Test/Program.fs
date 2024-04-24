open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats
open Plotly.NET
open Plotly.NET.LayoutObjects
open Plotly.NET.StyleParam

let refCount = 5
let sphere_r = Sphere(0.1, 0, -4, 9, 1)
let sphere_l = Sphere(0, 0, 4, 9, -1)
let p0 = vector [0.1493476; 0; -4.998761]
let r0 = vector [0.1493476+0.1221910; 0; -4.998761-4.997257]

let mutable laser = [(p0, r0)]

let rec proceed = function
    | 0, _ -> ()
    | count, true -> laser <- reflect sphere_r laser.Head :: laser; proceed (count-1, false)
    | count, false -> laser <- reflect sphere_l laser.Head :: laser; proceed (count-1, true)

proceed(refCount, true)

let cameraEye = CameraEye.init(1.25, 1.25, -1.25)
let cameraUp = CameraUp.init(5, 0, 0)
let camera = Camera.init(Eye = cameraEye, Up = cameraUp)
Chart.Line3D(
    x = [ for p, _ in laser -> p[0]]
    ,y = [ for p, _ in laser -> p[1]]
    ,z = [ for p, _ in laser -> p[2]]
    ,Camera = camera
)
// |> Chart.withSceneStyle(AspectMode = AspectMode.Data)
|> Chart.withSize(1000, 1000)
|> Chart.show

System.Console.ReadLine()