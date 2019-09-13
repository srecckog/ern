namespace ern
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.pnl_Login = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lbl_Lozinka = new System.Windows.Forms.Label();
            this.lbl_loginUser = new System.Windows.Forms.Label();
            this.pnl_Login.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_Login
            // 
            this.pnl_Login.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Login.Controls.Add(this.pictureBox2);
            this.pnl_Login.Controls.Add(this.btn_login);
            this.pnl_Login.Controls.Add(this.textBox3);
            this.pnl_Login.Controls.Add(this.textBox2);
            this.pnl_Login.Controls.Add(this.lbl_Lozinka);
            this.pnl_Login.Controls.Add(this.lbl_loginUser);
            resources.ApplyResources(this.pnl_Login, "pnl_Login");
            this.pnl_Login.Name = "pnl_Login";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ern.Properties.Resources.login;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // btn_login
            // 
            resources.ApplyResources(this.btn_login, "btn_login");
            this.btn_login.Name = "btn_login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // lbl_Lozinka
            // 
            resources.ApplyResources(this.lbl_Lozinka, "lbl_Lozinka");
            this.lbl_Lozinka.Name = "lbl_Lozinka";
            // 
            // lbl_loginUser
            // 
            resources.ApplyResources(this.lbl_loginUser, "lbl_loginUser");
            this.lbl_loginUser.Name = "lbl_loginUser";
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_Login);
            this.Name = "LoginForm";
            this.pnl_Login.ResumeLayout(false);
            this.pnl_Login.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Login;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lbl_Lozinka;
        private System.Windows.Forms.Label lbl_loginUser;
    }
}