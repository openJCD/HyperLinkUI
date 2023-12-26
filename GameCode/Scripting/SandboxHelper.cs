using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.GameCode.Scripting
{
    static class SandboxHelper
    {
        public static void ConfigureSandboxEnv(Lua env)
        {
            env.DoString(@"import ('HyperLinkUI', 'HyperLinkUI.Scripting.API') ");
            env.DoString(@"
                function cfg_sb()
                    import = function () end
                    dofile = nil
                    loadfile = nil
                    load = nil
                    coroutine = nil
                    os.date = nil
                    os.execute = nil
                    os.exit = nil
                    os.getenv = nil
                    os.remove = nil
                    os.rename = nil
                    os.setlocale = nil
                    newproxy = nil
                    debug = nil
                end
                cfg_sb()");
        }
    }
}
