namespace MultipassCell.Core

open FSharp.Stats

type Mirror<'T> =
    abstract reflect: Vector<'T> -> Vector<'T> -> Vector<'T>
    abstract normal: Vector<'T> -> Vector<'T>