using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // 윈폼
using MySql.Data.MySqlClient; // MySql 사용 시
using System.Collections; //ArrayLIst 클래스 사용 시 

namespace testdbwinform
{
    public partial class Form1 : Form
    {
        
        //데이터 베이스 정보
        static string server = "localhost";
        static string databaes = "mydb";
        static string port = "3308";
        static string user = "root";
        static string password = "!roottestdatabase23";
        
        static string connectionaddress = $"Server={server};Port={port};Database={databaes};Uid={user};Pwd={password}";
        // datagridview 행 생성을 위한 table 객체
        DataTable table = new DataTable();
        // mysql db 연결 시 필요한 것들
        MySqlConnection conn; // MySql db연동을 위해 필요
        MySqlCommand cmd; // 쿼리문 설정, 실행
        public Form1()
        {
            InitializeComponent();
            /*// 임의 난수 생성
            Random randomObj = new Random();
            int randomValue = randomObj.Next();*/
        }
        /////폼 동작
        
        // 폼이 시작되며 DB를 연결 시킨다.
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connectionaddress); // 실행할 정보 셋팅
            conn.Open(); //MySql db 실행
            dataGridView1.ReadOnly = true; // 메인 datagridview읽기만 가능하도록 설정 
            cmd = new MySqlCommand("", conn); // 쿼리문은 넣지 않고 일단 실행 -> 필요한 이벤트 처리기에서 쿼리문 설정.
            connected(); // 연결되었는지 확인
        }
        //
        //폼 동작
        //
        // 폼 동작 끝날때 db연결 해제
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close(); //MySql db 연결 종료.
        }
        // 항목 내의 셀 두번 클릭 시
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //string a = e.ToString();
        }
        //
        // 이벤트 처리기
        //
        private void button1_Click(object sender, EventArgs e) //추가버튼 클릭시 이동 이벤트
        {
            Form2 form2 = new Form2();
            form2.ShowDialog(); // 모달방식
        }
        private void button3_Click(object sender, EventArgs e) //intest
        {
            Main_ListView_items_Reader(); //화면 메인 리스트 뷰에 데이터 받아오기
        }
        private void button4_Click(object sender, EventArgs e) //outtest
        {
            Main_ListView_items_Writer(); //화면 메인 리스트 뷰에 데이터 가져오기
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show(); // 모달방식
        }
        //
        //커스텀 함수
        //
        // 메인 리스트 뷰 데이터 db에 넣기
        private void Main_ListView_items_Writer()
        {

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string Name = dataGridView1.Rows[i].Cells[0].Value.ToString(); //
                string Price = dataGridView1.Rows[i].Cells[1].Value.ToString(); //
                string Count = dataGridView1.Rows[i].Cells[2].Value.ToString(); //
                string Total = dataGridView1.Rows[i].Cells[3].Value.ToString(); //

                //INSERT INTO 쿼리문으로 받아온 정보를 DB에 전송한다. 
                string selectQuery = $"INSERT INTO main_tabel_test (name,price,count,total,c_num) VALUES  ('{Name}',{Price},{Count},{Total},{i})";

                //DB전송을 진행하고 실패시 에러메세지 출력
                try
                {
                    cmd.CommandText = selectQuery; // 쿼리문 지정
                    if (cmd.ExecuteNonQuery() != 1) //행의 수 반환(결과를 받을 필요가 없는 Query문에 많이 사용)
                        MessageBox.Show("데이터 넣기 실패");
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("전송완료");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // 메인 리스트 뷰에 전체 데이터 표시 (데이터 읽어오기)
        private void Main_ListView_items_Reader()
        {
            MySqlDataReader reader; // 서버에서 데이터 가져오기 설정
            dataGridView1.Rows.Clear(); // 데이터 그리드 뷰 초기화
            if (textBox1.Text == "")
            {
                string selectQuery = "SELECT * FROM main_table_test"; // 전체 항목 읽어오기
                // 모든 테이블 데이터를 가져옴.
                cmd.CommandText = selectQuery; // cmd에 쿼리 설정
                reader = cmd.ExecuteReader();
                // 데이터 가져와서 DataGridView에 설정
                try
                {
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(reader["casecode"], reader["table2_staffcode"], reader["accident_free"], reader["case_number"], reader["date"], reader["revenue"], reader["commute"]);
                    }
                    reader.Close(); // 루프 끝나면 읽기동작 종료.
                    MessageBox.Show("무사히 동작 완료");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message); // 에러 메시지 출력
                }
            }
            else
            {
                // 검색창 텍스트 박스에 내용이 입력 되면 해당 텍스트 박스내용에 있는 행을 보여줌. (현재 staff_code로 한정되어있음)
                string selectQuery = "SELECT * FROM main_table_test where table2_staffcode = '" + textBox1.Text + " '";
                cmd.CommandText = selectQuery;
                reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        // 해당되는 항목이 들어있는 행을 전부 가지고 옴.
                        dataGridView1.Rows.Add(reader["casecode"], reader["table2_staffcode"], reader["accident_free"], reader["case_number"], reader["date"], reader["revenue"], reader["commute"]);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message); // 에러 메시지 출력
                }
            }          
        }

       /* //INSERT처리
        public void InsertDB()
        {
            string sql = "Insert Into user  (id,name) values ('1','홍길동')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        //UPDATE처리
        public void UpdateDB()
        {
            string sql = "Update user Set name ='홍길동2' where id = 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

        }

        //DELETE처리
        public void DeleteDB()
        {
            string sql = "Delete From user where id = '1'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        //데이터조회
        public DataSet GetUser()
        {
            string sql = "select * from user";
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            da.Fill(ds);
            return ds;
        }*/

        // 콤보박스 선택
        int year;
        int month;
        int day;

        // 년도
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string yeart = comboBox1.SelectedItem.ToString();
            yeart = yeart.Replace("년", "");
            //MessageBox.Show(yeart);
            year = int.Parse(yeart);    // 년도 값

            comboBox2.Items.Clear();    // 초기화
            for (int i = 1; i <= 12; i++)
            {
                comboBox2.Items.Add(i.ToString() + "월");
            }
            comboBox2.SelectedIndex = 0;
        }
        // 윤년포함 mounth
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string montht = comboBox2.SelectedItem.ToString();
            montht = montht.Replace("월", "");
            //MessageBox.Show(montht);
            month = int.Parse(montht);  // 월 선택 값

            comboBox3.Items.Clear();    // 초기화

            int count;  // 일 개수

            // 31일
            if (month == 1 & month == 3 & month == 5 & month == 7 & month == 8 & month == 10 & month == 12)
            {
                count = 31;
            }
            else if (month == 2)
            {
                // 윤년
                if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                {
                    count = 29;
                }

                else
                {
                    count = 28;
                }
            }
            // 30일
            else
            {
                count = 30;
            }

            for (int i = 1; i <= count; i++)
            {
                comboBox3.Items.Add(i.ToString() + "일");
            }
        }
        // Day
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dayt = comboBox3.SelectedItem.ToString();
            dayt = dayt.Replace("일", "");
            day = int.Parse(dayt);  // 일 선택 값
            MessageBox.Show(day.ToString());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            //textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
        private void connected() //연결 되는지 확인
        {
            if (conn.State == ConnectionState.Open) // 데이터소스가 현재 연결상태인지 확인
            {
                label2.Text = "Connected"; // 연결 완료시
            }
            else
            {
                label2.Text = "DisConnected"; // 연결 불가시
            }
        }

        
    }
}
