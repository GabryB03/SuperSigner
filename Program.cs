using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class Program
{
    public static void Main()
    {
        Console.Title = "SuperSigner | Made by https://github.com/GabryB03/";

        if (!Directory.Exists("inputs"))
        {
            Directory.CreateDirectory("inputs");
            return;
        }

        string option = "";

        while (option == "")
        {
            Console.Write("Please, choose what certificate to use (from 1 to 22): ");
            byte parsed = 0;
            
            if (!byte.TryParse(Console.ReadLine(), out parsed))
            {
                Console.WriteLine("Invalid number, please try again.");
                option = "";
                continue;
            }

            if (!(parsed >= 1 && parsed <= 22))
            {
                Console.WriteLine("Invalid number, please try again.");
                option = "";
                continue;
            }

            option = parsed.ToString();
        }

        foreach (string file in Directory.GetFiles("inputs"))
        {
            new Thread(() => SignFile(file, option)).Start();
        }
    }

    public static void SignFile(string file, string option)
    {
        try
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WorkingDirectory = Path.GetFullPath("bin");
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"signtool.exe sign /fd SHA512 /v /ac {option}.crt /f {option}.pfx /p {File.ReadAllText("bin\\" + option + ".txt")} /t \"http://timestamp.digicert.com\" \"{Path.GetFullPath(file)}\"");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
        catch
        {

        }
    }
}