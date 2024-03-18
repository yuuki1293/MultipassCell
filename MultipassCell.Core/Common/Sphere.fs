namespace MultipassCell.Core.Common

open MultipassCell.Core
open FSharp.Stats
open FSharp.Stats.Vector.Generic

type Sphere(xr: float, yr: float, zr: float, r: float) =
    let pow2 x = pown x 2
    let size v = map (fun x -> x * x) v |> sum |> sqrt
    let unitV v = let s = size v in map (fun x -> x / s) v
    
    interface Mirror<float> with
        member this.reflect p0 r0 =
            let x0 = p0[0]
            let y0 = p0[1]
            let z0 = p0[2]
            let dx0 = r0[0]
            let dy0 = r0[1]
            let dz0 = r0[2]
            let a = pow2 dx0 + pow2 dy0 + pow2 dz0
            let b = -2. * (
                dx0 * (xr - x0)
                + dy0 * (yr - y0)
                + dz0 * (zr - z0))
            let c =
                pow2 (xr - x0) + pow2 (yr - y0) + pow2 (zr - z0)
                - pow2 r
            let t = (- b + sqrt ((pow2 b) - 4. * a * c)) / (2. * a)
            let t_ = (- b - sqrt ((pow2 b) - 4. * a * c)) / (2. * a)
            let x1 = x0 + dx0 * t
            let y1 = y0 + dy0 * t
            let z1 = z0 + dz0 * t
            let xv = xr - x1
            let yv = yr - y1
            let zv = zr - z1
            let n = (this :> Mirror<float>).normal (vector [xv; yv; zv])
            let r1 = r0 - 2. * n * (dot n r0)
            r1

        member _.normal v = unitV v