namespace MultipassCell.Core.McManus.Vectors

open FSharp.Stats

type Vectors =
    static member X x y dx dy dz = vector [| x; dx / dz; y; dy / dz |]

    static member D d =
        matrix [| [| 1.; d; 0.; 0. |]
                  [| 0.; 1.; 0.; 0. |]
                  [| 0.; 0.; 1.; d |]
                  [| 0.; 0.; 0.; 1. |] |]

    static member R gx gy =
        matrix [| [| 1.; 0.; 0.; 0. |]
                  [| -gx; 1.; 0.; 0. |]
                  [| 0.; 0.; 1.; 0. |]
                  [| 0.; 0.; -gy; 1. |] |]

    static member t t =
        let c = cos t
        let s = sin t

        matrix [| [| c; 0.; s; 0. |]
                  [| 0.; c; 0.; s |]
                  [| -s; 0.; c; 0. |]
                  [| 0.; -s; 0.; c |] |]
