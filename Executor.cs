using System;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace BetterSharpPick.Scripting
{
    public static class Executor
    {
        public static string Execute(string script)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));

            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = runspace;
                    ps.AddScript("$ErrorActionPreference = 'Continue';", useLocalScope: true);
                    ps.AddScript(script, useLocalScope: true);

                    Collection<PSObject> results = ps.Invoke();

                    if (ps.Streams.Error != null && ps.Streams.Error.Count > 0)
                    {
                        var err = string.Join(Environment.NewLine, ps.Streams.Error.Select(e => e.ToString()));
                        throw new InvalidOperationException(err);
                    }

                    return string.Join(Environment.NewLine, results.Select(r => r?.ToString())).TrimEnd();
                }
            }
        }
    }
}

