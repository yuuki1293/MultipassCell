namespace MultipassCell.Core.Common

open MultipassCell.Core
open FSharp.Stats
open FSharp.Stats.Vector.Generic

type Sphere(xr: float, yr: float, zr: float, r: float) =
    interface Mirror<float> with
        member _.reflect o r0 =
            let x0 = o.Values[0]
            let y0 = o.Values[1]
            let z0 = o.Values[2]
            let dx0 = r0.Values[0]
            let dy0 = r0.Values[1]
            let dz0 = r0.Values[2]
            let a = (pown dx0 2) + (pown dy0 2) + (pown dz0 2)
            let b = -2. * (
                + dx0 * x0 - dx0 * xr
                + dy0 * y0 - dy0 * yr
                + dz0 * z0 - dz0 * zr)
            let c =
                (pown x0 2) + (pown y0 2) + (pown z0 2)
                - 2. * (x0 * xr + y0 * yr + z0 * zr) 
                + (pown xr 2) + (pown yr 2) + (pown zr 2)
                - pown r 2
            let t = (- b + sqrt ((pown b 2) - 4. * a * c)) / (2. * a)
            let t_ = (- b - sqrt ((pown b 2) - 4. * a * c)) / (2. * a)
            let x1 = x0 - dx0 * t
            let y1 = y0 - dy0 * t
            let z1 = z0 - dz0 * t
            let xv = x1 - xr
            let yv = y1 - yr
            let zv = z1 - zr
            let n = vector [xv; yv; zv]
            let r1 = r0 - 2. * n * (dot n r0)
            r1