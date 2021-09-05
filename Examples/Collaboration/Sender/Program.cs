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
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Cube.FileSystem.DataContract;
using Cube.Logging;
using Cube.Pdf.Converter;

namespace Collaboration.Sender
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// CubeVP 連携デモ (CoSender) のメインプログラムを表します。
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
        /// CoSender のエントリポイントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Main(string[] args)
        {
            // ログ出力用の処理です。不要な場合、削除して構いません。
            InitLog(args);

            // まず、JSON データを保存するための一時ファイル名を決定します。
            // 次に、SettingFolder クラスに対して Format.Json および
            // 先ほど決定したパスを指定して初期化します。
            var json = Path.GetTempFileName();
            var settings = new SettingFolder(Format.Json, json);

            // 初期化完了後、Value プロパティに対して必要な設定内容を反映させます。
            // 例として、マイ・ドキュメントに cubevp-collaboration.pdf と言う
            // ファイル名で保存し、保存されたフォルダーを開くように設定してみます。
            // 必要な設定が完了したら、Save メソッドで設定内容を JSON 形式で保存します。
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            settings.Value.Destination = Path.Combine(dir, "cubevp-collaboration.pdf");
            settings.Value.PostProcess = PostProcess.OpenDirectory;
            settings.Save();

            // PrintDocument クラスの DocumentName プロパティに対して
            // 最初に決定した一時ファイルのパスを指定します。
            // その後、印刷ダイアログの表示等、必要な印刷処理を実行して終了です。
            var doc = new PrintDocument { DocumentName = json };
            var dialog = new PrintDialog { Document = doc };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                doc.PrintPage += PrintDummy;
                doc.Print();
            }
            else File.Delete(json);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrintDummy
        ///
        /// <summary>
        /// ダミーデータの印刷処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void PrintDummy(object s, PrintPageEventArgs e)
        {
            var text  = "CubeVP collaboration demo.";
            var font  = SystemFonts.DefaultFont;
            var brush = SystemBrushes.WindowText;
            var x = e.PageSettings.PaperSize.Width  * 0.1f;
            var y = e.PageSettings.PaperSize.Height * 0.1f;

            e.Graphics.DrawString(text, font, brush, x, y);
            e.HasMorePages = false;
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
            var src = typeof(Program);
            _ = Logger.ObserveTaskException();
            src.LogInfo(src.Assembly);
            src.LogInfo($"[ {string.Join(" ", args)} ]");
        }
    }
}
