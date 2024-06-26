// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace FSharp.Compiler.Service.Tests

open Xunit
open FSharp.Test
open FSharp.Test.Compiler

module AssemblySigningAttributes =

    [<FactForDESKTOP>]
    let ``--keycontainer:name DESKTOP`` () =
        FSharp """
module SignMe

open System
open System.Reflection

            """
        |> withOptions ["--keycontainer:myContainer"]
        |> compile
        |> shouldFail
        |> withWarningCode 0075
        |> withDiagnosticMessageMatches "The command-line option '--keycontainer' has been deprecated. Use '--keyfile' instead."
        |> ignore

    [<FactForNETCOREAPP>]
    let ``--keycontainer:name NETCOREAPP`` () =
        FSharp """
module SignMe

open System
open System.Reflection

            """
        |> withOptions ["--keycontainer:myContainer"]
        |> compile
        |> shouldFail
        |> withErrorCode 3393
        |> withDiagnosticMessageMatches "Key container signing is not supported on this platform."
        |> ignore

    //Expects: warning FS3392: The 'AssemblyKeyNameAttribute' has been deprecated. Use 'AssemblyKeyFileAttribute' instead.
    [<FactForDESKTOP>]
    let ``AssemblyKeyNameAttribute DESKTOP`` () =
        FSharp """
module SignMe

open System
open System.Reflection

[<assembly:AssemblyKeyNameAttribute("myContainer")>]
do ()
            """
             |> asFs
             |> compile
             |> shouldFail
             |> withWarningCode 3392
             |> withDiagnosticMessageMatches "The 'AssemblyKeyNameAttribute' has been deprecated. Use 'AssemblyKeyFileAttribute' instead."
             |> ignore

    //Expects: warning FS3392: The 'AssemblyKeyNameAttribute' has been deprecated. Use 'AssemblyKeyFileAttribute' instead.
    [<FactForNETCOREAPP>]
    let ``AssemblyKeyNameAttribute NETCOREAPP`` () =
        FSharp """
module SignMe

open System
open System.Reflection

[<assembly:AssemblyKeyNameAttribute("myContainer")>]
do ()
            """
             |> asFs
             |> compile
             |> shouldFail
             |> withErrorCode 2014
             |> withDiagnosticMessageMatches "A call to StrongNameGetPublicKey failed"
             |> ignore
