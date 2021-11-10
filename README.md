# 🔍 Trojan Source detector for .NET

Simple CLI tool that allows you to analyze your .NET projects and detect vulnerabilities related to hidden characters in your source.

## Problem

According to a recent research by [Cambridge University's Nicholas Boucher and Ross Anderson](https://arxiv.org/abs/2111.00169), there are **two vulnerabilities that impact most code compilers.**

These sorts of vulnerabilities have an **impact on software supply chains**; for example, if an attacker successfully commits code injection by deceiving human reviewers, future software is likely to inherit the vulnerability.

## Problem in depth

- **Extended strings:** make sections of string literals seem as code, having the same impact as comments and causing string comparison to fail.

- **Comment out:** forces a comment to appear as code, which is then ignored.

- **Early returns:** bypass a function by running a return statement that seems to be inside a comment.

The compilers support this unique code that you do not see, when compiling your application they interpret it **creating a compiled application different from the one you see in your IDE.**

### Read about the complete problem and how it works at:

[📕 Dotnetsafer Trojan source article](https://medium.com/dotnetsafer/trojan-source-in-dotnet-25f6ce190c1a)

## Solution:

Scan your project files to find hidden characters that your IDE does not interpret but the compiler does process.

## Instalation

On CMD or PowerShell:

```cmd
dotnet tool install --global TrojanSourceDetector --version 1.0.1
```

### Usage

After install this dotnet tool, run in your cmd:

```cmd
TrojanSourceDetector
```

and put your project/s full directory to scan.

#### Optional Commands

| Flag | Purpose |
|------|---------|
| -Verbose (-v) | Output the lines with problems both as they appear and with the unicode character tag displayed. |
| -ESC (-e) | Exclude escape character (\u7F) |
| -BOM (-b) | Exclude Unicode Byte-order Marks (\uFEFF) |

If the first parameter is a valid folder, it will be used instead of prompting the user for a folder to scan.

## Output / Demo

![result](https://cdn-images-1.medium.com/max/1200/1*MeZx1jiuqHBAVs3_vXaP8w.png)

