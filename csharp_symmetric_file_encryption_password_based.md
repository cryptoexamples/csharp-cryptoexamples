---
title: C# Symmetric Password Based File Encryption
keywords: sample
summary: "Password based symmetric file encryption in C#"
permalink: csharp_symmetric_file_encryption_password_based.html
folder: C#
references: [
    # Place a list of references used to create and/or understand this example.
    {
        url: "https://ourcodeworld.com/articles/read/471/how-to-encrypt-and-decrypt-files-using-the-aes-encryption-algorithm-in-c-sharp",
        description: "How to encrypt and decrypt files using the AES encryption algorithm in C#"
    },
    {
        url: "https://cryptography.io/en/latest/hazmat/primitives/key-derivation-functions/#cryptography.hazmat.primitives.kdf.pbkdf2.PBKDF2HMAC",
        description: "Cryptography Password Based Key Derivation Function 2 Documentation"
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
tags: [C#, CSharp, dotnet core, .net core, AES, Rfc2898DeriveBytes, PKCS7, Salt, Cipher Block Chaining]
---

## Use cases

- Password based encryption of a file
- Previously shared common secret (password)

## Used .Net version

- .Net Core 2.1

## Example Code for C# based asymmetric key storage

```csharp
{% include_relative csharp_cryptoexamples/src/ExampleFileEncryption.cs %}
```

{% include links.html %}