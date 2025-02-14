module MultipassCell.Core.Calc


let calc sphere_r sphere_l p0 p1 refCount=
    let r0 = p1 - p0

    let rec proceed =
        function
        | _, 0, _ -> []
        | pr, count, true ->
            pr
            :: proceed (reflect sphere_r pr, count - 1, false)
        | pr, count, false ->
            pr
            :: proceed (reflect sphere_l pr, count - 1, true)

    proceed ((p0, r0), refCount, true)
    
/// Calculate diff between two list of points
let inline error(truth: ('a * 'a * 'a) list) (comp: ('a * 'a * 'a) list) : 'a option =
    if (truth.Length <> comp.Length) then
        printfn "truth and comp are must be same length."
        printfn $"but truth: %d{truth.Length}, comp: %d{comp.Length}"
        None
    else
        List.zip truth comp
        |> List.map(fun ((tx, ty, _), (cx, cy, _)) -> pown(tx - cx) 2 + pown(ty - cy) 2 |> sqrt)
        |> List.sum
        |> Some
