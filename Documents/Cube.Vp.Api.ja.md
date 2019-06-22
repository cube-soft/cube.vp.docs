CubeVP API
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

## 概要

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
