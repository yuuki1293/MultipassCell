module Program

open System
open FSharp.Stats
open FSharpPlus
open MultipassCell.Core
open MultipassCell.Core.Calc
open MultipassCell.Core.Common
open MultipassCell.Test.SampleData
open Plotly.NET
open Plotly.NET.LayoutObjects
open Plotly.NET.StyleParam

type ErrResult = struct
        val Err: float
        val Dxr: float
        val Dyr: float
        val Dzr: float
        val Dxl: float
        val Dyl: float
        val Dzl: float
        new (err, dxr, dyr, dzr, dxl, dyl, dzl) =
            { Err = err; Dxr = dxr; Dyr = dyr; Dzr = dzr; Dxl = dxl; Dyl = dyl; Dzl = dzl}
        member m.format = (m.Err, m.Dxr, m.Dyr, m.Dzr, m.Dxl, m.Dyl, m.Dzl)
    end

[<EntryPoint>]
let main _ =
    let refCount = 18
    let square = sample.Sample3

    let toxyz sphere (x, y) =
        let xyz = solve sphere (vector [ x; y ])
        xyz[0], xyz[1], xyz[2]

    let true_r =
        [ -13.806, 3.486
          15.366, -2.806
          -11.782, 11.186
          6.962, -9.714
          -1.002, 15.41
          -4.962, -10.858
          10.306, 13.43
          -14.29, -5.754
          16.73, 5.95 ]

    let true_l =
        [ 6.062, -10.042
          -8.282, 14.29
          15.522, -5.114
          -14.706, 6.898
          18.25, 3.774
          -13.43, -2.298
          13.014, 12.442
          -5.07, -9.03
          2.278, 16.49 ]

    let mutable min_err = ErrResult(Double.MaxValue, 0, 0, 0, 0, 0, 0)
    let mutable p_sphere_r = Sphere.Zero
    let mutable p_sphere_l = Sphere.Zero
    let mutable p_laser = List.Empty
    let mutable p_true_r = List.Empty
    let mutable p_true_l = List.Empty
    
    let min_dxr, step_dxr, max_dxr = -10.0, 2.0, 10.0
    let min_dyr, step_dyr, max_dyr = -10.0, 2.0, 10.0
    let min_dzr, step_dzr, max_dzr = -10.0, 2.0, 10.0
    let min_dxl, step_dxl, max_dxl = -10.0, 2.0, 10.0
    let min_dyl, step_dyl, max_dyl = -10.0, 2.0, 10.0
    let min_dzl, step_dzl, max_dzl = -10.0, 2.0, 10.0
    seq {
        for dxr in min_dxr..step_dxr..max_dxr do
        for dyr in min_dyr..step_dyr..max_dyr do
        for dzr in min_dzr..step_dzr..max_dzr do
        for dxl in min_dxl..step_dxl..max_dxl do
        for dyl in min_dyl..step_dyl..max_dyl do
        for dzl in min_dzl..step_dzl..max_dzl do
            // load variables
            let list_r = square.SphereR |> Array.map float |> Array.toList
            let list_l = square.SphereL |> Array.map float |> Array.toList
            let sphere_r = Sphere([list_r[0] + dxr; list_r[1] + dyr; list_r[2] + dzr; list_r[3]; list_r[4]])
            let sphere_l = Sphere([list_l[0] + dxl; list_l[1] + dyl; list_l[2] + dzl; list_l[3]; list_l[4]])
            let p0 = solve sphere_l (square.P0 |> Array.map float |> vector)
            let p1 = solve sphere_r (square.P1 |> Array.map float |> vector)
            
            // calculate laser points
            let laser = calc sphere_r sphere_l p0 p1 refCount
            let points = [ for p, _ in laser -> p[0], p[1], p[2] ]
            let comp_l, comp_r = points |> List.partition (fun (_, _, z) -> z < 0)
            
            let true_r_ = List.map (toxyz sphere_r) true_r
            let true_l_ = List.map (toxyz sphere_l) true_l
            
            monad.fx {
                let! err_r = error true_r_ comp_r
                let! err_l = error true_l_ comp_l
                let err = err_r + err_l
                let result = ErrResult(err, dxr, dyr, dzr, dxl, dyl, dzl)
                do
                    printfn $"%A{result.format}"
                    if err < min_err.Err then
                        printfn "updated!"
                        min_err <- result
                        p_sphere_r <- sphere_r
                        p_sphere_l <- sphere_l
                        p_laser <- laser
                        p_true_r <- true_r_
                        p_true_l <- true_l_
                return! None
            }
    } |> Seq.toList |> ignore

    printfn $"%A{min_err.format}"
    printfn $"%A{(min_dxr, step_dxr, max_dxr, min_dyr, step_dyr, max_dyr, min_dzr, step_dzr, max_dzr, min_dxl, step_dxl, max_dxl, min_dyl, step_dyl, max_dyl, min_dzl, step_dzl, max_dzl)}"
    
    // render logic
    let sphere_r_list = surface p_sphere_r 10 (vector [ 25; 25 ]) (vector [ -25; -25 ])
    let sphere_l_list = surface p_sphere_l 10 (vector [ 25; 25 ]) (vector [ -25; -25 ])

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

    [ Chart.Line3D(
          x = [ for p, _ in p_laser -> p[0] ],
          y = [ for p, _ in p_laser -> p[1] ],
          z = [ for p, _ in p_laser -> p[2] ],
          Camera = camera
      )
      surface sphere_r_list
      surface sphere_l_list
      Chart.Point3D(p_true_r)
      Chart.Point3D(p_true_l) ]
    |> Chart.combine
    // |> Chart.withSceneStyle(AspectMode = AspectMode.Data)
    |> Chart.withSize (1000, 1000)
    |> Chart.show
    
    0
    