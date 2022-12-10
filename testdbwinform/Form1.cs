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
        int rowselect; //datagridview 선택
        //데이터 베이스 정보
        static string server = "localhost";
        static string databaes = "mydb";
        //static string port = "3308";
        //static string user = "root";
        //static string password = "!roottestdatabase23";
        // 공통 db 설정
        static string port = "3306";
        static string user = "test";
        static string password = "1234";

        static string connectionaddress = $"Server={server};Port={port};Database={databaes};Uid={user};Pwd={password}";
        // datagridview 행 생성을 위한 table 객체
        DataTable table = new DataTable();
        // mysql db 연결 시 필요한 것들
        MySqlConnection conn; // MySql db연동을 위해 필요
        MySqlCommand cmd; // 쿼리문 설정, 실행
        MySqlDataReader mainreader; // 서버에서 데이터 가져오기 설정
        MySqlDataReader subreader; // 서버에서 데이터 가져오기 설정
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
            Main_ListView_items_Reader(DateTime.Now.ToString("yyyy-MM-dd")); //화면 메인 리스트 뷰에 데이터 받아오기
            comboBox1.SelectedIndex=0;
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

        //
        // 이벤트 처리기
        //
        // 검색 텍스트 박스 keydown 이벤트
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if(String.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("값을 입력해주십시요", "검색 실패");
                    }
                    else
                    {
                    Search(textBox1.Text); //화면 메인 리스트 뷰에 데이터 받아오기
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e) //추가버튼 클릭시 이동 이벤트
        {
            Form2 form2 = new Form2();
            form2.ShowDialog(); // 모달방식
        }
        String delcode;
        private void button2_Click(object sender, EventArgs e) //삭제버튼 클릭시 이벤트
        {
            try
            {
                // 행 선택 안됐을 경우
                if(delcode == null) {
                    MessageBox.Show("삭제를 원하는 행을 선택해주십시요", "삭제실패");
                }
                
                else 
                {
                    if (MessageBox.Show("정말 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dataGridView1.Rows.Remove(dataGridView1.Rows[rowselect]); // 해당되는 row 삭제
                                                                                  //dataGridView2.Rows.Remove(dataGridView1.Rows[0]);// row 하나밖에 없으므로 0번째 행 삭제
                        DeleteDB(delcode); // 삭제 진행
                        Main_ListView_items_Reader(dateTimePicker1.Value.ToString("yyyy-MM-dd")); // 삭제 후 테이블 띄우기
                        dataGridView2.Rows.Clear();
                    }
                }
                

            }
            catch
            {
                MessageBox.Show("삭제를 원하는 행을 재선택 해주십시요", "삭제실패");
            }
            


        }
        //사원추가 버튼
        private void button5_Click_1(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show(); // 모달방식
        }
        //datafridview1 행 클릭시 datafridview2에 데이터 추가됨.
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            rowselect = e.RowIndex; // 현재 선택된 행값을 가져옴.
            string[] s = new string[8];
            if(dataGridView2.RowCount > 0) // 이미 있는 경우는 삭제
            {
                dataGridView2.Rows.Remove(dataGridView2.Rows[0]); // 해당되는 row 삭제
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                s[i] = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
            }
            dataGridView2.Rows.Add(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
            delcode = s[0];
            
        }
        //datafrideview1 클릭 이벤트 (delete처리)
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //delete키 누르면 싫행.
            if(e.KeyCode == Keys.Delete)
            {

                if (MessageBox.Show("정말 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataGridView1.Rows.Remove(dataGridView1.Rows[rowselect]); // 해당되는 row 삭제
                    dataGridView2.Rows.Remove(dataGridView1.Rows[0]);// row 하나밖에 없으므로 0번째 행 삭제
                }
                else
                {
                    MessageBox.Show("행이 선택되어있는지 확인해주세요");
                }
            }
        }
        // chart 콤보박스1 변경 시 이벤트 (
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series["Series1"].Points.Clear();
            // staffcode 클릭 시 모든 사원들의 총수익이 보이도록
            if ("staffcode" == comboBox4.SelectedItem.ToString())
            {
                mainquery(dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                for (int i = 0; i < dataGridView1.ColumnCount; i++) //column개수 만큼 동작
                {
                    while (mainreader.Read()) // 데이터 읽어와서 chart에 부착
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString() == mainreader["table2_staffcode"].ToString())
                        {
                            chart1.Series["Series1"].Points.AddXY(mainreader["table2_staffcode"], mainreader["revenue"]); //staffcode를 x에, 수익을 y에
                        }
                    }
                }
                mainreader.Close(); // 항상 사용 후 종료
            }
            else if("무사고여부" == comboBox4.SelectedItem.ToString())
            {

            }else if("배달건수" == comboBox4.SelectedItem.ToString())
            {

            }else if("출/퇴근" == comboBox4.SelectedItem.ToString())
            {

            }else if("수익률" == comboBox4.SelectedItem.ToString())
            {

            }
        }
        // chart 콤보박스2 변경 시 이벤트 (chart 모양 조절)
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ("Line" == comboBox5.SelectedItem.ToString())
            {
                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            else if ("Column" == comboBox5.SelectedItem.ToString())
            {
                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            }
            else if ("Point" == comboBox5.SelectedItem.ToString())
            {
                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
            else if ("Pie" == comboBox5.SelectedItem.ToString())
            {
                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            }
            else if ("Doughnut" == comboBox5.SelectedItem.ToString())
            {
                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            }
        }
        //
        //커스텀 함수
        // today => 원하는 날짜(선택된 날짜)
        private void mainquery(String today)
        {
            // 데이터 가져와서 DataGridView에 설정
            string selectQuery = "SELECT * FROM main_table_test where date= "+"'"+today+"'"; // 전체 항목 읽어오기
            cmd.CommandText = selectQuery; // cmd에 쿼리 설정
            mainreader = cmd.ExecuteReader();
        }
        private void subquery()
        {
            string selectQuery = "SELECT * FROM sub_table_test"; // 전체 항목 읽어오기
            cmd.CommandText = selectQuery; // cmd에 쿼리 설정
            subreader = cmd.ExecuteReader();
        }
        private void searchquery(String field, String data)
        {
            // 데이터 가져와서 DataGridView에 설정
            string selectQuery = "SELECT * FROM main_table_test where " + field + "= " + "'" + data + "'";
            cmd.CommandText = selectQuery; // cmd에 쿼리 설정
            mainreader = cmd.ExecuteReader();
        }
        private void searchnamequery(String field, List<String> data)
        {
            String syntex = "";
            // 데이터 가져와서 DataGridView에 설정
            for(int i = 0; i < data.Count; i++)
            {
                if(i == data.Count-1)
                {
                    syntex += data[i];
                }
                else
                {
                syntex += data[i] + " or ";
                }
            }
            string selectQuery = "SELECT * FROM main_table_test where " + field + "= " +  syntex;
            cmd.CommandText = selectQuery; // cmd에 쿼리 설정
            mainreader = cmd.ExecuteReader();
        }
        private List<String> searchname(String data)
        {
            // 데이터 가져와서 DataGridView에 설정
            string selectQuery = "SELECT * FROM sub_table_test where name= " + "'" + data + "'";
            cmd.CommandText = selectQuery; // cmd에 쿼리 설정
            subreader = cmd.ExecuteReader();
            List<String> namelist = new List<String>();
            while (subreader.Read())
            {
                namelist.Add(subreader["staffcode"].ToString());
            }
            subreader.Close();
            return namelist;
        } 
        // 메인 리스트 뷰에 전체 데이터 표시 (데이터 읽어오기)
        private void Main_ListView_items_Reader(String today)
        {
            dataGridView1.Rows.Clear(); // 데이터 그리드 뷰 초기화
            if (textBox1.Text == "")
            {
                mainquery(today);
                while (mainreader.Read())
                {
                    dataGridView1.Rows.Add(mainreader["casecode"], mainreader["table2_staffcode"], "", mainreader["accident_free"], mainreader["case_number"], mainreader["date"], mainreader["commute"], mainreader["revenue"]);
                }
                mainreader.Close();
                for (int i = 0; i < dataGridView1.RowCount; i++) //Row개수 만큼 동작
                {
                    if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "0")
                        dataGridView1.Rows[i].Cells[3].Value = "무사고";
                    else if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "1")
                        dataGridView1.Rows[i].Cells[3].Value = "사고발생";
                    if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "1")
                        dataGridView1.Rows[i].Cells[6].Value = "정상퇴근";
                    else if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "0")
                        dataGridView1.Rows[i].Cells[6].Value = "시간초과";
                    
                }
                // 가져온 staffcode와 일치하면 이름을 위치에 넣음
                // subreader
                
                // 비교문을 돌려서 staffcode와 일치하면 이름을 가져와서 적용함.
                for (int i = 0; i < dataGridView1.RowCount; i++) //Row개수 만큼 동작
                {
                    subquery();
                    while (subreader.Read())
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString() == subreader["staffcode"].ToString())
                        {
                            dataGridView1.Rows[i].Cells[2].Value = subreader["name"];
                        }
                    }
                    subreader.Close();
                }
            }
            else
            {
                // 검색창 텍스트 박스에 내용이 입력 되면 해당 텍스트 박스내용에 있는 행을 보여줌. (현재 staff_code로 한정되어있음)
                string selectQuery = "SELECT * FROM main_table_test where table2_staffcode = '" + textBox1.Text + " '";
                cmd.CommandText = selectQuery;
                mainreader = cmd.ExecuteReader();
                try
                {
                    while (mainreader.Read())
                    {
                        // 해당되는 항목이 들어있는 행을 전부 가지고 옴.
                        dataGridView1.Rows.Add(mainreader["casecode"], mainreader["table2_staffcode"], "", mainreader["accident_free"], mainreader["case_number"], mainreader["date"], mainreader["revenue"], mainreader["commute"]);
                    }
                    mainreader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message); // 에러 메시지 출력
                }
            }          
        }

        private void SearchData(String field,String data)
        {
            // 이름필드 - 테이블이 달라서 따로 서치
            if(field == "name")
            {
                field = "table2_staffcode";
                searchnamequery(field, searchname(data));
            }
            else
            {
            searchquery(field, data);
            }
            
            while (mainreader.Read())
            {
                dataGridView1.Rows.Add(mainreader["casecode"], mainreader["table2_staffcode"], "", mainreader["accident_free"], mainreader["case_number"], mainreader["date"], mainreader["commute"], mainreader["revenue"]);
            }
            mainreader.Close();
            for (int i = 0; i < dataGridView1.RowCount; i++) //Row개수 만큼 동작
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "0")
                    dataGridView1.Rows[i].Cells[3].Value = "무사고";
                else if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "1")
                    dataGridView1.Rows[i].Cells[3].Value = "사고발생";
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "1")
                    dataGridView1.Rows[i].Cells[6].Value = "정상퇴근";
                else if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "0")
                    dataGridView1.Rows[i].Cells[6].Value = "시간초과";

            }
            // 가져온 staffcode와 일치하면 이름을 위치에 넣음
            // subreader

            // 비교문을 돌려서 staffcode와 일치하면 이름을 가져와서 적용함.
            for (int i = 0; i < dataGridView1.RowCount; i++) //Row개수 만큼 동작
            {
                subquery();
                while (subreader.Read())
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() == subreader["staffcode"].ToString())
                    {
                        dataGridView1.Rows[i].Cells[2].Value = subreader["name"];
                    }
                }
                subreader.Close();
            }
        }

        // 검색
        private void Search(String data)
        {
            dataGridView1.Rows.Clear(); // 데이터 그리드 뷰 초기화
            String selectdata = comboBox1.SelectedItem.ToString();
            
            if(data == null)
            {
                return;
            }
            // 사건번호
            else if (selectdata.Equals("사건번호"))
            {
                selectdata = "casecode";
                SearchData(selectdata, data);

            }
            // 사원코드
            else if(selectdata.Equals("사원코드"))
            {
                selectdata = "table2_staffcode";
                SearchData(selectdata, data);
            }
            // 사원명
            else if (selectdata.Equals("사원명"))
            {
                selectdata = "name";
                SearchData(selectdata, data);
            }
            // 무사고 여부
            else if (selectdata.Equals("무사고 여부"))
            {
                selectdata = "accident_free";
                if(data == "무사고")
                {
                    data = "0";
                }
                else if(data == "사고")
                {
                    data = "1";
                }
                SearchData(selectdata, data);
            }
            // 배달건수
            else if (selectdata.Equals("배달건수"))
            {
                selectdata = "case_number";
                SearchData(selectdata, data);
            }
            // 출퇴근
            else if (selectdata.Equals("출/퇴근"))
            {
                selectdata = "commute";
                if(data == "정상퇴근")
                {
                    data= "1";
                }
                else if(data == "시간초과")
                {
                    data = "0";
                }
                SearchData(selectdata, data);
            }
            // 수익
            else if (selectdata.Equals("수익(이상)"))
            {
                selectdata = "revenue";
                SearchData(selectdata, data);
            }

        }

        //UPDATE처리
        public void UpdateDB()
        {
            string sql = "Update user Set name ='홍길동2' where id = 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        //DELETE처리
        public void DeleteDB(String code)
        {
            string sql = "DELETE FROM main_table_test where casecode = '" + code + "'"; // datafridview 값이 필요.
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
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

        // 날짜 선택 데이터
        int year;
        int month;
        int day;
        String dateSet;
        
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime SelectDate = dateTimePicker1.Value;
            dateSet = SelectDate.ToString("yyyy-MM-dd");
            year = SelectDate.Year;
            month = SelectDate.Month;
            day = SelectDate.Day;

            //MessageBox.Show(SelectDate.ToString());
            textBox1.Text = "";
            Main_ListView_items_Reader(dateSet);
        }
    }
}
