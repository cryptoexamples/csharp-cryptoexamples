---
title: C# String Hashing
keywords: sample
summary: "C# string hashing"
permalink: csharp_string_hash.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "https://docs.microsoft.com/de-de/dotnet/api/system.security.cryptography.hashalgorithm?view=netcore-2.1",
        description: "HashAlgorithm Class"
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
tags: [C#, CSharp, dotnet core, .net core, hash, SHA, SHA-256]
---

## Use cases

- Verifying if a string has been changed

## Used .Net version

- .Net Core 2.1

## Example Code for C# based hashing of a String using SHA-256 and UTF-8 encoding

```csharp
{% include_relative csharp_cryptoexamples/src/ExampleHashInOneMethod.cs %}
```



{% include links.html %}
