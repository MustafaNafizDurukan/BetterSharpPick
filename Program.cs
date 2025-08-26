using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Text;
using BetterSharpPick.Parsing;
using BetterSharpPick.Payload;
using BetterSharpPick.Util;
using BetterSharpPick.Scripting;

namespace BetterSharpPick
{
    internal static class Program
    {

        private static int Main(string[] args)
        {
            try
            {
                var parser = new ArgParser();
                var opts = parser.Parse(args);

                if (string.IsNullOrWhiteSpace(opts.PathOrUrl) &&
                    string.IsNullOrWhiteSpace(opts.Command))
                {
                    HelpText.Print();
                    return 0;
                }

                IPayloadPreparer preparer = new ConsolePayloadPreparer();
                string payload = preparer.Prepare(opts);

                Executor.Execute(payload);

                return 0;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine("[!] Error: " + ex.Message);
                Console.Error.WriteLine();
                HelpText.Print();
                return 1;
            }
        }
    }
}


