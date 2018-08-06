---
title: C# Asymetric Key Storage
keywords: sample
summary: "Asymetric Key storage in C#"
permalink: csharp_asymetric_key_storage.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-store-asymmetric-keys-in-a-key-container",
        description: "How to: Store Asymmetric Keys in a Key Container"
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
last_updated: "2018-08-06"
tags: [C#, CSharp, dotnet core, .net core, Asymmetric, Key, Storage]
---

## Use cases

- Store credentials of the cryptographic service provider

## Used .Net version

- .Net Core 2.1

## Example Code for C# based asymmetric key storage

```csharp
{% include_relative csharp_cryptoexamples/src/ExmapleKeyStorageProvider.cs %}
```

{% include links.html %}
