using BetterSharpPick.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSharpPick.Payload
{
    public interface IPayloadPreparer
    {
        string Prepare(Options opts);
    }

    public class PreparedPayload
    {
        public string Text { get; set; }
        public string[] Args { get; set; }
        public PreparedPayload()
        {
            Text = string.Empty;
            Args = new string[0];
        }
    }
}
