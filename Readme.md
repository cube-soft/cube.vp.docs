CubeVP: Cube VirtualPrinter
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/

## はじめに

CubeVP は、ユーザ独自の仮想プリンタを構築するためのソフトウェア群であり、
下記のソフトウェアによって構成されています。

* CubeVpMon  
  仮想プリンタ用ポートモニタ
* CubeVPM: Cube VirtualPrinter Manager  
  仮想プリンタのインストールおよびアンインストール用 GUI アプリケーション
* CubeVPC: Cube VirtualPrinter Console  
  仮想プリンタのインストールおよびアンインストール用コマンドライン型アプリケーション

CubeVP の概要を知るための最初のステップとして、下記のチュートリアルを参照下さい。  
https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Tutorial.ja.md

CubeVP を使用するには、.NET Framework 3.5 以降が必要です（4.5.2 以降を強く推奨）。
ご利用の端末にインストールされていない場合、以下の URL からダウンロードして下さい。

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

CubeVP の一部は、Apache License, Version 2.0 ライセンスで配布されている成果物を
用いて実現されています。  
http://www.apache.org/licenses/LICENSE-2.0

## 詳細

### CubeVP API

「任意のアプリケーション（以下、アプリケーションと呼ぶ）」から印刷が実行されると、
CubeVpMon （仮想プリンタ用ポートモニタ）は、PScript5 （Windows 標準プリンタドライバ）が
生成した PostScript 形式の印刷データを一時ファイルに保存します。
その後、「あらかじめ指定されたアプリケーション（以下、ユーザプログラムと呼ぶ）」を
以下の引数で実行します。尚、仮想プリンタのインストール時にユーザが引数を指定した
場合、上記に加えてそれらの引数も追加されます。

```
UserProgram.exe
    -DocumentName   DOCUMENT_NAME
    -InputFile      C:\ProgramData\CubeSoft\CubeVP\PSTF012.tmp
    -Digest         SHA256_HASH
    -UserName       USER_NAME
    -MachineName    MACHINE_NAME
    -DeleteOnClose
    -Exec           Path\To\YourProgram.exe
```

それぞれの引数の内容は下記の通りです。

* **DocumentName**  
  アプリケーションは、印刷コマンドを実行する際に「ドキュメント名」と呼ばれる値を
  指定して実行します。この引数は、指定されたドキュメント名をユーザプログラムに
  伝達します。
* **InputFile**  
  仮想プリンタが、印刷データ (PostScript) を保存した一時ファイルのパスを表します。
  一時ファイルは、仮想プリンタのインストール時に作業フォルダ (Temp) で指定した
  フォルダに保存されます。
* **Digest**  
  一時ファイルの内容に対して、SHA-256 のハッシュ値を計算した結果を表します。
  この値は、印刷データの破損や改ざんを検知する時などに使用されます。  
  例: 84e72201691f3869287ec95923c9b2b102f188e758d467d66b7d77690dc61048
* **UserName**  
  印刷コマンドが実行された時のユーザ名を表します。
* **MachineName**  
  印刷コマンドが実行された端末の名前を表します。
* **DeleteOnClose**  
  仮想プリンタが、**InputFile** 引数に指定したファイルを終了時に削除して欲しい事を
  伝えるための引数です。仮想プリンタは、生成された一時ファイルをそのままの状態で
  終了します。そのため、一時ファイルに対する削除などの後処理はユーザプログラムの
  責務となります。
* **Exec**  
  ユーザプログラム自身のパスが指定されます。
  この引数は、SYSTEM アカウントからログオン中のユーザアカウントに切り替えるための
  プログラムである CubeProxy.exe で利用されています。

CubeVpMon 経由で実行可能なユーザプログラムを作成するには、下記の情報も参考にして下さい。

* CubeVP API  
  https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Api.ja.md
* Cube.Pdf.Converter ... CubePDF 変換処理用ライブラリ  
  https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Pdf.Converter.ja.md
* Cube.Pdf.Ghostscript ... Ghostscript ラッパーライブラリ  
  https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Pdf.Ghostscript.ja.md

### CubeVPM: Cube VirtualPrinter Monitor

Cube VirtualPrinter Manager (CubeVPM) は、仮想プリンタのインストール
およびアンインストールを実行するための GUI アプリケーションです。
CubeVPM を初めて起動すると、まず、新しい仮想プリンタを追加する画面が表示されます。
追加時に設定する項目は下記の通りです。

* **プリンタ名**  
  指定された名前で新しい仮想プリンタが作成されます。
  プリンタ名には英数字の他、日本語を指定する事も可能です。
* **アプリケーション**  
  この仮想プリンタを指定して印刷が実行された時に実行されるアプリケーションへの
  パスを指定します。指定されたアプリケーションに対して何らかの引数が必要な場合、
  その下のテキストボックスに入力します。
  尚、仮想プリンタは印刷データ (PostScript) を一時保存したパスや印刷時に指定された
  ドキュメント名など、いくつかの引数は必ず指定されます。
* **作業フォルダ**  
  仮想プリンタが印刷データを一時保存するフォルダを指定します。
  特に理由がなければ初期設定をそのままご利用下さい。
  独自に設定する場合、SYSTEM アカウントおよびログオン中のユーザアカウントがともに
  アクセス可能なフォルダを指定する必要があります。例えば、「ユーザー」下の
  フォルダは SYSTEMアカウントがアクセスできないのでご注意下さい。
* **アプリケーションが終了するまで次の印刷ジョブを実行しない**  
  このオプションを有効にすると、プリントスプーラに複数の印刷ジョブが登録された
  場合に、指定されたアプリケーションの実行が終了するまで次の印刷ジョブの開始を
  待つようになります。無効の場合はアプリケーションが実行中かどうかに関わらず、
  印刷ジョブが登録されると即座にアプリケーションの実行を試みます。
* **アプリケーションをユーザアカウントで実行する**  
  印刷は SYSTEM アカウントで実行されるため、そのままの状態だと指定された
  アプリケーションも SYSTEM アカウントで実行される事となります。
  このオプションを有効にすると、仮想プリンタがログオン中のユーザで指定された
  アプリケーションを実行するようになります。
  尚、ログオン中のアカウントを特定する際、印刷コマンドを発行したアプリケーションの
  ユーザ名を利用します。このため、サービス経由など、ログオン中のユーザ以外の
  アカウントから印刷を実行した場合、このオプションが有効に機能しないのでご注意下さい。

プリンタ名以外の項目は、追加後に変更する事ができます。
変更したい項目を修正した後、適用ボタンを押して下さい。
また、仮想プリンタが不要になった場合は、削除ボタンを押して下さい。

CubeVPM に関する情報は、下記も参照下さい。  
https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Installer.Gui.ja.md

### CubeVPC: Cube VirtualPrinter Console

Cube VirtualPrinter Console (CubeVPC) は、仮想プリンタのインストール
およびアンインストールを実行するためのコマンドライン型アプリケーションです。
コマンドラインおよび設定用 JSON ファイルの詳細に関しては、下記を参照下さい。  
https://github.com/cube-soft/Cube.Vp.Docs/blob/master/Documents/Cube.Vp.Installer.Cli.ja.md

## 問題が発生した場合

CubeVP は、**C:\ProgramData\CubeSoft\CubeVP\Log** フォルダに実行ログを
出力しています。問題が発生した時は、これらのログを添付して support@cube-soft.jp
までご連絡お願いします。

## 更新履歴

* 2010/06/21 version 1.0.0
    - 最初の公開バージョン