using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        int deleveries; // 배달 건수
        int accident; // 무사고 확인
        string datenow; // 현재시간 확인 용도
        string casecode; // 랜덤 난수로 설정.
        int commission; // 수수료 수익
        int commute; // 제시간에 퇴근 했는지 여부 확인
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
        // 폼
        //
        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connectionaddress); // 실행할 정보 셋팅
            conn.Open(); //MySql db 실행
            cmd = new MySqlCommand("", conn); // 쿼리문은 넣지 않고 일단 실행 -> 필요한 이벤트 처리기에서 쿼리문 설정.
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close(); //MySql db 연결 종료.
        }
        //
        // 이벤트 처리기
        //
        //사원 이름 입력 시 이벤트
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // 무사고, 배달건수를 제외한 데이터는 모두 ReadOnly == 값 변경 불가.
            // 엔터키 이벤트
            if (e.KeyCode == Keys.Enter && String.IsNullOrWhiteSpace(textBox1.Text))  //텍스트 박스 텍스트가 없으면 Clear()
            {
                //dataGridView1.Columns.Clear();
                dataGridView1.DataSource = (dataGridView1.DataSource as DataTable).Clone();
                //((DataTable)dataGridView1.DataSource).Rows.Clear(); // 데이터 그리드 뷰의 모든 항목 지움 (컬렴명은 유지)
            }
            else if (e.KeyCode == Keys.Enter) // 텍스트 박스 길이가 3이 넘고 엔터를 쳐야지 실행됨.
            {
                try
                {
                    string selectQuery = "SELECT * FROM sub_table_test where name = '" + textBox1.Text + " '"; // textBox1과 일치하는 값을 가진 테이블 내 행만 가지고 옴. (sub 데이터 베이스의 name 필드)
                    cmd.CommandText = selectQuery; // cmd에 쿼리 설정
                    MySqlDataReader reader; // 서버에서 데이터 가져오기 설정
                    reader = cmd.ExecuteReader();
                    dataGridView1.Rows.Add(reader["staffcode"], reader["name"]); //사원명이 있으면 사원명과 staff_code를 세팅 (사원명이 중복될 경우 데이터 처리도 해야됨)
                    reader.Close(); // 셋팅 끝났으면 종료
                    MessageBox.Show("사원이 인증되었습니다.");
                    set_data(); //기본 데이터 셋팅
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //MessageBox.Show("해당하는 이름이 데이터베이스 내에 존재하지 않습니다.");
                }
            }
            else if(e.KeyCode == Keys.Enter && !String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("텍스트 박스를 확인해주세요");
            }
        }
        //추가버튼 클릭 시 db에 data 전송
        private void button1_Click(object sender, EventArgs e)
        {
            int check=0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if(null == dataGridView1.Rows[i].Cells[i].Value)
                {
                    check = 0;
                }else
                {
                    check = 1;
                }
            }
            if(check == 0)
            {
                MessageBox.Show("셀이 비어있습니다. 값을 넣어주세요");
            }else if(check == 1)
            {
                input_data(); // 데이터 업데이트
            }
        }
        // datafridview keydown 이벤트 시 다음 row로 이동하는 동작이 있음 (그냥 정보)
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // 엔터키 이벤트와 datagridview의 값이 null이 아닌 경우 배달건수 확인
            if (e.KeyCode == Keys.Enter && null != dataGridView1.Rows[1].Cells[5].Value)
            {
                deleveries = int.Parse(dataGridView1.Rows[1].Cells[5].Value.ToString()); // 배달 건수 int로 받음
                // 시간당 배달건수 평균 3~5개인 것을 고려 파트타임 8시간 동안 휴식시간 1시간을 제외한 나머지 7시간 * 5 == 35건수 밥도 안먹고 8시간 동안 뛰었을 때 50개 가량이 가능하지만 일반적인 기준
                // 이부분은 시스템에서 값을 받아와서 저장하는 것이 전제이고 값이 이상하게 찍힌 경우를 대비한 것. => 시스템 확인이 필요.
                if (40 < deleveries)
                {
                    MessageBox.Show("비정상적으로 값이 높습니다. 관리자에게 확인해주세요");
                }
                else
                {
                    commission = deleveries * 400; // 400은 고정 수수료 (건수 * 400 = 총수익)
                    dataGridView1.Rows[1].Cells[5].Value = commission; //datagridview에 셋팅
                }
            }
            // 비어있는 경우
            else if(e.KeyCode == Keys.Enter && null == dataGridView1.Rows[1].Cells[4].Value)
            {
                MessageBox.Show("배달건수가 비어있습니다.");
            }
            // 무사고 확인
            if (e.KeyCode == Keys.Enter && null != dataGridView1.Rows[1].Cells[3].Value)
            {
                if (0 < dataGridView1.Rows[1].Cells[2].Value.ToString().Length)
                {
                    // 입력 문자가 X or O 일때만 입력 가능하도록
                    if ("X" == dataGridView1.Rows[1].Cells[3].Value.ToString() || "O" == dataGridView1.Rows[1].Cells[3].Value.ToString())
                        if(e.KeyCode == Keys.X) // x인 경우
                        {
                            dataGridView1.Rows[1].Cells[3].Value = "무사고";
                            accident = 0;
                        }
                        else
                        {
                            dataGridView1.Rows[1].Cells[3].Value = "사고남";
                            accident = 1;
                        }
                    else
                    {
                        dataGridView1.Rows[1].Cells[3].Value = "";
                        MessageBox.Show("O 나 X 를 입력해주세요.");
                    }
                }
                else
                {
                    dataGridView1.Rows[1].Cells[3].Value = "";
                    MessageBox.Show("오늘의 무사고 여부를 확인해주세요.");
                }
            }
            else if (e.KeyCode == Keys.Enter && null == dataGridView1.Rows[1].Cells[5].Value.ToString())
            {
                MessageBox.Show("배달건수가 비어있습니다.");
            }
        }
        //
        // 커스텀 함수
        //
        private void set_data() //엔터키 누르면 정보 셋팅하는 함수.
        {
            Random randomObj = new Random();
            casecode = "caseNo" + randomObj.Next().ToString(); //next 시 10개의 int가 생성됨. casecode의 크기를 16으로 해놨음. 그래서 영문자 6개 더함.
            DateTime now = DateTime.Now;
            dataGridView1.Rows[1].Cells[4].Value = DateTime.Today; //현재 날짜 셋팅 년,월,일

            dataGridView1.Rows[1].Cells[1].Value = now.Hour + ":" + now.Minute; // 현재시간 셋팅 시간+분
            string DateNOw = dataGridView1.Rows[1].Cells[4].Value.ToString();
            string[] hour_minute = DateNOw.Split(':');
            int hour = int.Parse(hour_minute[0]);
            int minute = int.Parse(hour_minute[1]);
            dataGridView1.Rows[1].Cells[6].Value = commission; //수수료 수익 자동 셋팅
            // 00~8, 8~14, 14~00
            if (hour == 0 || hour == 7 && minute >= 50 || hour == 8 || hour == 9 && minute >= 50 || hour == 13 && minute >= 50 || hour == 14 || hour == 15 && minute >= 50)
            {
                commute = 1; // 1은 tinyint의 true값
            }
            else
            {
                commute = 0;
            }
            
        }
        // 데이터 추가 함수
        private void input_data()
        {
            // casecode, accident_free(bool), case_number, date, revenue, commute(bool)
            string caseNum = dataGridView1.Rows[1].Cells[5].Value.ToString(); // case_number (배달건수)
            datenow = dataGridView1.Rows[1].Cells[3].Value.ToString(); // date (현재날짜)


            //INSERT INTO 쿼리문으로 받아온 정보를 DB에 전송한다. 
            string selectQuery = $"INSERT INTO main_tabel_test (casecode,accident_free,case_number,date,revenue,commute) VALUES  ({casecode},{accident},{caseNum},{datenow},{commission},{commute})";

            //DB전송을 진행하고 실패시 에러메세지 출력
            try
            {
                cmd.CommandText = selectQuery; // 쿼리문 지정
                if (cmd.ExecuteNonQuery() != 1) //행의 수 반환(결과를 받을 필요가 없는 Query문에 많이 사용)
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
