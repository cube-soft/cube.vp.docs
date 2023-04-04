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
using System.IO;
using Cube.Collections;
using Cube.DataContract;
using Cube.Pdf.Converter;

namespace Collaboration.Receiver
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// CubeVP 連携デモ (CoReceiver) のメインプログラムを表します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class Program
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// CoReceiver のエントリポイントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Main(string[] args)
        {
            // ログ出力用の処理です。不要な場合、削除して構いません。
            InitLog(args);

            // まず、プログラム引数を ArgumentCollection クラスを用いて解析します。
            // 今回の連携デモでは JSON データの保存されているパスが DocumentName
            // オプション引数に指定されているため、SettingFolder クラスに対して
            // Format.Json および DocumentName の値を指定して初期化します。
            // その後、Load メソッドを実行する事により JSON 形式の設定内容が読み込まれます。
            var src = new ArgumentCollection(args);
            var settings = new SettingFolder(Format.Json, src.Options["DocumentName"]);
            settings.Load();

            // SettingFolder オブジェクトに対して、プログラム引数の内容を反映させるため
            // Set メソッドを実行します。
            // ただし、Set メソッドは Destination （保存場所）の値を上書きします。
            // 今回の連携デモでは、JSON データに記載された場所に PDF ファイルを保存する事を
            // 想定しているため、ローカル変数にいったん退避させた後、Set メソッド適用後に
            // 再度その値を反映させる事とします。
            var dest = settings.Value.Destination;
            settings.Set(src);
            settings.Value.Destination = dest;

            // 設定が完了したら、Facade クラスで変換処理を実行します。
            using (var facade = new Facade(settings)) facade.Invoke();

            // 最後に、印刷前プログラムが作成した一時ファイルを削除します。
            File.Delete(settings.Location);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitLog
        ///
        /// <summary>
        /// ログ出力に関する初期設定を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void InitLog(string[] args)
        {
            Cube.Logger.Configure(new Cube.Logging.NLog.LoggerSource());
            Cube.Logger.ObserveTaskException();
            Cube.Logger.Info(typeof(Program).Assembly);
            Cube.Logger.Info($"[ {string.Join(" ", args)} ]");
        }
    }
}
