using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWPFUI.Controls
{
    public class InteractionRequestTrigger : System.Windows.Interactivity.EventTrigger
    {
        protected override string GetEventName()
        {
            return "Requested";
        }
    }
}
