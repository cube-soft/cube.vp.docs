Tutorial for CubeVP
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

## 概要

このドキュメントでは、CubeVP の使い方を学ぶ最初のステップとして、メイン画面の表示されない
CubePDF を実行するための仮想プリンタを登録してみます。
このチュートリアルを実行する前に [CubeVP の Web ページ](https://www.cube-soft.jp/cubevp/) より、
あらかじめ評価版のインストールをお願いします。

まず、[Releases - Cube.Vp.Docs](https://github.com/cube-soft/Cube.Vp.Docs/releases) から
CubePDF の変換処理のみを実行する CubePDF Lite をダウンロードします。
ご利用端末の Windows が 32bit 版の場合は末尾が x86 のもの、64bit 版の場合は末尾が x64 のものとなります。
どちらを利用して良いか分からない場合、32bit 版のものをダウンロードして下さい。

次に、ダウンロードした圧縮ファイルを適当なフォルダに解凍・展開して下さい。
この際、解凍フォルダは SYSTEM アカウントもアクセス可能である必要があります。
**SYSTEM アカウントはユーザのデスクトップにはアクセスできない** のでご注意下さい。
今回は、下記のフォルダに解凍したとします。

```
C:\UserProgram
```

解凍が完了したら、デスクトップのショートカット等から CubeVPM を起動します。
起動すると、下図のような画面が表示されますので、プリンタ名は適当な名前、
アプリケーションには先ほど解凍したフォルダにある CubePdfLite.exe を指定して OK ボタンを押します。

![CubePdfLite.exe を指定して新しい仮想プリンタを作成](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Assets/Cube.Vp.Tutorial.ja.01.png?raw=true)

これで仮想プリンタの作成は完了です。
Google Chrome など、適当なアプリケーションから印刷ボタンを押すと、
先ほど作成した名前のプリンタが一覧に表示されるので、それを選択して印刷を実行してみます。
初期設定では、変換された PDF ファイルがデスクトップに保存され、
そのファイルが Adobe Acrobat Reader などの PDF ファイルに関連付けられたアプリケーションで表示されます。

## 関連情報

CubePDF の変換処理に対して何らかのカスタマイズを行いたい場合、
[Cube.Pdf.Converter のドキュメント](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Pdf.Converter.ja.md) を参照下さい。
また、[Examples/CubePdfLite](https://github.com/cube-soft/Cube.Vp.Docs/tree/master/Examples/CubePdfLite) には、
今回使用した CubePDF Lite のプロジェクト一式がありますので、テンプレート用のプロジェクトとして利用する事もできます。
その他のドキュメントは [目次](https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Index.ja.md) を参照下さい。