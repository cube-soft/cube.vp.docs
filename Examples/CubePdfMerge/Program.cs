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
namespace CubePdfMerge;

using System;
using Cube;
using Cube.Collections;
using Cube.Pdf;
using Cube.Pdf.Itext;
using Cube.Pdf.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Program
///
/// <summary>
/// CubePDF Lite のメインプログラムを表します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class Program
{
    /* --------------------------------------------------------------------- */
    ///
    /// Main
    ///
    /// <summary>
    /// CubePDF Merge のエントリポイントです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static void Main(string[] src)
    {
        try
        {
            // ログ出力用の処理です。不要な場合、削除して構いません。
            InitLog(src);

            // コマンドライン引数を解析します。
            // オプション引数は -o のみで、出力パスを表します。
            // それ以外の引数は全て入力ファイルのパスとして扱います。
            var args = new ArgumentCollection(src);
            if (args.Count <= 0) throw new ArgumentException("No input file");
            if (!args.Options.ContainsKey("o")) throw new ArgumentException("No output file");

            using var writer = new DocumentWriter();

            foreach (var f in args)
            {
                // PDF、PNG、JPEG、BMP ファイルを結合対象とします。
                // サンプルプログラムでは、拡張子が .pdf のものを PDF ファイル、
                // それ以外のものを画像ファイルとして処理します。
                if (f.ToLower().EndsWith(".pdf")) writer.Add(new DocumentReader(f));
                else writer.Add(new ImagePageCollection(f));
            }

            writer.Save(args.Options["o"]);
        }
        catch (Exception err)
        {
            Logger.Error(err);
            Console.WriteLine(err.ToString());
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InitLog
    ///
    /// <summary>
    /// ログ出力に関する初期設定を実行します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void InitLog(string[] args)
    {
        Logger.Configure(new Cube.Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);
        Logger.Info($"[ {string.Join(" ", args)} ]");
    }
}