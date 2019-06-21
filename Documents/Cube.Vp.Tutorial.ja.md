Tutorial for CubeVP
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## 概要

このドキュメントでは、CubeVP の使い方を学ぶ最初のステップとして、メイン画面の
表示されない CubePDF を実行するための仮想プリンタをご利用の端末に追加してみます。

まず、[Releases - Cube.Vp.Docs](https://github.com/cube-soft/Cube.Vp.Docs/releases) より
CubePDF の変換処理のみを実装した CubePDF Lite の圧縮ファイルをダウンロードして下さい。
お使いの Windows が 32bit 版の場合は末尾が x86 のもの、64bit 版の場合は末尾が x64 のものとなります。
どちらを利用して良いか分からない場合、32bit 版の方をダウンロードして下さい。

次に、ダウンロードした圧縮ファイルを適当なフォルダに解凍・展開して下さい。
この際、重要な点として、解凍フォルダは SYSTEM アカウントもアクセス可能である必要があります。
例えば、**SYSTEM アカウントは各ユーザのデスクトップにはアクセスできない** のでご注意下さい。
ここでは、下記フォルダに解凍・展開したとします。

```C:\UserProgram```

解凍が完了したら、デスクトップのショートカット等から CubeVPM を起動します。
起動すると、下図のような画面が表示されますので、プリンタ名は適当な名前、アプリケーションには先ほど解凍したフォルダにある CubePdfLite.exe を指定して OK ボタンを押します。

![CubePdfLite.exe を指定して新しい仮想プリンタを作成](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Assets/Cube.Vp.Tutorial.ja.01.png?raw=true)

これで仮想プリンタの作成は完了です。Google Chrome など、適当なアプリケーションから印刷ボタンを押すと、先ほど作成した名前のプリンタが一覧に表示されるので、それを選択して印刷を実行してみます。初期設定では、変換された PDF ファイルがデスクトップに保存され、そのファイルが Adobe Acrobat Reader などの PDF ファイルに関連付けられたアプリケーションで表示されます。

## 関連情報

CubePDF の変換処理に対して何らかのカスタマイズを行いたい場合、[Cube.Pdf.Converter のドキュメント](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Pdf.Converter.ja.md) を参照下さい。また、[Examples/CubePdfLite](https://github.com/cube-soft/Cube.Vp.Docs/tree/master/Examples/CubePdfLite) には、今回使用した CubePDF Lite のプロジェクト一式がありますので、テンプレート用のプロジェクトとして利用する事もできます。その他の、関連するドキュメントは下記の通りです。

* [CubeVP API の説明](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Api.ja.md)
* [CubeVPM (GUI インストーラ) の説明](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Installer.Gui.ja.md)
* [CubeVPC (CLI インストーラ) の説明](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Installer.Cli.ja.md)