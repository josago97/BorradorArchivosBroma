using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    private const int COUNTDOWN_TIME = 1000;
    private const int REFRESH_SCREEN_TIME = 10;

    static void Main()
    {
        DoCountdown();
        DeleteAllFiles();
    }

    static void DoCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            Console.WriteLine(i);
            Thread.Sleep(COUNTDOWN_TIME);
            Console.SetCursorPosition(0, 0);
        }

        Console.WriteLine(":3");
        Thread.Sleep(COUNTDOWN_TIME * 2);
        Console.Clear();
    }

    static void DeleteAllFiles()
    {
        int filesCount = 0;
        long filesCountSize = 0;
        string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        EnumerationOptions enumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false
        };

        foreach (string file in Directory.EnumerateFiles(rootPath, "*.*", enumerationOptions))
        {
            WriteConsole
            (
                $"Borrando {file}",
                $"{filesCount} archivos eliminados, {DisplayByteCount(filesCountSize)} liberados"
            );

            Thread.Sleep(REFRESH_SCREEN_TIME);

            if (DeleteFile(file, out long size))
            {
                filesCountSize += size;
                filesCount++;
            }
        }
    }

    static bool DeleteFile(string path, out long size)
    {
        bool isDeleted = false;
        size = 0;

        try
        {
            using FileStream stream = File.OpenRead(path);
            size = stream.Length;
            isDeleted = true;
        }
        catch { }

        return isDeleted;
    }

    static string DisplayByteCount(long bytes)
    {
        var giga = Math.Pow(2, 30);
        var mega = Math.Pow(2, 20);
        var kilo = Math.Pow(2, 10);

        if (bytes >= giga) return $"{(long)(bytes / giga)} GBytes";
        else if (bytes >= mega) return $"{(long)(bytes / mega)} MBytes";
        else if (bytes >= kilo) return $"{(long)(bytes / kilo)} KBytes";
        else return $"{bytes} Bytes";
    }

    static void WriteConsole(params string[] lines)
    {
        for (int i = Console.CursorTop; i > 0; i--)
        {
            Console.SetCursorPosition(0, i - 1);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(0, 0);
        Array.ForEach(lines, s => Console.WriteLine(s.Substring(0, Math.Min(s.Length, Console.WindowWidth))));
    }
}
