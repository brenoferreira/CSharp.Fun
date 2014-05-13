// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake

let buildDir = "./build/"
let testDir  = "./test/"

RestorePackages()

Target "Clean" (fun _ -> CleanDir buildDir)

Target "BuildApp" (fun _ ->
    !! "./CSharp.Fun/CSharp.Fun.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "Build-Output: ")

Target "BuildTest" (fun _ ->
    !! "./CSharp.Fun.Testes/CSharp.Fun.Testes.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: ")

Target "Test" (fun _ ->
    !! (testDir + "/CSharp.Fun.Testes.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "TestResults.xml" }))

// Default target
Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies
"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "Test"
  ==> "Default"

// start build
RunTargetOrDefault "Default"