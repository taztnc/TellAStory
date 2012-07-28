namespace TokensEngine.Test
{
    partial class TokensEngineTest
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
            this.buttonGenerateUniqueToken = new System.Windows.Forms.Button();
            this.buttonBindSessionTokenToFacebookToken = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGenerateUniqueToken
            // 
            this.buttonGenerateUniqueToken.Location = new System.Drawing.Point(12, 12);
            this.buttonGenerateUniqueToken.Name = "buttonGenerateUniqueToken";
            this.buttonGenerateUniqueToken.Size = new System.Drawing.Size(247, 23);
            this.buttonGenerateUniqueToken.TabIndex = 0;
            this.buttonGenerateUniqueToken.Text = "GenerateUniqueToken";
            this.buttonGenerateUniqueToken.UseVisualStyleBackColor = true;
            this.buttonGenerateUniqueToken.Click += new System.EventHandler(this.buttonGenerateUniqueToken_Click);
            // 
            // buttonBindSessionTokenToFacebookToken
            // 
            this.buttonBindSessionTokenToFacebookToken.Location = new System.Drawing.Point(12, 41);
            this.buttonBindSessionTokenToFacebookToken.Name = "buttonBindSessionTokenToFacebookToken";
            this.buttonBindSessionTokenToFacebookToken.Size = new System.Drawing.Size(247, 23);
            this.buttonBindSessionTokenToFacebookToken.TabIndex = 1;
            this.buttonBindSessionTokenToFacebookToken.Text = "Bind Session Token To Facebook Token";
            this.buttonBindSessionTokenToFacebookToken.UseVisualStyleBackColor = true;
            this.buttonBindSessionTokenToFacebookToken.Click += new System.EventHandler(this.buttonBindSessionTokenToFacebookToken_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Resolve Facebook Token";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TokensEngineTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 112);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonBindSessionTokenToFacebookToken);
            this.Controls.Add(this.buttonGenerateUniqueToken);
            this.Name = "TokensEngineTest";
            this.Text = "Tokens Engine Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerateUniqueToken;
        private System.Windows.Forms.Button buttonBindSessionTokenToFacebookToken;
        private System.Windows.Forms.Button button1;
    }
}

