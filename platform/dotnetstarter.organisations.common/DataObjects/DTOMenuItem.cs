using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects
{
    public class DTOMenuItem
    {
        public string heading { get; set; }
        public string routerLink { get; set; }
        public string icon { get; set; }
        public List<DTOMenuItem> children { get; set; }

        public DTOMenuItem(string heading = "", string routerLink = "", string icon = "")
        {
            children = new List<DTOMenuItem>();
            this.heading = heading;
            this.routerLink = routerLink;
            this.icon = icon;
        }
    }
}
