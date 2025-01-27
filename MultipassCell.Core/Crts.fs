module MultipassCell.Core.Crts

open FSharp.Stats

type Crt =
    | Generic of mr: Mirror * ml: Mirror * pr: (Vector<float> * Vector<float>)
    | McManus of D: Matrix<float> * R: Matrix<float> * t: Matrix<float> * X: Vector<float>

let crt =
    function
    | Generic (mr, ml, pr0) ->
        let pr1 = reflect mr pr0
        let pr2 = reflect ml pr1

        [ Generic(mr, ml, pr1)
          Generic(mr, ml, pr2) ]

    | McManus (D, R, t, X0) ->
        let t_1 = t.Transpose
        let X1 = t_1 * D * R * t * X0
        let X2 = t_1 * D * R * t * X1

        [ McManus(D, R, t, X1)
          McManus(D, R, t, X2) ]
