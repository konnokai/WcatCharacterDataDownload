using System.Threading;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WCP_Unity3d_Dowmload.Properties;

namespace WCP_Unity3d_Dowmload
{
	public class Form_Main : Form
    {
        #region 物件
        private Stopwatch SW = new Stopwatch();
        private Button btn_Start;
		private TextBox txt_ID;
		private FolderBrowserDialog FBD;
		private TextBox txt_Save;
		private Button btn_Save;
		private Label lab_ID;
		private Label lab_Name;
		private TextBox txt_Name;
		private GroupBox GB;
		private CheckBox chb_full;
		private CheckBox chb_evol;
		private CheckBox chb_Prefabs;
		private CheckBox chb_voice;
		private NumericUpDown NUP;
		private Label label1;
		private GroupBox GB_Ver;
        private RadioButton rdb_SJ;
		private RadioButton rdb_ST;
		private GroupBox GB_FVer;
		private RadioButton RB_I;
        private RadioButton RB_A;
        private RadioButton rdb_SK;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel lab_Status;
		private ToolStripStatusLabel lab_Job;
		private ToolStripStatusLabel lab_Version;
		private CheckBox chb_bust;
		private ToolStripStatusLabel lab_liberation;
		private ToolStripStatusLabel lab_gender;
		private CheckBox chb_icon;
		private OpenFileDialog OFD;
        #endregion

        private string Save_Directory = "";

        public Form_Main()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Environment.ExitCode = 0;
			txt_Save.Text = Program.Save_Path;
			if (Program.ID != "") txt_ID.Text = Program.ID;
            if (Program.Name != "") txt_Name.Text = Program.Name;
            switch (Program.Server)
			{
			case 1:
				rdb_ST.Checked = true;
				break;
			case 2:
				rdb_SJ.Checked = true;
				break;
			case 3:
				rdb_SK.Checked = true;
				break;
			}
			switch (Program.Ver)
			{
			case 1:
				RB_A.Checked = true;
				break;
			case 2:
				RB_I.Checked = true;
				break;
			}
			if (Program.AutoDownload) btn_Start_Click(sender, e);
        }

		private void txt_ID_TextChanged(object sender, EventArgs e)
		{
			lab_Status.Text = "等待開始";
			if (txt_ID.Text.Length != 8)
			{
				lab_gender.Text = "性別:未知";
				lab_Version.Text = "版本:未知";
				lab_Job.Text = "職業:未知";
				lab_liberation.Text = "神解:未知";
				return;
			}
			int num = 0;
			int.TryParse(txt_ID.Text.Substring(0, 1), out num);
			switch (num)
			{
			case 1:
				lab_gender.Text = "性別:男";
				break;
			case 2:
				lab_gender.Text = "性別:女";
				break;
			default:
				lab_gender.Text = "性別:錯誤";
				break;
			}
			int num2 = 0;
			int.TryParse(txt_ID.Text.Substring(1, 2), out num2);
			switch (num2)
			{
			case 1:
				lab_Job.Text = "職業:劍";
				break;
			case 2:
				lab_Job.Text = "職業:拳";
				break;
			case 3:
				lab_Job.Text = "職業:斧";
				break;
			case 4:
				lab_Job.Text = "職業:槍";
				break;
			case 5:
				lab_Job.Text = "職業:弓";
				break;
			case 6:
				lab_Job.Text = "職業:法";
				break;
			case 7:
				lab_Job.Text = "職業:雙";
				break;
			case 8:
				lab_Job.Text = "職業:龍";
				break;
			case 9:
				lab_Job.Text = "職業:變";
				break;
            case 10:
                lab_Job.Text = "職業:大劍";
                break;
            }
			int num3 = 0;
			int.TryParse(txt_ID.Text.Substring(3, 1), out num3);
			switch (num3)
			{
			case 8:
				lab_Version.Text = "版本:台限";
				rdb_ST.Checked = true;
				break;
			case 9:
				lab_Version.Text = "版本:韓限";
				rdb_SK.Checked = true;
				break;
			default:
				lab_Version.Text = "版本:一般";
				break;
			}
			int num4 = 0;
			int.TryParse(txt_ID.Text.Substring(7, 1), out num4);
			switch (num4)
			{
			case 0:
				lab_liberation.Text = "神解:否";
				return;
			case 1:
				lab_liberation.Text = "神解:是";
				return;
			default:
				lab_liberation.Text = "神解:錯誤";
				return;
			}
		}

		private void chb_voice_CheckedChanged(object sender, EventArgs e)
		{
			NUP.Enabled = chb_voice.Checked;
		}

		private void btn_Save_Click(object sender, EventArgs e)
		{
			if (FBD.ShowDialog() == DialogResult.OK) txt_Save.Text = FBD.SelectedPath;
        }

		private void btn_Start_Click(object sender, EventArgs e)
		{
			if (txt_ID.Text.Length != 8 || txt_Save.Text.Length == 0)
			{
				lab_Status.Text = "錯誤";
				return;
			}
			Save_Directory = ((!string.IsNullOrEmpty(txt_Name.Text)) ? txt_Name.Text : txt_ID.Text);
			Download_Event(false);
			if (!Directory.Exists(txt_Save.Text + "\\" + Save_Directory)) Directory.CreateDirectory(txt_Save.Text + "\\" + Save_Directory);
            if (File.Exists(Environment.GetEnvironmentVariable("TEMP") + "\\card.txt")) File.Delete(Environment.GetEnvironmentVariable("TEMP") + "\\card.txt");
            if (chb_icon.Checked) WriterDownloadMessage("Card_0_icon_card_ID_0_png".Replace("ID", txt_ID.Text));
            if (chb_bust.Checked) WriterDownloadMessage("Card_1_bust_card_ID_1_png".Replace("ID", txt_ID.Text));
            if (chb_full.Checked) WriterDownloadMessage("Card_2_full_card_ID_2_png".Replace("ID", txt_ID.Text));
            if (chb_evol.Checked) WriterDownloadMessage("Card_3_evol_card_ID_3_png".Replace("ID", txt_ID.Text));
            if (chb_voice.Checked)
			{
                for (int i = 0; i < NUP.Value; i++) WriterDownloadMessage("Sound_Voice_Player_ID_Value_wav".Replace("ID", txt_ID.Text).Replace("Value", i.ToString("D2")));                
			}
			if (chb_Prefabs.Checked)
			{
				if (lab_Job.Text == "職業:龍" && MessageBox.Show("目前下載的檔案包含龍騎模組\r\n要一併下載嗎?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					WriterDownloadMessage("Character_Prefabs_Player_ply_ID_rider_prefab".Replace("ID", txt_ID.Text));
				}
				else if (lab_Job.Text == "職業:變" && MessageBox.Show("目前下載的檔案包含變身士模組\r\n要一併下載嗎?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					WriterDownloadMessage("Character_Prefabs_Player_ply_ID_m_prefab".Replace("ID", txt_ID.Text));
				}
                else WriterDownloadMessage("Character_Prefabs_Player_ply_ID_prefab".Replace("ID", txt_ID.Text));
            }
            string str = Environment.GetEnvironmentVariable("TEMP") + "\\card.txt \"" + txt_Save.Text + "\\" + Save_Directory + "\"";
			if (rdb_ST.Checked) str += " /tw";
            else if (rdb_SJ.Checked) str += " /jp";
            else if (rdb_SK.Checked) str += " /kr";
            if (RB_A.Checked) str += " /a";
            else if (RB_I.Checked) str += " /i";
            ThreadPool.QueueUserWorkItem(delegate
            {
                Process process = new Process();
                process.StartInfo.FileName = Application.StartupPath + "\\清單下載器.exe";
                process.StartInfo.Arguments = str + " /ac /ad /ncd /dd /ns";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                if (Program.Close) Environment.Exit(1);
                Invoke(new Action(delegate { Download_Event(true); }));
            });
        }

		private void Download_Event(bool Work)
		{
			txt_ID.ReadOnly = !Work;
			txt_Name.ReadOnly = !Work;
			btn_Save.Enabled = Work;
			btn_Start.Enabled = Work;
			GB.Enabled = Work;
			GB_Ver.Enabled = Work;
			GB_FVer.Enabled = Work;
			NUP.Enabled = (Work && chb_voice.Enabled);
		}

		private void WriterDownloadMessage(string Message)
		{
			using (StreamWriter streamWriter = new StreamWriter(Environment.GetEnvironmentVariable("TEMP") + "\\card.txt", true))
			{
				streamWriter.WriteLine(Message);
			}
		}

		private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.SavePath = txt_Save.Text;
			Settings.Default.Save();
		}

		private void InitializeComponent()
		{
            this.btn_Start = new System.Windows.Forms.Button();
            this.txt_ID = new System.Windows.Forms.TextBox();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.txt_Save = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.lab_ID = new System.Windows.Forms.Label();
            this.lab_Name = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.GB = new System.Windows.Forms.GroupBox();
            this.chb_icon = new System.Windows.Forms.CheckBox();
            this.chb_bust = new System.Windows.Forms.CheckBox();
            this.chb_evol = new System.Windows.Forms.CheckBox();
            this.NUP = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chb_Prefabs = new System.Windows.Forms.CheckBox();
            this.chb_voice = new System.Windows.Forms.CheckBox();
            this.chb_full = new System.Windows.Forms.CheckBox();
            this.GB_Ver = new System.Windows.Forms.GroupBox();
            this.rdb_SK = new System.Windows.Forms.RadioButton();
            this.rdb_SJ = new System.Windows.Forms.RadioButton();
            this.rdb_ST = new System.Windows.Forms.RadioButton();
            this.GB_FVer = new System.Windows.Forms.GroupBox();
            this.RB_I = new System.Windows.Forms.RadioButton();
            this.RB_A = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lab_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.lab_gender = new System.Windows.Forms.ToolStripStatusLabel();
            this.lab_Job = new System.Windows.Forms.ToolStripStatusLabel();
            this.lab_Version = new System.Windows.Forms.ToolStripStatusLabel();
            this.lab_liberation = new System.Windows.Forms.ToolStripStatusLabel();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP)).BeginInit();
            this.GB_Ver.SuspendLayout();
            this.GB_FVer.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(223, 278);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "開始";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // txt_ID
            // 
            this.txt_ID.Location = new System.Drawing.Point(12, 52);
            this.txt_ID.MaxLength = 8;
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(139, 22);
            this.txt_ID.TabIndex = 1;
            this.txt_ID.TextChanged += new System.EventHandler(this.txt_ID_TextChanged);
            // 
            // txt_Save
            // 
            this.txt_Save.Location = new System.Drawing.Point(12, 12);
            this.txt_Save.Name = "txt_Save";
            this.txt_Save.ReadOnly = true;
            this.txt_Save.Size = new System.Drawing.Size(258, 22);
            this.txt_Save.TabIndex = 2;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(276, 12);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(22, 23);
            this.btn_Save.TabIndex = 3;
            this.btn_Save.Text = "...";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // lab_ID
            // 
            this.lab_ID.AutoSize = true;
            this.lab_ID.Location = new System.Drawing.Point(10, 37);
            this.lab_ID.Name = "lab_ID";
            this.lab_ID.Size = new System.Drawing.Size(44, 12);
            this.lab_ID.TabIndex = 5;
            this.lab_ID.Text = "角色ID:";
            // 
            // lab_Name
            // 
            this.lab_Name.AutoSize = true;
            this.lab_Name.Location = new System.Drawing.Point(157, 37);
            this.lab_Name.Name = "lab_Name";
            this.lab_Name.Size = new System.Drawing.Size(100, 12);
            this.lab_Name.TabIndex = 6;
            this.lab_Name.Text = "資料夾名稱(選填):";
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(159, 52);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(139, 22);
            this.txt_Name.TabIndex = 7;
            // 
            // GB
            // 
            this.GB.Controls.Add(this.chb_icon);
            this.GB.Controls.Add(this.chb_bust);
            this.GB.Controls.Add(this.chb_evol);
            this.GB.Controls.Add(this.NUP);
            this.GB.Controls.Add(this.label1);
            this.GB.Controls.Add(this.chb_Prefabs);
            this.GB.Controls.Add(this.chb_voice);
            this.GB.Controls.Add(this.chb_full);
            this.GB.Location = new System.Drawing.Point(12, 169);
            this.GB.Name = "GB";
            this.GB.Size = new System.Drawing.Size(286, 106);
            this.GB.TabIndex = 8;
            this.GB.TabStop = false;
            this.GB.Text = "下載";
            // 
            // chb_icon
            // 
            this.chb_icon.AutoSize = true;
            this.chb_icon.Checked = true;
            this.chb_icon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_icon.Location = new System.Drawing.Point(6, 21);
            this.chb_icon.Name = "chb_icon";
            this.chb_icon.Size = new System.Drawing.Size(48, 16);
            this.chb_icon.TabIndex = 11;
            this.chb_icon.Text = "頭像";
            this.chb_icon.UseVisualStyleBackColor = true;
            // 
            // chb_bust
            // 
            this.chb_bust.AutoSize = true;
            this.chb_bust.Checked = true;
            this.chb_bust.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_bust.Location = new System.Drawing.Point(113, 21);
            this.chb_bust.Name = "chb_bust";
            this.chb_bust.Size = new System.Drawing.Size(48, 16);
            this.chb_bust.TabIndex = 10;
            this.chb_bust.Text = "縮圖";
            this.chb_bust.UseVisualStyleBackColor = true;
            // 
            // chb_evol
            // 
            this.chb_evol.AutoSize = true;
            this.chb_evol.Checked = true;
            this.chb_evol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_evol.Location = new System.Drawing.Point(6, 43);
            this.chb_evol.Name = "chb_evol";
            this.chb_evol.Size = new System.Drawing.Size(60, 16);
            this.chb_evol.TabIndex = 3;
            this.chb_evol.Text = "覺醒圖";
            this.chb_evol.UseVisualStyleBackColor = true;
            // 
            // NUP
            // 
            this.NUP.Location = new System.Drawing.Point(6, 77);
            this.NUP.Name = "NUP";
            this.NUP.Size = new System.Drawing.Size(65, 22);
            this.NUP.TabIndex = 9;
            this.NUP.Value = new decimal(new int[] {
            55,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "下載語音，所編號的數值至(最低30，最高100):";
            // 
            // chb_Prefabs
            // 
            this.chb_Prefabs.AutoSize = true;
            this.chb_Prefabs.Location = new System.Drawing.Point(220, 43);
            this.chb_Prefabs.Name = "chb_Prefabs";
            this.chb_Prefabs.Size = new System.Drawing.Size(48, 16);
            this.chb_Prefabs.TabIndex = 2;
            this.chb_Prefabs.Text = "模組";
            this.chb_Prefabs.UseVisualStyleBackColor = true;
            // 
            // chb_voice
            // 
            this.chb_voice.AutoSize = true;
            this.chb_voice.Checked = true;
            this.chb_voice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_voice.Cursor = System.Windows.Forms.Cursors.Default;
            this.chb_voice.Location = new System.Drawing.Point(113, 43);
            this.chb_voice.Name = "chb_voice";
            this.chb_voice.Size = new System.Drawing.Size(48, 16);
            this.chb_voice.TabIndex = 1;
            this.chb_voice.Text = "語音";
            this.chb_voice.UseVisualStyleBackColor = true;
            this.chb_voice.CheckedChanged += new System.EventHandler(this.chb_voice_CheckedChanged);
            // 
            // chb_full
            // 
            this.chb_full.AutoSize = true;
            this.chb_full.Checked = true;
            this.chb_full.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_full.Location = new System.Drawing.Point(220, 21);
            this.chb_full.Name = "chb_full";
            this.chb_full.Size = new System.Drawing.Size(48, 16);
            this.chb_full.TabIndex = 0;
            this.chb_full.Text = "立繪";
            this.chb_full.UseVisualStyleBackColor = true;
            // 
            // GB_Ver
            // 
            this.GB_Ver.Controls.Add(this.rdb_SK);
            this.GB_Ver.Controls.Add(this.rdb_SJ);
            this.GB_Ver.Controls.Add(this.rdb_ST);
            this.GB_Ver.Location = new System.Drawing.Point(12, 122);
            this.GB_Ver.Name = "GB_Ver";
            this.GB_Ver.Size = new System.Drawing.Size(286, 41);
            this.GB_Ver.TabIndex = 10;
            this.GB_Ver.TabStop = false;
            this.GB_Ver.Text = "伺服器版本";
            // 
            // rdb_SK
            // 
            this.rdb_SK.AutoSize = true;
            this.rdb_SK.Location = new System.Drawing.Point(238, 19);
            this.rdb_SK.Name = "rdb_SK";
            this.rdb_SK.Size = new System.Drawing.Size(47, 16);
            this.rdb_SK.TabIndex = 2;
            this.rdb_SK.TabStop = true;
            this.rdb_SK.Text = "韓版";
            this.rdb_SK.UseVisualStyleBackColor = true;
            // 
            // rdb_SJ
            // 
            this.rdb_SJ.AutoSize = true;
            this.rdb_SJ.Location = new System.Drawing.Point(122, 19);
            this.rdb_SJ.Name = "rdb_SJ";
            this.rdb_SJ.Size = new System.Drawing.Size(47, 16);
            this.rdb_SJ.TabIndex = 1;
            this.rdb_SJ.Text = "日版";
            this.rdb_SJ.UseVisualStyleBackColor = true;
            // 
            // rdb_ST
            // 
            this.rdb_ST.AutoSize = true;
            this.rdb_ST.Checked = true;
            this.rdb_ST.Location = new System.Drawing.Point(6, 19);
            this.rdb_ST.Name = "rdb_ST";
            this.rdb_ST.Size = new System.Drawing.Size(47, 16);
            this.rdb_ST.TabIndex = 0;
            this.rdb_ST.TabStop = true;
            this.rdb_ST.Text = "台版";
            this.rdb_ST.UseVisualStyleBackColor = true;
            // 
            // GB_FVer
            // 
            this.GB_FVer.Controls.Add(this.RB_I);
            this.GB_FVer.Controls.Add(this.RB_A);
            this.GB_FVer.Location = new System.Drawing.Point(12, 80);
            this.GB_FVer.Name = "GB_FVer";
            this.GB_FVer.Size = new System.Drawing.Size(286, 41);
            this.GB_FVer.TabIndex = 11;
            this.GB_FVer.TabStop = false;
            this.GB_FVer.Text = "檔案版本";
            // 
            // RB_I
            // 
            this.RB_I.AutoSize = true;
            this.RB_I.Location = new System.Drawing.Point(211, 19);
            this.RB_I.Name = "RB_I";
            this.RB_I.Size = new System.Drawing.Size(39, 16);
            this.RB_I.TabIndex = 1;
            this.RB_I.Text = "I版";
            this.RB_I.UseVisualStyleBackColor = true;
            // 
            // RB_A
            // 
            this.RB_A.AutoSize = true;
            this.RB_A.Checked = true;
            this.RB_A.Location = new System.Drawing.Point(35, 19);
            this.RB_A.Name = "RB_A";
            this.RB_A.Size = new System.Drawing.Size(43, 16);
            this.RB_A.TabIndex = 0;
            this.RB_A.TabStop = true;
            this.RB_A.Text = "A版";
            this.RB_A.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lab_Status,
            this.lab_gender,
            this.lab_Job,
            this.lab_Version,
            this.lab_liberation});
            this.statusStrip1.Location = new System.Drawing.Point(0, 304);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(310, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lab_Status
            // 
            this.lab_Status.Name = "lab_Status";
            this.lab_Status.Size = new System.Drawing.Size(55, 17);
            this.lab_Status.Text = "等待開始";
            // 
            // lab_gender
            // 
            this.lab_gender.Name = "lab_gender";
            this.lab_gender.Size = new System.Drawing.Size(58, 17);
            this.lab_gender.Text = "性別:未知";
            // 
            // lab_Job
            // 
            this.lab_Job.Name = "lab_Job";
            this.lab_Job.Size = new System.Drawing.Size(58, 17);
            this.lab_Job.Text = "職業:未知";
            // 
            // lab_Version
            // 
            this.lab_Version.Name = "lab_Version";
            this.lab_Version.Size = new System.Drawing.Size(58, 17);
            this.lab_Version.Text = "版本:未知";
            // 
            // lab_liberation
            // 
            this.lab_liberation.Name = "lab_liberation";
            this.lab_liberation.Size = new System.Drawing.Size(58, 17);
            this.lab_liberation.Text = "神解:未知";
            // 
            // OFD
            // 
            this.OFD.DefaultExt = "exe";
            this.OFD.Filter = "執行檔|*.exe";
            this.OFD.Title = "請選擇清單下載器";
            // 
            // Form_Main
            // 
            this.AcceptButton = this.btn_Start;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 326);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GB_FVer);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.GB_Ver);
            this.Controls.Add(this.GB);
            this.Controls.Add(this.txt_Name);
            this.Controls.Add(this.lab_Name);
            this.Controls.Add(this.lab_ID);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_Save);
            this.Controls.Add(this.txt_ID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.Text = "白貓素材下載清單製作工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GB.ResumeLayout(false);
            this.GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP)).EndInit();
            this.GB_Ver.ResumeLayout(false);
            this.GB_Ver.PerformLayout();
            this.GB_FVer.ResumeLayout(false);
            this.GB_FVer.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
