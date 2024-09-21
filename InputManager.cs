using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class InputManager
    {
        public InputManager()
        {
            Input = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> Input { get; }

        public void AddKey(string key)
        {
            Input.Add(key, false);
        }

        public void KeyDown(string key)
        {
            Input[key] = true;
        }

        public void KeyUp(string key) 
        {
            Input[key] = false;
        }

    }
}
