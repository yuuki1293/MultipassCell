#nowarn "0025"

namespace MultipassCell.Core.Common

open MultipassCell.Core
open FSharp.Stats
open FSharp.Stats.Vector.Generic

type Sphere(pr: Vector<float>, r: float, sign_: int) =
    let pow2 x = pown x 2
    let size v = map (fun x -> x * x) v |> sum |> sqrt
    let unitV v = let s = size v in map (fun x -> x / s) v
    let choice z0 dz0 t t_ =
        if isNan t then nan
        else
            let z = z0 + dz0 * t
            if sign z = sign_ then t else t_
    let dot2 vec = dot vec vec
            
    new (args: float list) =
        match args with
        | xr::yr::zr::r::sign_::_ -> Sphere(vector [float xr; float yr; float zr], float r, int sign_)
        | _ -> failwith "too few arguments"; Sphere(Vector.zeroCreate 3, 0., 0)
    
    interface Mirror with
        member this.reflect p0 r0 =
            let a = dot2 r0
            let b = -2. * (dot r0 (pr - p0))
            let c = dot2 (pr - p0) - pow2 r
            let t = (- b + sqrt ((pow2 b) - 4. * a * c)) / (2. * a)
            let t_ = (- b - sqrt ((pow2 b) - 4. * a * c)) / (2. * a)
            let t = choice p0[2] r0[2] t t_
            let p1 = p0 + r0 * t
            let pv = pr - p1
            let n = (this :> Mirror).normal pv
            let r1 = r0 - 2. * n * (dot n r0)
            (p1, r1)

        member _.normal v = unitV v
        
        member _.solve v =
            let x, y = v[0], v[1]
            let z = float sign_ * sqrt(pow2 r - pow2(x - pr[0]) - pow2(y - pr[1])) + pr[2]
            vector [x; y; z]
        
    interface Surface with
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
                                yield float sign_ * sqrt(pow2 r - pow2(x - pr[0]) - pow2(y - pr[1])) + pr[2]
                        }
                }
            
            (xs, ys, zs)