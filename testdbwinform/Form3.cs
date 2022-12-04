﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySql 사용 시

namespace testdbwinform
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        //데이터 베이스 정보
        static string server = "localhost"; // local
        static string databaes = "mydb"; // db이름
        static string port = "3308"; // port 3308 사용
        static string user = "root"; // 사용자
        static string password = "!roottestdatabase23"; // password
        // conn에 들어갈 정보
        static string connectionaddress = $"Server={server};Port={port};Database={databaes};Uid={user};Pwd={password}";
        // MySql db연동을 위해 필요
        MySqlConnection conn;
        // 쿼리문 설정, 실행
        MySqlCommand cmd;
        //
        //폼
        //
        private void Form3_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connectionaddress); // 실행할 정보 셋팅
            conn.Open(); //MySql db 실행
            cmd = new MySqlCommand("", conn); // 쿼리문은 넣지 않고 일단 실행 -> 필요한 이벤트 처리기에서 쿼리문 설정.
        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close(); //MySql db 종료
        }
        //
        //이벤트 처리기
        //
        private void button1_Click(object sender, EventArgs e) // 사원 추가하기 버튼
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text) && !String.IsNullOrWhiteSpace(textBox2.Text) && !String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    if (2 < textBox1.Text.Length && 10 < textBox2.Text.Length && 5 < textBox3.Text.Length)
                    {
                        input_data(); // 데이터 입력
                    }
                    else
                    {
                        MessageBox.Show("입력을 확인해주세요");
                    }
                }
                else if (String.IsNullOrWhiteSpace(textBox1.Text))
                    MessageBox.Show("이름을 입력해주세요");
                else if (3 > textBox1.Text.Length)
                    MessageBox.Show("이름을 확인해주세요");
                else if (String.IsNullOrWhiteSpace(textBox2.Text))
                    MessageBox.Show("전화번호를 입력해주세요");
                else if (10 > textBox2.Text.Length)
                    MessageBox.Show("전화번호를 확인해주세요 '-' 제외");
                else if (String.IsNullOrWhiteSpace(textBox3.Text))
                    MessageBox.Show("주소를 입력해주세요");
                else if (7 > textBox3.Text.Length)
                    MessageBox.Show("주소를 확인해주세요");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //
        //커스텀 함수
        //
        private void input_data()
        {
            string name = textBox1.Text; // 사원 이름
            string tel = textBox2.Text; // 전화번호
            string addr = textBox3.Text.Trim(); // 주소
            //INSERT INTO 쿼리문으로 받아온 정보를 DB에 전송한다. 
            string selectQuery = $"INSERT INTO sub_table_test(name,tel,addr) VALUES  ('{name}',{tel},'{addr}')";

            //DB전송을 진행하고 실패시 에러메세지 출력
            try
            {
                cmd.CommandText = selectQuery; // 쿼리문 지정
                if (cmd.ExecuteNonQuery() != 1) // 행의 수 반환(결과를 받을 필요가 없는 Query문에 많이 사용)
                    MessageBox.Show("데이터 넣기 실패");
                else
                {
                    MessageBox.Show("전송완료");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
