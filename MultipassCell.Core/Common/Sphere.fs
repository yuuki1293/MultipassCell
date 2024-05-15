#nowarn "0025"

namespace MultipassCell.Core.Common

open MultipassCell.Core
open FSharp.Stats
open FSharp.Stats.Vector.Generic

type Sphere(xr: float, yr: float, zr: float, r: float, sign_: int) =
    let pow2 x = pown x 2
    let size v = map (fun x -> x * x) v |> sum |> sqrt
    let unitV v = let s = size v in map (fun x -> x / s) v
    let choice z0 dz0 t t_ =
        if isNan t then nan
        else
            let z = z0 + dz0 * t
            if sign z = sign_ then t else t_
            
    new (args: float list) =
        match args with
        | xr::yr::zr::r::sign_::_ -> Sphere(float xr, float yr, float zr, float r, int sign_)
        | _ -> failwith "too few arguments"; Sphere(0., 0., 0., 0., 0) 
    
    interface Mirror<float> with
        member this.reflect p0 r0 =
            let [|x0; y0; z0|] = p0.ToArray()
            let [|dx0; dy0; dz0|] = r0.ToArray()
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
            let t = choice z0 dz0 t t_
            let p1 = p0 + r0 * t
            let pv = vector [xr; yr; zr] - p1
            let n = (this :> Mirror<float>).normal pv
            let r1 = r0 - 2. * n * (dot n r0)
            (p1, r1)

        member _.normal v = unitV v
        
        member _.solve v =
            let x, y = v[0], v[1]
            let z = float sign_ * sqrt(pow2 r - pow2(x - xr) - pow2(y - yr)) + zr
            vector [|x; y; z|]
        
    interface Surface<float> with
        member this.surface resolution corner1 corner2 =
            let [|stepX; stepY|] =
                (sub corner2 corner1)
                |> map (fun x -> x / (float resolution))
                |> toArray
            
            let xs = seq [corner1[0] .. stepX .. corner2[0]]
            let ys = seq [corner1[1] .. stepY .. corner2[1]]
            
            let zs =
                seq {
                    for x in xs do
                        yield seq {
                            for y in ys do
                                yield float sign_ * sqrt(pow2 r - pow2(x - xr) - pow2(y - yr)) + zr
                        }
                }
            
            (xs, ys, zs)