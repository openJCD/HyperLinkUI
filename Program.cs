using System;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            using var game = new HyperLinkUI.Game1();
            game.Run();
        }
        catch (Exception ex)
        {
            var cdf = File.Create(@"..\hyperlink_crashdump.txt");
            StreamWriter s = new StreamWriter(cdf);
            s.Write(ex.Message + "\n" + ex.StackTrace);
            s.Flush();
        }
    }
}