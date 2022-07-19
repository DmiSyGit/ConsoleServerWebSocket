namespace photoWindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.getProducts = new System.Windows.Forms.Button();
            this.statusBtn = new System.Windows.Forms.Button();
            this.sendBtn = new System.Windows.Forms.Button();
            this.nameProductTb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.descriptionProductTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CategoryCb = new System.Windows.Forms.ComboBox();
            this.CategoryLbl = new System.Windows.Forms.Label();
            this.sizesTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addressMagazinTb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.priceTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.idPhotoMainTb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.idPhotoSecondTb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.idPhotoThirdTb = new System.Windows.Forms.TextBox();
            this.testBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 57);
            this.button1.TabIndex = 1;
            this.button1.Text = "OpenConWithBaseData";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(356, 216);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Read";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(541, 446);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(230, 68);
            this.button3.TabIndex = 2;
            this.button3.Text = "Занести фото";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(328, 426);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(367, 100);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "10";
            // 
            // getProducts
            // 
            this.getProducts.Location = new System.Drawing.Point(541, 370);
            this.getProducts.Name = "getProducts";
            this.getProducts.Size = new System.Drawing.Size(230, 68);
            this.getProducts.TabIndex = 2;
            this.getProducts.Text = "Занести продукт";
            this.getProducts.UseVisualStyleBackColor = true;
            this.getProducts.Click += new System.EventHandler(this.getProducts_Click);
            // 
            // statusBtn
            // 
            this.statusBtn.Location = new System.Drawing.Point(356, 150);
            this.statusBtn.Name = "statusBtn";
            this.statusBtn.Size = new System.Drawing.Size(155, 30);
            this.statusBtn.TabIndex = 2;
            this.statusBtn.Text = "status";
            this.statusBtn.UseVisualStyleBackColor = true;
            this.statusBtn.Click += new System.EventHandler(this.statusBtn_Click);
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(356, 187);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(155, 23);
            this.sendBtn.TabIndex = 7;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // nameProductTb
            // 
            this.nameProductTb.Location = new System.Drawing.Point(782, 13);
            this.nameProductTb.Name = "nameProductTb";
            this.nameProductTb.Size = new System.Drawing.Size(266, 23);
            this.nameProductTb.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(620, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 28);
            this.label1.TabIndex = 9;
            this.label1.Text = "Наименование";
            // 
            // descriptionProductTb
            // 
            this.descriptionProductTb.Location = new System.Drawing.Point(782, 50);
            this.descriptionProductTb.Name = "descriptionProductTb";
            this.descriptionProductTb.Size = new System.Drawing.Size(266, 23);
            this.descriptionProductTb.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(620, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "Описание";
            // 
            // CategoryCb
            // 
            this.CategoryCb.FormattingEnabled = true;
            this.CategoryCb.Location = new System.Drawing.Point(782, 160);
            this.CategoryCb.Name = "CategoryCb";
            this.CategoryCb.Size = new System.Drawing.Size(266, 23);
            this.CategoryCb.TabIndex = 10;
            this.CategoryCb.SelectedIndexChanged += new System.EventHandler(this.CategoryCb_SelectedIndexChanged);
            // 
            // CategoryLbl
            // 
            this.CategoryLbl.AutoSize = true;
            this.CategoryLbl.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CategoryLbl.Location = new System.Drawing.Point(629, 155);
            this.CategoryLbl.Name = "CategoryLbl";
            this.CategoryLbl.Size = new System.Drawing.Size(106, 28);
            this.CategoryLbl.TabIndex = 9;
            this.CategoryLbl.Text = "Категория";
            // 
            // sizesTb
            // 
            this.sizesTb.Location = new System.Drawing.Point(782, 90);
            this.sizesTb.Name = "sizesTb";
            this.sizesTb.Size = new System.Drawing.Size(266, 23);
            this.sizesTb.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(629, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 28);
            this.label3.TabIndex = 9;
            this.label3.Text = "Размеры";
            // 
            // addressMagazinTb
            // 
            this.addressMagazinTb.Location = new System.Drawing.Point(782, 312);
            this.addressMagazinTb.Name = "addressMagazinTb";
            this.addressMagazinTb.Size = new System.Drawing.Size(266, 23);
            this.addressMagazinTb.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(629, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 28);
            this.label4.TabIndex = 9;
            this.label4.Text = "Адрес";
            // 
            // priceTb
            // 
            this.priceTb.Location = new System.Drawing.Point(782, 127);
            this.priceTb.Name = "priceTb";
            this.priceTb.Size = new System.Drawing.Size(266, 23);
            this.priceTb.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(629, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 28);
            this.label5.TabIndex = 9;
            this.label5.Text = "Цена";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(629, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 28);
            this.label6.TabIndex = 9;
            this.label6.Text = "Гл.Фото";
            // 
            // idPhotoMainTb
            // 
            this.idPhotoMainTb.Location = new System.Drawing.Point(782, 189);
            this.idPhotoMainTb.Name = "idPhotoMainTb";
            this.idPhotoMainTb.Size = new System.Drawing.Size(46, 23);
            this.idPhotoMainTb.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(629, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 28);
            this.label7.TabIndex = 9;
            this.label7.Text = "Воторое Фото";
            // 
            // idPhotoSecondTb
            // 
            this.idPhotoSecondTb.Location = new System.Drawing.Point(782, 221);
            this.idPhotoSecondTb.Name = "idPhotoSecondTb";
            this.idPhotoSecondTb.Size = new System.Drawing.Size(46, 23);
            this.idPhotoSecondTb.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(629, 245);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 28);
            this.label8.TabIndex = 9;
            this.label8.Text = "Третье Фото";
            // 
            // idPhotoThirdTb
            // 
            this.idPhotoThirdTb.Location = new System.Drawing.Point(782, 250);
            this.idPhotoThirdTb.Name = "idPhotoThirdTb";
            this.idPhotoThirdTb.Size = new System.Drawing.Size(46, 23);
            this.idPhotoThirdTb.TabIndex = 8;
            // 
            // testBtn
            // 
            this.testBtn.Location = new System.Drawing.Point(160, 460);
            this.testBtn.Name = "testBtn";
            this.testBtn.Size = new System.Drawing.Size(75, 23);
            this.testBtn.TabIndex = 11;
            this.testBtn.Text = "test";
            this.testBtn.UseVisualStyleBackColor = true;
            this.testBtn.Click += new System.EventHandler(this.testBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 526);
            this.Controls.Add(this.testBtn);
            this.Controls.Add(this.idPhotoThirdTb);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.idPhotoSecondTb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.idPhotoMainTb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.priceTb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.addressMagazinTb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sizesTb);
            this.Controls.Add(this.CategoryLbl);
            this.Controls.Add(this.CategoryCb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.descriptionProductTb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameProductTb);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.statusBtn);
            this.Controls.Add(this.getProducts);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "10";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button getProducts;
        private System.Windows.Forms.Button statusBtn;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.TextBox nameProductTb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox descriptionProductTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CategoryCb;
        private System.Windows.Forms.Label CategoryLbl;
        private System.Windows.Forms.TextBox sizesTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox addressMagazinTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox priceTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox idPhotoMainTb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox idPhotoSecondTb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox idPhotoThirdTb;
        private System.Windows.Forms.Button testBtn;
    }
}

