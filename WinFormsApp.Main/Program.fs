module WinFormsApp.Main.Program

open System
open System.Windows.Forms
open Form1

[<STAThread; EntryPoint>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false
    new View() |> Application.Run
    0
