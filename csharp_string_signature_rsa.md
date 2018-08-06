---
title: C# String Signing
keywords: sample
summary: "String signing in C#"
permalink: csharp_string_signature_rsa.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "https://msdn.microsoft.com/en-us/library/9tsc5d0z(v=vs.110).aspx",
        description: "RSACryptoServiceProvider.SignData Method (Byte[], Object)"
    }
]
authors: [
    {        
        name: "Nico Rusam",
        url: "https://github.com/romanicus"
    }
]
# List all reviewers that reviewed this version of the example. When the example is updated all old reviews
# must be removed from the list below and the code has to be reviewed again. The complete review process
# is documented in the main repository of CryptoExamples
current_reviews: [

]
# Indicates when this example was last updated/created. Reviews don't change this.
last_updated: "2018-07-24"
tags: [C#, CSharp, dotnet core, .net core, RSA, Asymmetric, String, hash, SHA, SHA1]
---

## Use cases

- Verifying if a string has been changed

## Used .Net version

- .Net Core 2.1

## Example Code for C# based asymmetric key storage

```csharp
{% include_relative csharp_cryptoexamples/src/ExampleSignature.cs %}
```

{% include links.html %}
