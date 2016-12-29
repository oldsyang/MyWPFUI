using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyWPFUI
{
    public static class WPFUIManage
    {
        public static void LoadDLL()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //{
            //    String projectName = Assembly.GetExecutingAssembly().GetName().Name.ToString();
            //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(projectName + ".lib.Microsoft.Expression.Interactions.dll"))
            //    {
            //        Byte[] b = new Byte[stream.Length];
            //        stream.Read(b, 0, b.Length);
            //        return Assembly.Load(b);
            //    }
            //};

        }
    }
}
