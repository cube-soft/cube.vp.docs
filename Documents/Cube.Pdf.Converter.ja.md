Cube.Pdf.Converter
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## 概要

[Cube.Pdf.Converter](https://www.nuget.org/packages/Cube.Pdf.Converter/) は
[CubePDF](https://www.cube-soft.jp/cubepdf/) で使用される PostScript から PDF 等への
変換処理を提供するライブラリで、.NET Framework 3.5 以降、および .NET Standard 2.0 以降で
利用可能な NuGet パッケージとして公開されています。利用したいプロジェクトで下記の
PackageReference を記述するか、または Visual Studio の「参照の追加」機能を用いて追加して下さい。

```
<PackageReference Include="Cube.Pdf.Converter" Version="4.0.0" />
```

Cube.Pdf.Converter は [Ghostscript](https://www.ghostscript.com/) に依存しています。
また、Cube プロジェクトでは
[Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript)
を非公式な NuGet パッケージとして公開しています。この NuGet パッケージは
Cube.Pdf.Converter の依存ライブラリなので自動的に端末の packages フォルダに展開
されますが、場合によっては、実行フォルダへのコピーは手動で行う必要があります。
Native ライブラリのコピーについては、
[Native ライブラリのコピーについて](https://docs.cube-soft.jp/entry/cubevp/how-to-copy)
を参照下さい。

Cube.Pdf.Converter は、CubePDF の変換処理をエミュレートするなど、限定された用途の
ために公開されているライブラリです。PDF の変換や編集に関する汎用的な操作など
Cube.Pdf.Converter の動作に合致しないような処理を行いたい場合、下記のライブラリ群を
利用する事もご検討下さい。

* [Cube.Pdf](https://www.nuget.org/packages/Cube.Pdf/)
* [Cube.Pdf.Ghostscript](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
* [Cube.Pdf.Itext](https://www.nuget.org/packages/Cube.Pdf.Itext/)
* [Cube.Pdf.Pdfium](https://www.nuget.org/packages/Cube.Pdf.Pdfium/)

### サンプルプログラム

Cube.Pdf.Converter の最も簡単なサンプルプログラムは下記の通りです。

```cs
// using Cube.Pdf.Converter;

static void Main(string[] args)
{
    var settings = new SettingFolder();
    settings.Load();    // レジストリの設定をロード
    settings.Set(args); // 仮想プリンターからの引数を解析

    using (var facade = new Facade(settings)) facade.Invoke();
}
```

Cube.Pdf.Converter は、主に以下の 2 種類のクラスを用いて変換処理を実行します。

* [Facade](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Core/Sources/Facade.cs)
* [SettingFolder](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Core/Sources/SettingFolder.cs)

SettingFolder はユーザー設定の読み込み・解析を担い、Facade クラスは SettingFolder の
内容に応じて、実際に CubePDF における変換処理（メイン処理）を実行します。
尚、このサンプルプログラムと同等の処理を実行するプロジェクトを
[CubePdfLite](https://github.com/cube-soft/Cube.Vp.Docs/tree/master/Examples/CubePdfLite) にて公開しています。

## 詳細

### Facade

Facade クラスのコンストラクタ、プロパティ、メソッドは下記の通りです。

```cs
// using System.Collections.Generic;
// using System.Reflection;
// using Cube.Pdf.Converter;

public sealed class Facade
{
    public Facade();
    public Facade(Assembly assembly);
    public Facade(SettingFolder settings);

    public SettingFolder Settings { get; }
    public IEnumerable<string> Results { get; }
    public bool Busy { get; }

    public void Invoke();
}
```

コンストラクタは、引数無しで実行可能な他、Assembly オブジェクトまたは SettingFolder
オブジェクトを指定する事もできます。Assembly オブジェクトを指定した場合、Facade クラスは
指定された内容を用いて初期状態の SettingFolder オブジェクトを生成し、その設定内容を利用します。
設定内容は、Settings プロパティを通じて後から変更する事も可能です。

Invoke メソッドは、現在の設定内容にしたがって CubePDF の変換処理を実行します。
実行後、最終的に保存されたファイルのパス一覧が Results に格納されます。
これは、変換形式として PNG や JPEG などの複数ページを保持できないものを指定した場合、
Settings で指定した保存先パスと実際に生成されるファイルのパスが異なる事があるため、
その確認に用いられます。

### SettingFolder

SettingFolder クラスのコンストラクタ、プロパティ、メソッドは下記の通りです。

```cs
// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using Cube.FileSystem.DataContract;
// using Cube.Pdf.Converter;

public sealed class SettingFolder
{
    public SettingFolder();
    public SettingFolder(Assembly assembly);
    public SettingFolder(Format format, string location);
    public SettingFolder(Assembly assembly, Format format, string location);

    public SettingValue Value { get; }
    public DocumentName DocumentName { get; }
    public string Digest { get; }

    public void Load();
    public void Save();
    public void Set(IEnumerable<string> args);
}
```

コンストラクタには Assembly オブジェクトおよびいくつかの付加的な情報を指定します。

引数 *format* には Cube.DataContract.Format.Registry または Cube.DataContract.Format.Json
のどちらかを指定し（Cube.DataContract.Format.Xml は非推奨）、それぞれレジストリまたは
JSON ファイルから設定内容を読み込みます。JSON ファイルの例は、
[Cube.Pdf.Converter.json](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Assets/Cube.Pdf.Converter.json)
を参照下さい。

引数 *location* には、設定内容が保存されている場所を指定します。ただし、引数 *format* に
Cube.FileSystem.DataContract.Format.Registry が指定された場合、設定内容は
HKEY_CURRENT_USER\Software\location に存在するものと見なされます。

設定内容は Load メソッドを実行した時に読み込まれ、読み込み結果は Value プロパティに格納されます。

DocumentName プロパティは、仮想プリンター経由で指定されたドキュメント名を表し、
Digest プロパティは入力ファイルのハッシュ値を保持します。
いずれも、読み取り専用プロパティであり、値の設定には Set メソッドを利用します。

Value プロパティは、レジストリまたは JSON ファイルから読み込んだ内容を保持します。
各プロパティの内容は下記の通りです。尚、プロパティ名とレジストリ等での名前が異なる場合、
後者を括弧内に記載します。また、CubePDF メイン画面の表示設定に関するものは、ここでは省略します。

* **Destination**  
  変換したファイルを保存するパスを表します。レジストリ等から読み込んだ場合は、
  最後に Save メソッドが実行された時の値を保持します。
* **Format**  
  変換後のファイル形式を表します。
  指定可能な値として、Pdf, Ps, Eps, Jpeg, Png, Bmp, Tiff などがあります。
  詳細は [Format](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Ghostscript/Sources/Parameters/Format.cs) を参照下さい。
* **SaveOption**  
  保存パスとして指定したファイルが存在する場合の挙動を表します。
  設定可能な値は、Overwrite, MergeHead, MergeTail, Rename の 4 種類です。
* **Orientation**  
  各ページの向きの設定方法を表します。
  設定可能な値は、Auto, Portrait, UpsideDown, Landscape, Seascape の 5 種類です。
  詳細は [Orientation](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Ghostscript/Sources/Parameters/Orientation.cs) を参照下さい。
* **Resolution**  
  画像データの解像度を整数値で表します。
  この値は、画像データのダウンサンプリング時に利用されます。
* **ColorMode**  
  画像データの色の変換方法を表します。
  設定可能な値は Rgb, Cmyk, Grayscale, Monochrome, SameAsSource の 5 種類です。
  詳細は [ColorMode](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Ghostscript/Sources/Parameters/ColorMode.cs) を参照下さい。
  尚、指定されたファイルの種類によっては、Monochrome を指定した場合も Grayscale として処理される場合があります。
* **Encoding**  
  PDF に埋め込まれた画像の圧縮形式を表します。
  [Encoding](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Ghostscript/Sources/Parameters/Encoding.cs) と言う enum で定義されており、
  PDF で利用可能な値は、None, Flate, Lzw, Jpeg の 4 種類です。
* **Downsampling**  
  画像データのダウンサンプリング方法を表します。
  設定可能な値は、None, Average, Bicubic, Subsample の 4 種類です。
* **Linearization**  
  PDF データを先頭から読み込み可能にするかどうかを真偽値で表します。
  このオプションは、Adobe Acrobat Reader 等では「Web 表示用に最適化」と呼ばれています。
* **EmbedFonts**  
  変換後のファイルにフォント情報を埋め込むかどうかを真偽値で表します。
  **現在、EmbedFonts を false に設定すると表示端末に該当フォントがインストール
  されているかどうかに関わらず、文字化けが発生する問題が確認されています。**
  そのため、特別な理由がない限り true に設定して下さい。
* **PostProcess**  
  変換処理の終了後に実行される操作を表します。
  設定可能な値は、None, Open, OpenDirectory, Others の 4 種類です。
* **UserProgram**  
  変換処理の終了後に実行されるユーザープログラムのパスを表します。
  この設定は、PostProcess が Others 以外の時には無視されます。
* **Temp**  
  一時ファイルを保存するためのディレクトリを表します。
  尚、この値は Ghostscript の一時ディレクトリとしても利用されます。
  Ghostscript はパスにマルチバイト文字が含まれると処理に失敗する場合があります。
  そのため、設定値にはマルチバイト文字が含まれないようにして下さい。
* **Metadata**  
  PDF の文書プロパティを表します。
  詳細は、[Metadata](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Core/Sources/Metadata.cs) を参照下さい。

Value プロパティは、いくつかのレジストリ等には保存されないプロパティも保持します。
これらのプロパティは、直接、または Set メソッドを通じて設定されます。
レジストリ等には保存されないプロパティは下記の通りです。

* **Source**  
  変換元となるデータが保存されているパスを表します。
  通常、Set メソッドを通じて、仮想プリンターより指定されたパスが設定されます。
* **DeleteSource**  
  変換処理終了後に Source で指定されたファイルを削除するかどうかを真偽値で表します。
  通常、Set メソッドを通じて、仮想プリンターより指定された値が設定されます。
* **Encryption**  
  PDF のセキュリティ設定を表します。
  詳細は、[Encryption](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Core/Sources/Encryption.cs) を参照下さい。

#### 旧バージョンからの移行

CubePDF SDK 8.0.0 にて、設定内容に変更が行われており、特にレジストリ等に保存される際の名前が大きく変更されています。
具体的には、プログラム側のプロパティ名とレジストリ等に保存される名前が異なっていた設定項目について、プロパティ名の方に統一するように変更されました。
CubePDF の設定は、既定ではレジストリの ```KEY_CURRENT_USER\Software\CubePDF``` サブキー下に保存されます。
8.0.0 以前は v2 と言うサブキーを作成していましたが、8.0.0 以降は　v3 と言うサブキーに変更されています。

プログラム上で旧バージョンから移行する場合、Migrate 拡張メソッドの利用を検討して下さい。
例えば、CubePDF SDK 8.0.0 以前 (CubePDF 3.0.0 以前) から移行する場合、下記のようなプログラムとなります。

```cs
// using Cube.Pdf.Converter;
var settings = new SettingFolder();
settings.Migrate(@"CubeSoft\CubePDF\v2");
```

Migrate 拡張メソッドは、指定されたサブキーが存在する場合は該当の内容を使用して移行処理を実行します。
また、サブキーが存在しない場合は通常の Load メソッドが実行されますので、どちらの場合にも対応可能となっております。