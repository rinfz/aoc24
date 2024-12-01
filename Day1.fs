module Day1

open System.IO
open System

let part1 (left: int[]) (right: int[]) =
    left |> Array.zip right |> Array.map (fun (a, b) -> a - b |> abs) |> Array.sum

let part2 (left: int[]) (right: int[]) =
    let counts = right |> Array.countBy id |> Map.ofArray
    left |> Array.sumBy (fun x -> x * (Map.tryFind x counts |> Option.defaultValue 0))

let solve() =
    let input =
        File.ReadAllLines("inputs/1")
        |> Array.map (fun line -> line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        |> Array.map (fun parts -> (int parts[0], int parts[1]))
    let left = input |> Array.map fst |> Array.sort
    let right = input |> Array.map snd |> Array.sort
    printfn "part1: %d" (part1 left right)
    printfn "part2: %A" (part2 left right)
    ()
