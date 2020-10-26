CubeVP API
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

## 概要

このドキュメントでは、CubeVP によって作成される仮想プリンターが連携する
ユーザープログラムを実行する際の仕様 (API) について記載します。

「任意のアプリケーション（以下、アプリケーションと呼ぶ）」から印刷が実行されると、
仮想プリンターは、Windows 標準プリンタードライバである PScript5 が生成した PostScript
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
  印刷コマンドが実行された時のユーザー名を表します。
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
