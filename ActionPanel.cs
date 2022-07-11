using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.ActionPanelPlugin
{
    [PreventThemingAttribute]
    public partial class ActionPanel : Form
    {
        public ActionPanel()
        {
            InitializeComponent();
            //Do not use themes, I will color my controls 
            //Utilities.ThemeManager.ApplyThemeTo(this);
        }

        private void hideParentPanel()
        {
            ActionPanelPlugin pSelf = null;
            foreach (Plugin.Plugin p in Plugin.PluginLoader.Plugins)
            {
                if (p.Name == "ActionPanelPlugin") pSelf = p as ActionPanelPlugin;
            }
            if (pSelf != null)
            {
                pSelf.hidePanel();
            }
        }

        public void setSafetyStatus(bool status)
        {
            if (status)
            {
                btnSafety.Image = Properties.Resources.small_red_button;
            }
            else
            {
                btnSafety.Image = Properties.Resources.small_grey_button;

            }
        }


        public void setHours(int minutes)
        {
            this.label1.Text = (minutes / 60).ToString();
        }

        private void btnSafety_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo) == (int)DialogResult.Yes)
                MainV2.comPort.setMode(
                    new MAVLink.mavlink_set_mode_t() { custom_mode = (MainV2.comPort.MAV.cs.sensors_enabled.motor_control == true && MainV2.comPort.MAV.cs.sensors_enabled.seen) ? 1u : 0u },
                    MAVLink.MAV_MODE_FLAG.SAFETY_ARMED);
            //hideParentPanel();
        }

        private void toggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ActionPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
