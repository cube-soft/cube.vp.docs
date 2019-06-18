Cube.Pdf.Ghostscript
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## 概要

[Cube.Pdf.Ghostscipt](https://www.nuget.org/packages/Cube.Pdf.Ghostscript) は
[Ghostscript](https://www.ghostscript.com/) を .NET Framework 上で利用するための
ラッパーライブラリであり、.NET Framework 3.5 以降で利用可能な NuGet パッケージとして
公開されています。利用したいプロジェクトで下記の PackageReference を記述するか、
または Visual Studio の「参照の追加」機能を用いて追加して下さい。

```
<PackageReference Include="Cube.Pdf.Ghostscript" Version="2.16.0" />
```

Cube.* プロジェクトでは Ghostscript のライブラリを非公式な NuGet パッケージとして
[Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) を
公開しています。この NuGet パッケージは Cube.Pdf.Ghostscript の依存ライブラリとして
登録されているため自動的に開発端末にダウンロードされますが、実行プログラムの存在する
ディレクトリへのコピーは手動で行う必要があります。
実行前に、packages/cube.native.ghostscript から必要なプラットフォーム (x86/x64) の
gsdll32.dll をコピーして下さい。

### サンプルプログラム

簡単なサンプルとして、PostScript ファイルを PDF に変換するコードを記載します。
PDF に変換する場合は PdfConverter クラスを利用します。ページサイズや解像度、
埋め込み画像の圧縮方式などを設定するプロパティが用意されているので、必要な場合は
値を設定し、Invoke メソッドを実行する事で変換が完了します。

```cs
// using Cube.Pdf;
// using Cube.Pdf.Ghostscript;

var converter = new PdfConverter
{
    Paper        = Paper.Auto,
    Orientation  = Orientation.Auto,
    ColorMode    = ColorMode.Rgb,
    Resolution   = 600,
    Compression  = Encoding.Jpeg,
    Downsampling = Downsampling.None,
    Version      = new PdfVersion(1, 7),
};

converter.Invoke(@"path\to\src.ps", @"path\to\dest.pdf");
```

## 詳細

Ghostscript API では gsapi_init_with_args() と言う関数で変換処理を実行しますが、
この関数はコマンドライン版とまったく同じ引数を指定する事ができます。
Ghostscript のコマンドラインは、下記のように 3 種類に大別されます。

```
gs OPTIONS -c PS_CODE -f INPUT_FILES
```

Cube.Pdf.Ghostscript の基底クラスである [Converter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/Converter.cs) は、
上記のコマンドラインを生成して変換を実行するための非常に薄いラッパーとなります。

```cs
// using System.Collections.Generics;
// using Cube.Pdf.Ghostscript;

public class Converter
{
    public Converter(Format format);
    public ICollection<Argument> Options;
    public ICollection<Code> Codes;
    public void Invoke(string src, string dest);
}
```

使い方としては、まずコンストラクタに変換対象となる
[Format](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/Parameters/Format.cs) を
指定します。次に、Options プロパティには通常オプション、Codes プロパティには
PostScript コードをそれぞれ必要な数だけ指定します。そして、最後に Invoke メソッドに
変換元ファイル (PS, EPS, PDF) および保存先のパスを指定して変換を実行します。

通常、基底クラスである Converter をそのまま使用するよりも、各種継承クラスを必要に
応じて使用する方が便利です。Cube.Pdf.Ghostscript では現在、以下の変換クラスを
提供しています。これらのクラスは、ページサイズや画像の圧縮形式などを指定するための
プロパティを用意しており、設定値に応じた Ghostscript 引数を自動的に追加します。

* [DocumentConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/DocumentConverter.cs) ... PS/EPS/PDF
    - [PdfConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/PdfConverter.cs)
* [ImageConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/ImageConverter.cs) ... PNG/JPEG/BMP/TIFF
    - [JpegConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/JpegConverter.cs)

対応するプロパティが存在せず、自ら Ghostscript の引数を追加する必要がある場合、
Options 引数に Argument オブジェクトを追加します。
[Argument](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/Argument.cs)
クラスのコンストラクタは下記のようになっています。

```cs
// using Cube.Pdf.Ghostscript;

public class Argument
{
    public Argument(string name, bool value);
    public Argument(string name, int value);
    public Argument(string name, string value);
    public Argument(char type);
    public Argument(char type, int value);
    public Argument(char type, string name);
    public Argument(char type, string name, bool value);
    public Argument(char type, string name, int value);
    public Argument(char type, string name, string value);
    public Argument(char type, string name, string value, bool literal);
}
```

Ghostscript の引数一覧は [How to Use Ghostscript](https://www.ghostscript.com/doc/current/Use.htm) および、そのリンク先で確認する事ができます。

```
-dAutoRotatePages=/PageByPage
```

例えば、上記のような Ghostscript 引数を生成する場合、以下のようにコンストラクタの
各引数を指定して Argument オブジェクトを生成します。

```
new Argument('d', "AutoRotatePages", "PageByPage", true);
```

最後の引数は、値に "/" （スラッシュ）が必要な場合は true、それ以外は false を指定
します。尚、ほとんどの場合、上記以外のコンストラクタを利用する事で、自ら "/" の
有無を判定しなくても良いように設計しています。

尚、Ghostscript は作業フォルダとして TEMP 環境変数の値を利用しますが、Windows の
ユーザ名に日本語が混在していると問題になる事があります。この問題を回避するために、
Converter クラスには Temp と言うプロパティを設定しています。Converter クラスは、
このプロパティに値が設定されている場合、Invoke メソッドの実行中のみ TEMP 環境変数を
設定された値に変更します。