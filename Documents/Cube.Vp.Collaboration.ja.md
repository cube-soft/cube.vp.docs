Collaboration for CubeVP
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

CubeVP では、仮想プリンターと言う性質上、印刷コマンドを発行するプログラム（**印刷前プログラム**）と印刷データ生成後に
CubeVP より実行されるプログラム（**印刷後プログラム**）の 2 種類が存在します。この時、印刷前プログラムの GUI で
設定した内容に応じて印刷後プログラムの変換処理を実行したい等、2 つのプログラムを連携させたいと言う要求が考えられます。
このドキュメントでは、印刷前後でのプログラム連携を実現させるための方法をサンプルプログラムとともに紹介します。

## プログラム連携方法の概要

CubeVP を通じて生成される仮想プリンターは Windows 標準の印刷機構を利用する都合、各種操作にも様々な制約が課されます。
例えば、印刷前プログラムの設定内容に応じてプログラム引数を調整し、印刷後プログラムを実行する事はできません。

印刷前プログラムが、ある印刷操作において、印刷後プログラムに対して自由な内容を伝えられる領域は DocumentName
（文書名）と呼ばれる値に限られます。
そこで、このドキュメントではこの値を利用して、下記の順序で設定内容を動的に適用する事を考えます。

1. 印刷前プログラムが、変換処理に関する設定内容を JSON 形式で一時ファイルに保存する。
2. 印刷前プログラムが、一時ファイルのパスを DocumentName に設定し、印刷コマンドを発行する。
3. 印刷後プログラムが、DocumentName に指定されたパスから設定内容を読み込み、その内容に応じて変換処理を実行する。

尚、この方法が適用可能な事例は、印刷前プログラムの DocumentName を自分で設定可能な場合に限られる事に注意下さい。

## プログラム連携方法の詳細

それでは、実際に印刷前後のプログラムを連携させる方法について記載します。
尚、ここで記載したプログラムは [Examples/Collaboration - Cube.Vp.Docs](https://github.com/cube-soft/Cube.Vp.Docs/tree/master/Examples/Collaboration)
にて完全な形で公開しています。

### 印刷前プログラム

印刷前プログラムは下記の通りです。

```cs
var json = Path.GetTempFileName();
var settings = new SettingFolder(Format.Json, json);

var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
settings.Value.Destination = Path.Combine(dir, "cubevp-collaboration.pdf");
settings.Value.PostProcess = PostProcess.OpenDirectory;
settings.Save();

var doc = new PrintDocument { DocumentName = json };
var dialog = new PrintDialog { Document = doc };
if (dialog.ShowDialog() == DialogResult.OK)
{
    doc.PrintPage += PrintDummy;
    doc.Print();
}
```

印刷前プログラムでは、まず、設定内容を保存するためのパスを決定します。その後、Format.Json および決定したパスを引数に
SettingFolder オブジェクトを初期化します。これによって、SettingFolder オブジェクトは、指定されたパスに対して
JSON 形式で設定内容を読み込み、または保存します。SettingFolder クラスの詳細については、
[SettingFolder - CubePDF SDK](https://clown.cube-soft.jp/entry/cubevp/sdk/converter#SettingFolder) を参照下さい。

次に、SettingFolder オブジェクトの Value プロパティに対して、必要な設定を行います。
例として、このプログラムではマイ・ドキュメントに cubevp-collaboration.pdf と言う名前で PDF ファイルを作成し、
完了後にファイルの保存されたフォルダーを開くように設定します。
設定が完了したら、Save メソッドを実行して先ほど決定したファイルに JSON 形式で設定内容を保存します。

最後に、.NET の [PrintDocument クラスの DocumentName プロパティ](https://docs.microsoft.com/ja-jp/dotnet/api/system.drawing.printing.printdocument.documentname)
に対して最初に決定したパスを設定し、このオブジェクトを用いて、印刷コマンド発行までに必要な残りの処理を実行します。

### 印刷後プログラム

印刷後プログラムは下記の通りです。

```cs
var src = new ArgumentCollection(args);
var settings = new SettingFolder(Format.Json, src.Options["DocumentName"]);
settings.Load();

var dest = settings.Value.Destination;
settings.Set(src);
settings.Value.Destination = dest;

using (var facade = new Facade(settings)) facade.Invoke();
```

印刷後プログラムでは、まず、プログラム引数から設定内容が JSON 形式で保存されているファイルへのパスを取得します。
CubeVP が印刷後プログラムを実行する際に指定するプログラム引数の仕様は、[CubeVP API](https://clown.cube-soft.jp/entry/cubevp/api) を参照下さい。
その後、Format.Json および取得したパスを引数に SettingFolder オブジェクトを初期化し、Load メソッドを実行します。

次に、プログラム引数の内容を SettingFolder オブジェクトに反映させるために Set メソッドを実行します。
尚、Set メソッドを実行すると、プログラム引数の内容に応じて変換後のファイルの保存場所が変更されます。
今回の例では、印刷前プログラムが決定したフォルダーおよびファイル名で保存するため、実行前に Value.Destination
プロパティの値をローカル変数に退避させ、該当プロパティに再代入すると言う回避策を導入しています。

各種設定処理が完了したら、最後に、SettingFolder オブジェクトを引数に Facade オブジェクトを初期化し、Invoke メソッドで変換処理を実行します。

## サンプルプログラムの実行方法

最後に、サンプルプログラムを実際に実行する方法について記載します。
まず、[Cube.Vp.Docs](https://github.com/cube-soft/Cube.Vp.Docs) リポジトリを clone する等してプロジェクトをダウンロードします。
そして、[Collaboration.sln](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Examples/Collaboration/Collaboration.sln) を
Visual Studio で開いてビルドを実行して下さい。

![CoReceiver.exe を指定して新しい仮想プリンターを作成](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Assets/Cube.Vp.Collaboration.ja.01.png?raw=true)

ビルドが正常に完了したら、CubeVPM を用いて印刷後プログラムの実行される仮想プリンターを登録します。
その後、印刷前プログラムを実行し、先ほど登録した仮想プリンターを選択して印刷コマンドを実行する事で挙動を確認できます。