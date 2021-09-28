CubeVP: Cube VirtualPrinter
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

## はじめに

[CubeVP](https://www.cube-soft.jp/cubevp/) は、
[CubePDF](https://www.cube-soft.jp/cubepdf/) でも利用されている、
ユーザー独自の仮想プリンターを構築するためのソフトウェア群であり、下記のソフトウェアによって構成されています。

* CubeVpMon  
  仮想プリンターを構築するためのシステムライブラリ
* CubeVPM: Cube VirtualPrinter Manager  
  仮想プリンターのインストールおよびアンインストール用 GUI アプリケーション
* CubeVPC: Cube VirtualPrinter Console  
  仮想プリンターのインストールおよびアンインストール用コマンドライン型アプリケーション

また、仮想プリンターと連携するユーザープログラムの作成補助として、
別途 [CubePDF SDK](https://github.com/cube-soft/Cube.Pdf) を公開しています。
これは、CubePDF で使用されている変換処理をそのままライブラリ化したものとなっています。

CubeVP の概要を知るための最初のステップとして、下記を参照下さい。

* 目次  
  https://docs.cube-soft.jp/entry/cubevp
* CubeVP チュートリアル  
  https://docs.cube-soft.jp/entry/cubevp/tutorial

CubeVP を使用するには .NET Framework 3.5 以降が必要です（4.5.2 以降を強く推奨）。
ご利用の端末にインストールされていない場合、以下の URL からダウンロードして下さい。

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

CubeVP の一部は、[Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0)
ライセンスで配布されている成果物を用いて実現されています。  

## 詳細

### CubeVP API

「任意のアプリケーション（以下、アプリケーションと呼ぶ）」から印刷が実行されると、
CubeVpMon は、Windows 標準プリンタードライバである PScript5 が生成した PostScript
形式の印刷データを一時ファイルに保存します。
その後、「あらかじめ指定されたアプリケーション（以下、ユーザープログラムと呼ぶ）」を
下記の引数で実行します。尚、仮想プリンターのインストール時にユーザが引数を指定した場合、
それらの引数も併せて指定されます。

```
UserProgram.exe
    -DocumentName   DOCUMENT_NAME
    -InputFile      C:\ProgramData\CubeSoft\CubeVP\PSTF012.tmp
    -Digest         SHA256_HASH
    -UserName       USER_NAME
    -MachineName    MACHINE_NAME
    -DeleteOnClose
    -Exec           Path\To\UserProgram.exe
```

それぞれの引数の内容は下記の通りです。

* **DocumentName**  
  アプリケーションは、印刷コマンドを実行する際に「ドキュメント名」と呼ばれる値を指定して実行します。
  この引数は、指定されたドキュメント名をユーザープログラムに伝達します。
* **InputFile**  
  仮想プリンターが、印刷データ (PostScript) を保存した一時ファイルのパスを表します。
  一時ファイルは、仮想プリンターのインストール時に作業フォルダ (Temp) で指定したフォルダに保存されます。
* **Digest**  
  一時ファイルの内容に対して、SHA-256 のハッシュ値を計算した結果を表します。
  この値は、印刷データの破損や改ざんを検知する時などに使用されます。  
  例: 84e72201691f3869287ec95923c9b2b102f188e758d467d66b7d77690dc61048
* **UserName**  
  印刷コマンドが実行された時のユーザ名を表します。
* **MachineName**  
  印刷コマンドが実行された端末の名前を表します。
* **DeleteOnClose**  
  仮想プリンターが、**InputFile** で指定したファイルを削除して欲しい事を伝えるための引数です。
  仮想プリンターは、生成した一時ファイルをそのままの状態で終了します。
  そのため、一時ファイルに対する削除などの後処理はユーザープログラムの責務となります。
* **Exec**  
  ユーザープログラム自身のパスが指定されます。
  この引数は、SYSTEM アカウントからログオン中のユーザーアカウントに切り替えるプログラム
  CubeProxy.exe で利用されています。
  そのため、CubeProxy.exe を経由せずに直接ユーザープログラムが実行された時など、
  **Exec** が指定されない場合もあります。

CubeVpMon 経由で実行可能なユーザープログラムを作成するには、下記の情報も参考にして下さい。

* CubeVP API  
  https://docs.cube-soft.jp/entry/cubevp/api
* Cube.Pdf.Converter ... CubePDF 変換処理用ライブラリ  
  https://docs.cube-soft.jp/entry/cubevp/sdk/converter
* Cube.Pdf.Ghostscript ... Ghostscript ラッパーライブラリ  
  https://docs.cube-soft.jp/entry/cubevp/sdk/ghostscript

### CubeVPM: Cube VirtualPrinter Manager

CubeVPM は、仮想プリンターのインストールおよびアンインストールを実行するための
GUI アプリケーションです。
CubeVPM を初めて起動すると、新しい仮想プリンターを追加する画面が表示されます。
追加時に設定する項目は下記の通りです。

* **プリンター名**  
  指定された名前で新しい仮想プリンターが作成されます。
  プリンター名には英数字の他、日本語を指定する事も可能です。
* **アプリケーション**  
  印刷が実行された時に実行されるアプリケーションへのパスを指定します。
  指定されたアプリケーションに対して何らかの引数が必要な場合、
  その下のテキストボックスに入力します。
  尚、仮想プリンターは印刷データを一時保存したパスや印刷時に指定されたドキュメント名など、
  いくつかの引数は必ず指定されます。
* **作業フォルダ**  
  仮想プリンターが印刷データを一時保存するフォルダを指定します。
  特に理由がなければ初期設定をそのままご利用下さい。
  尚、作業フォルダは SYSTEM アカウントおよびログオン中のユーザがともにアクセス可能な必要があります。
  「ユーザー (Users)」フォルダは SYSTEMアカウントがアクセスできないのでご注意下さい。
* **アプリケーションが終了するまで次の印刷ジョブを実行しない**  
  このオプションを有効にすると、プリントスプーラに複数の印刷ジョブが登録された場合に、
  指定されたアプリケーションの実行が終了するまで次の印刷ジョブの開始を待つようになります。
  無効の場合はアプリケーションが実行中かどうかに関わらず、
  印刷ジョブが登録されると即座にアプリケーションの実行を試みます。
* **アプリケーションをユーザアカウントで実行する**  
  印刷は SYSTEM アカウントで実行されるため、そのままの状態だと指定されたアプリケーションも
  SYSTEM アカウントで実行される事となります。このオプションを有効にすると、
  仮想プリンターがログオン中のユーザで指定されたアプリケーションを実行するようになります。
  尚、アカウントを特定する際に、印刷コマンドを実行したアプリケーションのユーザ名を利用します。
  このため、サービス経由などログオン中のユーザ以外のアカウントから印刷を実行した場合、
  このオプションが有効に機能しないのでご注意下さい。

プリンター名以外の項目は、追加後に変更する事ができます。
変更したい項目を修正した後、適用ボタンを押して下さい。
また、仮想プリンターが不要になった場合は、削除ボタンを押して下さい。

CubeVPM に関する情報は、下記も参照下さい。  
https://docs.cube-soft.jp/entry/cubevp/gui

### CubeVPC: Cube VirtualPrinter Console

CubeVPC は、仮想プリンターのインストールおよびアンインストールを実行するためのコマンドライン型アプリケーションです。
コマンドラインおよび設定用 JSON ファイルの詳細に関しては下記を参照下さい。  
https://docs.cube-soft.jp/entry/cubevp/cli

## 問題が発生した場合、および改善要望について

CubeVP は、```C:\ProgramData\CubeSoft\CubeVP\Log``` フォルダに実行ログを出力しています。
問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡お願いします。
ただし、メール対応に関しては Personal ライセンスは対象外となります。

公開ライブラリの利用方法が分からないなどの理由で、ドキュメントの加筆をご要望の場合
[Issues](https://github.com/cube-soft/Cube.Vp.Docs/issues) にご登録下さい。
ただし、全ての要望に応えられるとは限らない旨、ご了承下さい。
また、自身の手によって修正した内容を適用して欲しい場合、下記の方法で Pull Request をお願いします。

1. 下記 URL から Cube.Vp.Docs リポジトリを fork します。  
   https://github.com/cube-soft/Cube.Vp.Docs/fork
2. 自身の修正を fork したリポジトリに反映し、push します。
3. GitHub 上で fork したリポジトリにアクセスし、New pull request ボタンから新しい Pull Request を作成します。

Cube.Pdf ライブラリへの修正についても、同様に
[Cube.Pdf リポジトリ](https://github.com/cube-soft/Cube.Pdf) に対して
Pull Request をお願いします。  

## 更新履歴

* 2021-09-29 version 5.0.0
    - CubePDF SDK 5.0.0 へ更新
* 2021-09-07 version 4.0.2
    - CubePDF SDK 4.0.2 へ更新
* 2021-08-06 version 4.0.1
    - CubePDF SDK 4.0.1 へ更新
* 2021-07-12 version 4.0.0
    - CubePDF SDK 4.0.0 へ更新
    - 内部処理を修正
    - CubeVP のバージョン表記を CubePDF SDK と統一（3.0.0 をスキップ）
* 2020-12-07 version 2.0.0
    - 仮想プリンターの再インストールが必要ない場合に省略するように修正
    - 仮想プリンターのインストール処理を改善
    - ログ出力用ライブラリを log4net から NLog に変更
    - CubePDF SDK 3.1.1 へ更新
* 2019-06-21 version 1.0.0
    - 最初の公開バージョン