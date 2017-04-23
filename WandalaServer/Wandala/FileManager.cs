using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wandala
{
    class FileManager
    {
        public void Log(string message)
        {
            File.WriteAllText("Wandala_ErrorLog.txt", message);
        }
    }
}
