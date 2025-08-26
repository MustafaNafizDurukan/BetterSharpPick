using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSharpPick.Services;

namespace BetterSharpPick.Parsing
{
    public class ArgParser
    {
        public Options Parse(string[] args)
        {
            if (args == null) args = new string[0];

            var decoder = new DecodeService();

            bool useXor = false;
            string xorKey = string.Empty;
            byte xorKeyByte = 0;

            string pathOrUrl = string.Empty;
            string command = string.Empty;
            var commandArgs = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string token = args[i];

                switch (token.ToLowerInvariant())
                {
                    case "-b64":
                    case "--b64":
                        decoder.UseBase64 = true;
                        break;

                    case "-xor":
                    case "--xor":
                        EnsureHasValue(args, i, "-xor/--xor");
                        useXor = true;
                        i++;

                        var rawKey = decoder.Decode(args[i]).Trim();

                        int keyVal;
                        if (!int.TryParse(rawKey, NumberStyles.Integer, CultureInfo.InvariantCulture, out keyVal))
                            throw new ArgumentException("XOR key must be a single decimal integer (0..255).");

                        if (keyVal < 0 || keyVal > 255)
                            throw new ArgumentOutOfRangeException("-xor", "XOR key out of range (0..255).");

                        xorKey = keyVal.ToString(CultureInfo.InvariantCulture);
                        xorKeyByte = (byte)keyVal;
                        break;

                    case "-path":
                    case "--path":
                        EnsureHasValue(args, i, "-path/--path");
                        i++;
                        pathOrUrl = decoder.Decode(args[i]);
                        break;

                    case "-c":
                    case "--command":
                        EnsureHasValue(args, i, "-c/--command");
                        i++;
                        command = decoder.Decode(args[i]);
                        break;

                    case "-arg":
                    case "--arg":
                        EnsureHasValue(args, i, "-arg/--arg");
                        i++;
                        commandArgs.Add(decoder.Decode(args[i]));
                        break;

                    default:
                        break;
                }
            }

            if (useXor && string.IsNullOrEmpty(xorKey))
                throw new ArgumentException("XOR key has not been specified. Use '-xor <key>' (In b64 mode, key may be encoded with base64).");

            var opts = new Options();
            opts.UseBase64 = decoder.UseBase64;
            opts.UseXor = useXor;
            opts.XorKey = xorKeyByte;
            opts.PathOrUrl = pathOrUrl;
            opts.Command = command;
            opts.CommandArgs = commandArgs.ToArray();
            return opts;
        }

        private static void EnsureHasValue(string[] args, int currentIndex, string flagName)
        {
            bool missing = currentIndex + 1 >= args.Length ||
                           args[currentIndex + 1].StartsWith("-", StringComparison.Ordinal);
            if (missing)
                throw new ArgumentException("Value exptected for: '" + flagName + "'");
        }
    }
}