/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System.Reflection;
using Cube;
using Cube.Pdf.Converter;

namespace CubePdfLite
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// CubePDF Lite のメインプログラムを表します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class Program
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// CubePDF Lite のエントリポイントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Main(string[] args)
        {
            Logger.ObserveTaskException();
            Logger.Info(typeof(Program), Assembly.GetExecutingAssembly());
            Logger.Info(typeof(Program), $"[ {string.Join(" ", args)} ]");

            // 1. 初期設定ではレジストリの下記のサブキーが対象となります。
            // HKCU\Software\CubeSoft\CubePDF\v2
            // 対象とするサブキーを変更したい場合、コンストラクタで設定します。
            // 例えば、HKCU\Software\Foo\Bar を対象とするには下記のように記述して下さい。
            //
            // var settings = new SettingFolder(
            //     Assembly.GetExecutingAssembly(),
            //     Cube.DataContract.Format.Registry,
            //     @"Foo\Bar"
            // );
            //
            // また、SettingFolder はレジストリ以外に JSON 形式にも対応しています。
            // JSON ファイルを対象とする場合、第 2 引数を Cube.DataContract.Format.Json とし、
            // 対象とする JSON ファイルへの絶対パスを第 3 引数に指定して下さい。
            var settings = new SettingFolder(Assembly.GetExecutingAssembly());

            // 2. Load() で対象としているレジストリ等から設定内容を読み込み、
            // Set(string[]) で仮想プリンタからの引数を解析します。
            settings.Load(); // レジストリの設定をロード
            settings.Set(args); // 仮想プリンタからの引数を解析

            // 3. 設定内容は Value プロパティが保持します。
            // Value 中の各種プロパティは、手動で変更する事も可能です。
            // 例として、ここでは PostProcess を「何もしない」に設定しています。
            // 設定可能な内容については、下記も参照下さい。
            // https://docs.cube-soft.jp/entry/cubevp/sdk/converter
            //
            // 尚、CubePDF SDK 3.0.0 より Set(string[]) 実行時に
            // Destination から末尾のファイル名を除去（DocumaneName.Value に置換）
            // する形に変更されました。そのため、1.0.0 時に Destination に対して
            // 行っていた処理は不要となりました。
            settings.Value.PostProcess = PostProcess.None;

            // 4. 設定内容のロードが完了したら、Facade クラスで変換処理を実行します。
            using (var facade = new Facade(settings)) facade.Invoke();
        }
    }
}