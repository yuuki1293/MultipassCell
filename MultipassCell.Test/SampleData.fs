module MultipassCell.Test.SampleData

open System.IO
open FSharp.Data.JsonProvider

type Sample =
    JsonProvider<"./data/sample.json", EmbeddedResource="MultipassCell.Test, MultipassCell.Test.data.sample.json">
    
let sample =
    File.ReadAllText "./data/sample.json"
    |> Sample.Parse