---
title: C# Asymmetric String Encryption
keywords: sample
summary: "Asymmetric String Encryption in C#"
permalink: csharp_asymmetric_string_encryption.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "http://csharp-tricks.blogspot.com/2015/03/rsa-verschlusselung.html",
        description: "C# Tipps und Tricks: RSA Verschlüsselung"
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
tags: [C#, CSharp, dotnet core, .net core, RSA, Asymmetric, String, Encryption]
---

## Use cases

- All can encrypt a message using the public key, but only the recipient can decrypt it using the private key
- Encrypt a string using the public key and decrypting it using the private key

## Used .Net version

- .Net Core 2.1

## Example Code for C# based asymmetric key storage

```csharp
{% include_relative csharp_cryptoexamples/src/ExampleAsymmetricStringEncryption.cs %}
```

{% include links.html %}
