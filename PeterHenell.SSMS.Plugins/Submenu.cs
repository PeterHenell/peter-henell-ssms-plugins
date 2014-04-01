﻿using RedGate.SIPFrameworkShared;

namespace PeterHenell.SSMS.Plugins
{
    class Submenu : SubmenuSimpleOeMenuItemBase
    {
        public Submenu(SimpleOeMenuItemBase[] subMenus)
            : base(subMenus)
        {
        }

        public override string ItemText
        {
            get { return "Peter Henell SSMS Plugins"; }
        }

        public override bool AppliesTo(ObjectExplorerNodeDescriptorBase oeNode)
        {
            return GetApplicableChildren(oeNode).Length > 0;
        }
    }
}