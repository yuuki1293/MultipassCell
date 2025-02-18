module MultipassCell.Test.UnitTest1

open MultipassCell.Core.Calc
open MultipassCell.Core.McManus.Vectors
open NUnit.Framework
open MultipassCell.Test.SampleData
open MultipassCell.Core
open MultipassCell.Core.Common
open FSharp.Stats
open Plotly.NET
open Plotly.NET.LayoutObjects
open Plotly.NET.StyleParam

[<Test>]
let SquareReal () =
    let refCount = 18

    let square = sample.Sample32
    let sphere_r = Sphere(square.SphereR |> Array.map float |> Array.toList)
    let sphere_l = Sphere(square.SphereL |> Array.map float |> Array.toList)
    let p0 = solve sphere_l (square.P0 |> Array.map float |> vector)
    let p1 = solve sphere_l (square.P1 |> Array.map float |> vector)

    let laser = calc sphere_r sphere_l p0 p1 refCount

    let sphere_r_list = surface sphere_r 10 (vector [ 25; 25 ]) (vector [ -25; -25 ])
    let sphere_l_list = surface sphere_l 10 (vector [ 25; 25 ]) (vector [ -25; -25 ])

    let surface xyz =
        Chart.Surface(
            X = (let x, _, _ = xyz in x),
            Y = (let _, y, _ = xyz in y),
            zData = (let _, _, z = xyz in z),
            ColorScale =
                Colorscale.Custom(
                    seq [ 0.0, Color.fromARGB 64 0 0 0
                          1.0, Color.fromARGB 64 0 0 0 ]
                ),
            ShowScale = false
        )

    let cameraEye = CameraEye.init (1.25, 1.25, -1.25)
    let cameraUp = CameraUp.init (5, 0, 0)
    let camera = Camera.init (Eye = cameraEye, Up = cameraUp)

    let toxyz sphere p =
        let xyz = solve sphere (p |> vector)
        xyz[0], xyz[1], xyz[2]

    [ Chart.Line3D(
          x = [ for p, _ in laser -> p[0] ],
          y = [ for p, _ in laser -> p[1] ],
          z = [ for p, _ in laser -> p[2] ],
          Camera = camera
      )
      // surface sphere_r_list
      // surface sphere_l_list
      Chart.Point3D(
          [ [ 15.366; 2.806 ]
            [ -11.782; -11.186 ]
            [ 6.962; 9.714 ]
            [ -1.002; -15.41 ]
            [ -4.962; 10.858 ]
            [ 10.306; -13.43 ]
            [ -14.29; 5.754 ]
            [ 16.73; -5.95 ] ]
          |> List.map (toxyz sphere_r)
      )
      Chart.Point3D(
          [ [ 6.062; -10.042 ]
            [ -8.282; 14.29 ]
            [ 15.522; -5.114 ]
            [ -14.706; 6.898 ]
            [ 18.25; 3.774 ]
            [ -13.43; -2.298 ]
            [ 13.014; 12.442 ]
            [ -5.07; -9.03 ]
            [ 2.278; 16.49 ] ]
          |> List.map (toxyz sphere_l)
      ) ]
    |> Chart.combine
    // |> Chart.withSceneStyle(AspectMode = AspectMode.Data)
    |> Chart.withSize (1000, 1000)
    |> Chart.show

[<Test>]
let ApplyD () =
    let X = Vectors.X 0 0 2 1 1
    let D = Vectors.D 400
    let t = Vectors.t 1

    X.ToArray() |> printfn "X = %A\nd = 2"
    (D * X).ToArray() |> printfn "D.X = %A"
    (t * X).ToArray() |> printfn "t.X = %A"
