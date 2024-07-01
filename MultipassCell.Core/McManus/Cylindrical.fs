module MultipassCell.Core.McManus.Cylindrical

open FSharp.Stats

type Cylindrical(D: Matrix<float>, Rf: Matrix<float>, tf: Matrix<float>, Rb: Matrix<float>, tb: Matrix<float>) =
    let tf_ = tf.Transpose
    let tb_ = tb.Transpose
    member _.D (X: Vector<float>) = D * X
    member _.Rf (X: Vector<float>) = Rf * X
    member _.tf (X: Vector<float>) = tf * X
    member _.Rb (X: Vector<float>) = Rb * X
    member _.tb (X: Vector<float>) = tb * X
    member _.Crt (X: Vector<float>) =
        tf_ * D * Rf * tf * tb_ * D * Rb * tb * X