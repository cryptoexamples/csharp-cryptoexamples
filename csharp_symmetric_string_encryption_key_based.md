---
title: C# Symmetric String Encryption with key generation
keywords: sample
summary: "Symmetric string encryption in C# with key generation"
permalink: csharp_symmetric_string_encryption_key_based.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "https://docs.microsoft.com/de-de/dotnet/api/system.security.cryptography.aes?view=netcore-2.0",
        description: "Aes Class"
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
tags: [C#, CSharp, dotnet core, .net core, AES]
---

## Use cases

- Random key generation
- Key based encryption of a string

## Used .Net version

- .Net Core 2.1

## Example Code for C# based asymmetric key storage

```csharp
{% include_relative csharp_cryptoexamples/src/ExampleStringEncryptionKeyBased.cs %}
```

{% include links.html %}
