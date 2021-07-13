How to copy native libraries
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## 概要

[Cube.Pdf](https://github.com/cube-soft/Cube.Pdf) は現在、
[Ghostscript](https://www.ghostscript.com/) および
[PDFium](https://pdfium.googlesource.com/pdfium/) と言う Native なライブラリに依存しています。
また、これらのライブラリを簡単に利用するために、非公式な NuGet パッケージとして
下記のものを公開しています。

* [Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript)
* [Cube.Native.Pdfium](https://www.nuget.org/packages/Cube.Native.Pdfium)
* [Cube.Native.Pdfium.Lite](https://www.nuget.org/packages/Cube.Native.Pdfium.Lite)

これらのライブラリは、下記のようにプロジェクトファイルに対して PackageReference
を記述すると端末の packages フォルダーに展開されますが、場合によっては、
最終的な実行フォルダーには手動でコピーする必要があります。

```
<PackageReference Include="Cube.Native.Ghostscript" Version="9.54.0" />
```

例えば、PackageReference 形式で上記のように Cube.Native.Ghostscript パッケージを
参照した場合、Windows の初期設定では NuGet パッケージのフォルダー構成は下記のように
なります（*USER_NAME* はログオン中のユーザー名に置換して下さい）。

```
C:\Users\USER_NAME\.nuget\packages
    + cube.native.ghostscript
        + 9.54.0
            + contents
                + win_x86/native
                + win_x64/native
            + runtimes
                + win_x86/native
                + win_x64/native
```

上記の runtimes 下にあるフォルダーからプロジェクトの設定に応じた gsdll32.dll をコピーし、
実行フォルダーへペーストして下さい。Any CPU の場合、端末の Windows の種類 (32bit or 64bit)
および 32ビットの優先 (Prefer 32bit) オプションの有無で必要となる gsdll32.dll が異なるので
ご注意下さい。