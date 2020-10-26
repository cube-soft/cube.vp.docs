CubeVPC
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubevp/

## はじめに

Cube VirtualPrinter Console (CubeVPC) は、仮想プリンターのインストール
およびアンインストールを実行するためのコマンドライン型アプリケーションです。
CubeVPC を使用するには .NET Framework 3.5 以降が必要です（4.5.2 以降を強く推奨）。
もし、ご利用の端末にインストールされていない場合、以下の URL からダウンロードして下さい。

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

## 使用方法

```
CubeVpc.exe JSON -Command COMMAND [OPTIONS]
```

CubeVPC の必須パラメータは **JSON** および **COMMAND** の 2 種類です。
**JSON** には、インストールまたはアンインストールするプリンター構成を記載した
JSON 形式のファイルへのパスを記載します。
**COMMAND** には、下記の 4 種類の中から一つを指定します。

* **Install**  
  指定された構成でプリンター等をインストールします。
  既にインストールされている項目は、処理をスキップします。
* **Uninstall**  
  指定された構成でプリンター等をアンインストールします。
* **Reinstall**  
  指定された構成でプリンター等をいったんアンインストールした後、
  再度インストールを実行します。
* **Update** (3.0.0 or later)  
  必要と判断された場合のみ、指定された構成でインストールを実行します。
  インストール時の挙動は **Reinstall** と同様です。

**OPTIONS** に指定可能なオプションは下記の通りです。

* **-Resource DIRECTORY**  
  JSON ファイルに記載されている、インストールに必要な各種ファイルが存在する
  フォルダへのパスを指定します。
* **-Relative**  
  コマンドライン上で指定されたパスを CubeVpc.exe が存在する
  フォルダからの相対パスとして認識します。
* **-Force**  
  JSON ファイルに記載されたプリンタードライバやポートモニタに依存する全ての要素を
  強制的にアンインストールします。このオプションを指定した場合、意図しない
  プリンタードライバまでアンインストールされる可能性があります。また、オプションを
  指定しない場合は、対象となるプリンタードライバ等が他のプリンターに使用されている
  などの理由で、アンインストールに失敗する事があります。
* **-Retry COUNT**  
  プリンター等のインストールまたはアンインストールに失敗した時に再試行する回数を
  指定します。
* **-Timeout SECOND**  
  プリンター等のインストールまたはアンインストール実行時のタイムアウト時間の初期値を
  秒単位で指定します。実際のタイムアウト時間は、実行に失敗する度に等倍されます。
  例えば 30 を指定した場合、実際のタイムアウト時間は 30 秒、60 秒、90 秒、... と
  増加していきます。

CubeVPC の実行コマンド例は下記の通りです。

```
CubeVPC.exe CubePrinter.json
    -Command Reinstall
    -Relative
    -Resource Printers
    -Retry 6
    -Timeout 30
```

## JSON 仕様

CubeVPC に指定する構成用 JSON ファイルの仕様は下記の通りです。

```
{
    "Timestamp": "YYYY/mm/dd HH:MM:ss",
    "Printers": [ ... ],
    "PrinterDrivers": [ ... ],
    "Ports": [ ... ],
    "PortMonitors": [ ... ]
}
```

構成要素は **Timestamp**, **Printers**, **PrinterDrivers**, **Ports**, **PortMonitors** の
5 種類であり、**Timestamp** 以外はいずれも配列形式で複数の要素を指定する事ができます。
また、既にインストールされている要素を利用する等の理由で新たにインストールする
ものが存在しない場合、該当項目を省略する事も可能です。

**Timestamp** は、この JSON ファイルの最終更新日時を記載します。
CubeVPC は、この値とインストール時に生成するスタンプファイルを比較する事で、
インストール処理を省略可能かどうかを判断します。

### PortMonitors

**PortMonitors** には、インストールまたはアンインストールするポートモニタを
指定します。指定項目は下記の通りです。

* **Name** (string)  
  インストールまたはアンインストールするポートモニタの名前を指定します。
* **FileName** (string)  
  モジュール名を指定します。
* **Config** (string)  
  UI モジュール名を指定します。

尚、**FileName** および **Config** で指定されたモジュールは **-Resource**
オプションで指定されたフォルダに存在するものとします。

### Ports

**Ports** には、インストールまたはアンインストールするポートを指定します。
指定項目は下記の通りです。

* **Name** (string)  
  インストールまたはアンインストールするポートの名前を指定します。
* **MonitorName** (string)  
  ポートが利用するポートモニタの名前を指定します。
  ここで指定されるポートモニタは、既にインストールされているか、または、
  同じ構成ファイルに記述されている必要があります。
* **Application** (string)  
  ポートが実行するアプリケーションのパスを指定します。
* **Arguments** (string)  
  ポートがアプリケーションを実行する際に指定する引数を指定します。
  実際にポートがアプリケーションを実行する際には、ここで指定されたもの以外の
  引数が含まれる事があります。
* **Temp** (string)  
  一時ファイル等を保存するフォルダのパスを指定します。
  ログイン中のユーザーおよび SYSTEM アカウントが書き込み可能なフォルダを指定して下さい。
* **WaitForExit** (bool)  
  アプリケーションが終了するまで、次の印刷ジョブの処理を待機するかどうかを
  指定します。false の場合、複数のプロセスが同時に実行される可能性があります。
* **RunAsUser** (bool)  
  アプリケーションをログオン中のユーザーで実行するかどうかを指定します。
  false の場合、SYSTEM アカウントで実行されます。尚、**Proxy** の項目が省略
  されている場合、この項目に関わらず SYSTEM アカウントで実行されます。
* **Proxy** (string)  
  SYSTEM アカウントからログオン中のユーザーに切り替えるためのプログラムのパスを
  指定します。該当機能を自ら実装する等の場合を除き、同梱する CubeProxy.exe の
  パスを指定して下さい。

### PrinterDrivers

**PrinterDrivers** には、インストールまたはアンインストールするプリンタードライバを
指定します。指定項目は下記の通りです。

* **Name** (string)  
  インストールまたはアンインストールするプリンタードライバの名前を指定します。
* **MonitorName** (string)  
  プリンタードライバが利用するポートモニタの名前を指定します。
  ここで指定されるポートモニタは、既にインストールされているか、または、同じ構成
  ファイルに記述されている必要があります。
* **Data** (string)  
  PostScript Printer Driver (PPD) ファイルを指定します。指定された PPD ファイルは
  **-Resource** オプションで指定されたフォルダに存在するものとします。
* **FileName** (string)  
  モジュール名を指定します（例: pscript5.dll）。
* **Config** (string)  
  UI モジュール名を指定します（例: psui5.dll）。
* **Help** (string)  
  ヘルプ用ファイル名を指定します（例: pscript.hlp）。
* **Dependencies** (string 配列)  
  その他の依存ファイルを配列形式で指定します。
* **Repository** (string)  
  **FileName**, **Config**, **Help**, **Dependencies** で指定された各種モジュールを
  DriverStore フォルダから検索する際に使用します。例えば、64bit 環境において
  ntprint を指定した場合、  
  DriverStore/FileRepository/ntprint.ing_amd64_xxxxxxxxxxxxxxx/amd64  
  からの取得を試みます。

### Printers

**Printers** には、インストールまたはアンインストールするプリンターを指定します。
指定可能な項目は、指定項目は下記の通りです。

* **Name** (string)  
  インストールまたはアンインストールするプリンター名を指定します。
* **ShareName** (string)  
  プリンターを LAN 等のネットワーク上で共有する時の名前を指定します。
* **DriverName** (string)  
  プリンターが利用するプリンタードライバ名を指定します。
  指定されたプリンタードライバは既にインストールされているか、または、同じ構成
  ファイルに記述されている必要があります。
* **PortName** (string)  
  プリンターが利用するポート名を指定します。
  指定されたポートは既にインストールされているか、または、同じ構成ファイルに記述
  されている必要があります。

### Examples

CubeVPC に指定する JSON ファイルの構成例は下記の通りです。

```
{
    "Timestamp" : "2020/10/26 12:00:00",
    "Printers" : [{
        "Name"         : "CustomPDF",
        "ShareName"    : "CustomPDF",
        "DriverName"   : "CubeVPD",
        "PortName"     : "CubeVP0:"
    }],
    "PrinterDrivers" : [{
        "Name"         : "CubeVPD",
        "MonitorName"  : "CubeVpMon",
        "Data"         : "CubeVPD.ppd",
        "Repository"   : "ntprint",
        "FileName"     : "pscript5.dll",
        "Config"       : "ps5ui.dll",
        "Help"         : "pscript.hlp",
        "Dependencies" : [ "pscript.ntf", "pscrptfe.ntf", "ps_schm.gdl" ]
    }],
    "Ports" : [{
        "Name"         : "CubeVP0:",
        "MonitorName"  : "CubeVpMon",
        "Application"  : "C:\\Program Files\\CubePDF\\CubePdf.exe",
        "Proxy"        : "C:\\Program Files\\CubeVP\\CubeProxy.exe",
        "Arguments"    : "",
        "Temp"         : "C:\\ProgramData\\CubeSoft\\CubeVP",
        "WaitForExit"  : false,
        "RunAsUser"    : true
    }],
    "PortMonitors" : [{
        "Name"         : "CubeVpMon",
        "FileName"     : "cubevpmon.dll",
        "Config"       : "cubevpmonui.dll"
    }]
}
```