using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWPFUI.Controls
{
    public class InteractionRequest
    {
        public event EventHandler Requested;

        public void Request()
        {
            if (Requested != null)
            {
                Requested(this, new EventArgs());
            }
        }
    }

}
