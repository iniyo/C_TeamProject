
namespace testdbwinform
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.staffcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.now_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accident_free = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.case_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 20F);
            this.label1.Location = new System.Drawing.Point(26, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "사원명을 입력하세요 : ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(323, 45);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(108, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.staffcode,
            this.now_time,
            this.name,
            this.accident_free,
            this.date,
            this.case_number,
            this.revenue});
            this.dataGridView1.Location = new System.Drawing.Point(31, 101);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(554, 62);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // staffcode
            // 
            this.staffcode.HeaderText = "staffcode";
            this.staffcode.Name = "staffcode";
            this.staffcode.ReadOnly = true;
            // 
            // now_time
            // 
            this.now_time.HeaderText = "현재시간";
            this.now_time.Name = "now_time";
            this.now_time.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "사원명";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // accident_free
            // 
            this.accident_free.HeaderText = "무사고여부";
            this.accident_free.Name = "accident_free";
            // 
            // date
            // 
            this.date.HeaderText = "날짜";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // case_number
            // 
            this.case_number.HeaderText = "배달건수";
            this.case_number.Name = "case_number";
            // 
            // revenue
            // 
            this.revenue.HeaderText = "수수료수익";
            this.revenue.Name = "revenue";
            this.revenue.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(626, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "추가";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 191);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn staffcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn now_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn accident_free;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn case_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenue;
    }
}