Cube.Pdf.Converter
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## 概要

[Cube.Pdf.Converter](https://www.nuget.org/packages/Cube.Pdf.Converter/) は
[CubePDF](https://www.cube-soft.jp/cubepdf/) で使用される PostScript から PDF 等への
変換処理を提供するライブラリで、.NET Framework 3.5 以降で利用可能な NuGet
パッケージとして公開されています。利用したいプロジェクトで下記の PackageReference を
記述するか、または Visual Studio の「参照の追加」機能を用いて追加して下さい。

```
<PackageReference Include="Cube.Pdf.Converter" Version="1.0.0.20" />
```

Cube.Pdf.Converter は [Ghostscript](https://www.ghostscript.com/) に依存しています。
また、Cube.* プロジェクトでは Ghostscript のライブラリを非公式な NuGet パッケージとして
[Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) を
公開しています。この NuGet パッケージは Cube.Pdf.Converter の依存ライブラリとして
登録されているため自動的に開発端末にダウンロードされますが、実行プログラムの存在する
ディレクトリへのコピーは手動で行う必要があります。
実行前に、packages/cube.native.ghostscript から必要なプラットフォーム (x86/x64) の
gsdll32.dll をコピーして下さい。

Cube.Pdf.Converter は、CubePDF の変換処理をエミュレートするなど、限定された用途の
ために公開されているライブラリです。PDF の変換や編集に関する汎用的な操作など
Cube.Pdf.Converter の動作に合致しないような処理を行いたい場合、下記のライブラリ群を
利用する事もご検討下さい。

* [Cube.Pdf](https://www.nuget.org/packages/Cube.Pdf/)
* [Cube.Pdf.Ghostscript](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
* [Cube.Pdf.Itext](https://www.nuget.org/packages/Cube.Pdf.Itext/)
* [Cube.Pdf.Pdfium](https://www.nuget.org/packages/Cube.Pdf.Pdfium/)

## サンプルプログラム

Cube.Pdf.Converter の最も簡単なサンプルプログラムは下記の通りです。

```cs
// using Cube.Pdf.Converter;

static void Main(string[] args)
{
    var settings = new SettingFolder();
    settings.Load();    // レジストリの設定をロード
    settings.Set(args); // CubeVP 仮想プリンタ経由の場合

    using (var facade = new Facade(settings)) facade.Invoke();
}
```

Cube.Pdf.Converter は、主に以下の 2 種類のクラスを用いて変換処理を実行します。

* [Facade](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Core/Sources/Facade.cs)
* [SettingFolder](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Core/Sources/SettingFolder.cs)

SettingFolder はユーザ設定の読み込み・解析を担い、Facade クラスは SettingFolder の
内容に応じて、実際に CubePDF における変換処理（メイン処理）を実行します。

## API Reference

### Facade

Facade クラスのコンストラクタ、プロパティ、メソッドは下記の通りです。

```cs
// using System.Collections.Generics;
// using System.Reflection;

public sealed class Facade
{
    public Facade(Assembly assembly);
    public Facade(SettingFolder settings);

    public SettingFolder Settings { get; }
    public IEnumerable<string> Results { get; }

    public void Invoke();
}
```

コンストラクタには、Assembly オブジェクトまたは SettingFolder オブジェクトを指定
します。Assembly オブジェクトを指定した場合、Facade クラスは指定された内容を用いて
初期状態の SettingFolder オブジェクトを生成し、その設定内容を利用します。
設定内容は、Settings プロパティを通じて後から変更する事も可能です。

Invoke メソッドは、現在の設定内容にしたがって CubePDF の変換処理を実行します。
実行後、最終的に保存されたファイルのパス一覧が Results に格納されます。
変換形式として PNG や JPEG などの複数ページを保持できないものを指定した場合、
Settings で指定した保存先パスと実際に生成されるファイルのパスが異なる事があるため、
その確認に用いられます。

### SettingFolder

SettingFolder クラスのコンストラクタ、プロパティ、メソッドは下記の通りです。

```cs
// using System;
// using System.Reflection;
// using Cube.DataContract;

public sealed class SettingFolder
{
    public SettingFolder(Assembly assembly);
    public SettingFolder(Assembly assembly, Format format, string path);

    public SettingValue Value { get; }
    public DocumentName DocumentName { get; }
    public string Digest { get; }

    public void Load();
    public void Save();
    public void Set(string[] args);
}
```

コンストラクタには、Assembly オブジェクトおよびいくつかの付加的な情報を指定します。
引数 *format* には Cube.DataContract.Registry または Cube.DataContract.Json の
どちらかを指定し（Cube.DataContract.Xml は非推奨）、それぞれレジストリまたは JSON
ファイルから設定内容の読み込みます。引数 *path* には、設定内容が保存されている
場所を指定します。ただし、*format* に Cube.DataContract.Registry が指定された場合、
設定内容は HKEY_CURRENT_USER\Software\path に存在するものと見なされます。

設定内容は、Load メソッドを実行した時に読み込まれ、読み込み結果は Value プロパティ
に格納されます。

DocumentName プロパティは、仮想プリンタ経由で指定されたドキュメント名を表し、
Digest プロパティは入力ファイルのハッシュ値を保持します。いずれも、読み取り専用
プロパティであり、値の設定には Set メソッドを利用します。