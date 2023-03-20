using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class PrePlayer
    {
        string name;

        public string Name { get => name; set => name = value; }

        public PrePlayer() { }

        public PrePlayer(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
