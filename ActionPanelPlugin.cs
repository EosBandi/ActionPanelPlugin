using System;
using MissionPlanner.Utilities;
using MissionPlanner.Maps;
using System.IO;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace MissionPlanner.ActionPanelPlugin
{
    public class ActionPanelPlugin : MissionPlanner.Plugin.Plugin
    {
        SplitContainer FDRightSide;
        Panel ap;
        ActionPanel apf;
        bool panelStatus = false;

        public override string Name
        {
            get { return "ActionPanelPlugin"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override string Author
        {
            get { return "Andras Schaffer"; }
        }

        //[DebuggerHidden]
        public override bool Init()
        {

            loopratehz = 1f;

            apf = new ActionPanel();
            ap = new Panel();

            //Capture keyboard presses from Main Instance
            MainV2.instance.ProcessCmdKeyCallback += this.Instance_ProcessCmdKeyCallback;

            MainV2.instance.BeginInvoke((MethodInvoker)(() =>
            {

                FDRightSide = Host.MainForm.FlightData.Controls.Find("splitContainer1", true).FirstOrDefault() as SplitContainer;

                //Since It is temprary, it can be as tall as the panel and set to right.
                ap.Size = new Size(300, FDRightSide.Panel2.Height);
                ap.Location = new Point(FDRightSide.Panel2.Width-300,0);
                ap.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                ap.Visible = true;
                Host.FDGMapControl.Parent.Controls.Add(ap);
                ap.BringToFront();


                apf.TopLevel = false;
                apf.FormBorderStyle = FormBorderStyle.None;
                apf.Dock = DockStyle.Fill;
                apf.Visible = true;
                ap.Controls.Add(apf);


                //men.Click += enable_Click;
                //Host.FDMenuMap.Items.Add(men);
                //weatherImageDate = new Label();
                //weatherImageDate.Name = "lbl_alt";
                //weatherImageDate.Location = new System.Drawing.Point(0, 100);
                //weatherImageDate.Text = "";
                //weatherImageDate.AutoSize = true;
                //weatherImageDate.Font = new Font("Tahoma", 15, FontStyle.Bold);
                //weatherImageDate.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                //weatherImageDate.ForeColor = System.Drawing.Color.DarkCyan;

                //FDRightSide.Panel2.Controls.Add(weatherImageDate);
                //FDRightSide.Panel2.Controls.SetChildIndex(weatherImageDate, 1);

            }));


            //Maps.Propagation.ele_run = true;

            return true;
        }


        public override bool Loaded()
        {
            setPanelVisibility(panelStatus);
            Host.FPGMapControl.OnPolygonClick += FDGMapControl_OnPolygonClick;
            return true;
        }

        private void FDGMapControl_OnPolygonClick(GMapPolygon item, object mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        public override bool Loop()
        {
            if (Host.cs.connected)
            {
                MainV2.instance.BeginInvoke((MethodInvoker)(() =>
                {
                    apf.setHours(100);
                    apf.setSafetyStatus(Host.cs.sensors_enabled.motor_control);
                }));
            }

            return true;
        }

        public override bool Exit()
        {

            return true;
        }


        public void hidePanel()
        {
            panelStatus = false;
            setPanelVisibility(panelStatus);
        }

        private void setPanelVisibility(bool visible)
        {
            MainV2.instance.BeginInvoke((MethodInvoker)(() =>
            {

                if (visible)
                {
                    ap.BringToFront();
                }
                else
                {
                    ap.SendToBack();
                }
                ap.Invalidate();
            }));
        }





        private bool Instance_ProcessCmdKeyCallback(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            //Add our shortcut

            if (keyData == (Keys.Alt | Keys.A))
            {
                panelStatus = !panelStatus;
                setPanelVisibility(panelStatus);
                return true;
            }
            return false;
        }

    }

}