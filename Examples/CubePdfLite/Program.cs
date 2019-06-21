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
using Cube.FileSystem;
using Cube.Pdf.Converter;
using System;
using System.Reflection;

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
            // 1. 初期設定ではレジストリの下記のサブキーが対象となります。
            // HKCU\Software\CubeSoft\CubePDF\v2
            // 例えば、HKCU\Software\Foo\Bar を対象とする場合、下記のように初期化して下さい。
            //
            // var settings = new SettingFolder(
            //     Assembly.GetExecutingAssembly(),
            //     Cube.DataContract.Registry,
            //     @"Foo\Bar"
            // );
            //
            // また、SettingFolder はレジストリ以外に、JSON 形式にも対応しています。
            // JSON ファイルを対象とする場合、第 2 引数を Cube.DataContract.Json とし、
            // 対象とする JSON ファイルへの絶対パスを第 3 引数に指定して下さい。
            var settings = new SettingFolder(Assembly.GetExecutingAssembly());

            // 2. Load() で対象としているレジストリ、または JSON ファイルからのロード、
            // Set(string[]) で仮想プリンタからの引数を解析します。
            // 設定内容は Value プロパティに格納されています。
            // Value 中の各種プロパティは、手動で変更する事も可能です。
            settings.Load(); // レジストリの設定をロード
            settings.Value.Destination = GetDirectory(settings.Value, settings.IO);
            settings.Set(args); // 仮想プリンタからの引数を解析

            // 3. 設定内容のロードが完了したら、Facade クラスで変換処理を実行します。
            using (var facade = new Facade(settings)) facade.Invoke();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectory
        ///
        /// <summary>
        /// レジストリからロードされた内容はファイル名が含まれている可能性があります。
        /// このメソッドでは、フォルダ名と予想されるパスを返します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static string GetDirectory(SettingValue src, IO io)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                if (string.IsNullOrEmpty(src.Destination)) return desktop;
                var dest = io.Get(src.Destination);
                return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
            }
            catch { return desktop; }
        }

    }
}