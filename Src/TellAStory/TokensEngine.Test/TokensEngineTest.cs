using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TokensEngine;

namespace TokensEngine.Test
{
    public partial class TokensEngineTest : Form
    {
        public TokensEngineTest()
        {
            InitializeComponent();
        }

        private void buttonGenerateUniqueToken_Click(object sender, EventArgs e)
        {
            ITokensEngine tokensEngine = TokenEngineAccessor.Get();
            string ust =  tokensEngine.CreateUniquSessionToken();
            MessageBox.Show(String.Format("Unique session token was created. It's value is '{0}'",ust)); 
        }

        private void buttonBindSessionTokenToFacebookToken_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            FlowLayoutPanel flp = new FlowLayoutPanel();
            f.Controls.Add(flp);

            Label lblSessionToken = new Label();
            lblSessionToken.Text = "Session Token:";
            flp.Controls.Add(lblSessionToken);

            TextBox txtBoxSessionToken = new TextBox();
            flp.Controls.Add(txtBoxSessionToken);

            Label lblFacebookToken = new Label();
            lblFacebookToken.Text = "Facebook Token:";
            flp.Controls.Add(lblFacebookToken);

            TextBox txtBoxFacebookSessionToken = new TextBox();
            flp.Controls.Add(txtBoxFacebookSessionToken);

            Button btnBind = new Button();
            btnBind.Text = "Bind";
            btnBind.Click += new EventHandler((x,y)=>{
                    ITokensEngine tokensEngine = TokenEngineAccessor.Get();
                    String sessionToken = txtBoxSessionToken.Text;
                    String facebookSessionToken = txtBoxFacebookSessionToken.Text;
                    tokensEngine.BindSessionTokenToFacebookToken(sessionToken, facebookSessionToken);
                    MessageBox.Show(String.Format("Tokens were binded. {0} was binded to {1}", sessionToken, facebookSessionToken)); 
                });
            flp.Controls.Add(btnBind);

            f.Show();
        }

    }
}
