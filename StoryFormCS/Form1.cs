using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StoryFormCS
{
    public partial class Form1 : Form
    {
        ArrayList linesList = new ArrayList();

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string time = hour.Text + ":" + min.Text + ":" + sec.Text;
            string place = textBox2.Text;
            string speaker = textBox3.Text;
            string lines = Convert.ToString(textBox4.Text);
            
            lines = lines.Replace("'", "\\'");

            string data = time + "," + place + "," + speaker + "," + lines;
            listBox1.Items.Add(data);

            Dictionary<string, string> speech = new Dictionary<string, string>();
            speech.Add("time", time);
            speech.Add("place", place);
            speech.Add("speaker", speaker);
            speech.Add("lines", lines);

            linesList.Add(speech);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (linesList.Count > 0)
            {
                string json = @"{ ";
                json += @"scean1 : { time : '" + ((Dictionary<string, string>)linesList[0])["time"]
                    + @"', place : '" + ((Dictionary<string, string>)linesList[0])["place"]
                    + @"', speaker : '" + ((Dictionary<string, string>)linesList[0])["speaker"]
                    + @"', lines : '" + ((Dictionary<string, string>)linesList[0])["lines"] + @"'}";

                for (int i = 1; i < linesList.Count; i++)
                {
                    json += @", scean" + (i + 1) + " : { time : '" + ((Dictionary<string, string>)linesList[i])["time"]
                        + @"', place : '" + ((Dictionary<string, string>)linesList[i])["place"]
                        + @"', speaker : '" + ((Dictionary<string, string>)linesList[i])["speaker"]
                        + @"', lines : '" + ((Dictionary<string, string>)linesList[i])["lines"] + @"'}";
                }
                json += @" }";
                
                JObject jo = JObject.Parse(json);

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "JSON file|*.json";
                saveFileDialog1.Title = "Save an JSON File";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, jo.ToString());
                }

                //MessageBox.Show(jo.ToString());
            }
        }

        private void hour_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void min_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void sec_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "JSON file|*.json";
            openFileDlg.Title = "Open an JSON File";

            if (openFileDlg.ShowDialog() == DialogResult.OK && openFileDlg.FileName != "")
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDlg.FileName);

                JObject jo = JObject.Parse(sr.ReadToEnd());

                int i = 1;
                while(true)
                {
                    JToken idToken = jo["scean" + i++];

                    if (idToken != null)
                    {
                        String time = idToken["time"].ToString();
                        String place = idToken["place"].ToString();
                        String speaker = idToken["speaker"].ToString();
                        String lines = idToken["lines"].ToString();

                        lines = lines.Replace("'", "\\'");

                        String data = time + "," + place + "," + speaker + "," + lines;
                        listBox1.Items.Add(data);

                        Dictionary<String, String> speech = new Dictionary<string, string>();
                        speech.Add("time", time);
                        speech.Add("place", place);
                        speech.Add("speaker", speaker);
                        speech.Add("lines", lines);

                        linesList.Add(speech);
                    }
                    else
                        break;
                }
                sr.Close();
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            int selectIndex = listBox1.SelectedIndex;
            if (selectIndex != -1)
            {
                button4.Enabled = true;
                
                Dictionary<String, String> item = (Dictionary<String, String>)linesList[selectIndex];

                string time = item["time"];
                string[] value = time.Split(':');
                hour.Text = value[0];
                min.Text = value[1];
                sec.Text = value[2];

                textBox2.Text = item["place"];
                textBox3.Text = item["speaker"];

                string lines = item["lines"];
                lines = lines.Replace("\\'", "'");
                textBox4.Text = lines;
            }
            else
            {
                button4.Enabled = false;

                hour.Text = "";
                min.Text = "";
                sec.Text = "";

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            //MessageBox.Show("Hello : " + selectIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectIndex = listBox1.SelectedIndex;

            if(selectIndex != -1)
            {
                Dictionary<string, string> selectItem = (Dictionary<string, string>)linesList[selectIndex];
                linesList.RemoveAt(selectIndex);
                listBox1.Items.RemoveAt(selectIndex);
                button4.Enabled = false;

                hour.Text = "";
                min.Text = "";
                sec.Text = "";

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.SelectionMode = SelectionMode.None;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Hello : " + selectIndex);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                groupBox3.Enabled = true;
            else
                groupBox3.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
                groupBox4.Enabled = true;
            else
                groupBox4.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
                textBox10.Enabled = true;
            else
                textBox10.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (linesList.Count > 0)
            {
                string condition = "condition : { ";
                condition += "period : {";

                if(checkBox1.Checked)
                {
                    if(radioButton1.Checked)
                    {
                        condition += "every : 'day";
                        condition += "', time : '" + textBox7.Text + ":" + textBox6.Text + ":" + textBox5.Text;
                    }
                    else if(radioButton2.Checked)
                    {
                        string weekName;
                        condition += "every : 'week";

                        if(comboBox1.Text == "일")
                            weekName = "0";
                        else if (comboBox1.Text == "월")
                            weekName = "1";
                        else if (comboBox1.Text == "화")
                            weekName = "2";
                        else if (comboBox1.Text == "수")
                            weekName = "3";
                        else if (comboBox1.Text == "목")
                            weekName = "4";
                        else if (comboBox1.Text == "금")
                            weekName = "5";
                        else if (comboBox1.Text == "토")
                            weekName = "6";
                        else
                            weekName = "0";

                        condition += "', time : '" + weekName + " " + textBox7.Text + ":" + textBox6.Text + ":" + textBox5.Text;
                    }
                    else if(radioButton3.Checked)
                    {
                        condition += "every : 'month";
                        condition += "', time : '" + comboBox2.Text + " " + textBox7.Text + ":" + textBox6.Text + ":" + textBox5.Text;
                    }
                    else
                    {
                        condition += "every : 'date";
                        condition += "', time : '" + comboBox3.Text + "." + comboBox4.Text + "." + comboBox2.Text + " " + textBox7.Text + ":" + textBox6.Text + ":" + textBox5.Text;
                    }
                }
                else
                    condition += "every : 'none', time : 'none";

                condition += "'}";
                condition += ", place : {";

                if(checkBox2.Checked)
                {
                    condition += "mapName : '" + textBox1.Text;
                    condition += "', xpos : '" + textBox8.Text;
                    condition += "', ypos : '" + textBox9.Text;
                }
                else
                    condition += "mapName : 'none', xpos : 'none', ypos : 'none";

                condition += "'}";

                if (checkBox3.Checked)
                    condition += ", procedeEvent : '" + textBox10.Text;
                else
                    condition += ", procedeEvent : 'none";

                condition += "'}";

                string json = "script : { ";
                json += "scean1 : { time : '" + ((Dictionary<string, string>)linesList[0])["time"]
                    + "', place : '" + ((Dictionary<string, string>)linesList[0])["place"]
                    + "', speaker : '" + ((Dictionary<string, string>)linesList[0])["speaker"]
                    + "', lines : '" + ((Dictionary<string, string>)linesList[0])["lines"] + "'}";

                for (int i = 1; i < linesList.Count; i++)
                {
                    json += ", scean" + (i + 1) + " : { time : '" + ((Dictionary<string, string>)linesList[i])["time"]
                        + "', place : '" + ((Dictionary<string, string>)linesList[i])["place"]
                        + "', speaker : '" + ((Dictionary<string, string>)linesList[i])["speaker"]
                        + "', lines : '" + ((Dictionary<string, string>)linesList[i])["lines"] + "'}";
                }
                json += " }";

                string output = "{name : '" + textBox11.Text + "', " + condition + ", " + json + "}";
                JObject jo = JObject.Parse(output);

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = textBox11.Text;
                saveFileDialog1.Filter = "JSON file|*.json";
                saveFileDialog1.Title = "Save an JSON File";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, jo.ToString());
                }

                //MessageBox.Show(jo.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "JSON file|*.json";
            openFileDlg.Title = "Open an JSON File";

            if (openFileDlg.ShowDialog() == DialogResult.OK && openFileDlg.FileName != "")
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDlg.FileName);

                JObject jo = JObject.Parse(sr.ReadToEnd());

                JToken name = jo["name"];
                textBox11.Text = name.ToString();

                JToken condition = jo["condition"];

                JToken conditionPeriod = condition["period"];

                if (conditionPeriod.ToString() != "none")
                {
                    JToken conditionPeriodEvery = conditionPeriod["every"];
                    JToken conditionPeriodTime = conditionPeriod["time"];

                    checkBox1.Checked = true;
                    groupBox3.Enabled = true;

                    if (conditionPeriodEvery.ToString() == "day")
                    {
                        radioButton1.Checked = true;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                        radioButton4.Checked = false;

                        string timeValue = conditionPeriodTime.ToString();
                        string[] timeValueList = timeValue.Split(':');

                        textBox7.Text = timeValueList[0];
                        textBox6.Text = timeValueList[1];
                        textBox5.Text = timeValueList[2];
                    }
                    else if (conditionPeriodEvery.ToString() == "week")
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = true;
                        radioButton3.Checked = false;
                        radioButton4.Checked = false;

                        string weekValue = conditionPeriodTime.ToString();
                        string[] weekValueList = weekValue.Split(' ');

                        if (weekValueList[0] == "0")
                            comboBox1.Text = "일";
                        else if (weekValueList[0] == "1")
                            comboBox1.Text = "월";
                        else if (weekValueList[0] == "2")
                            comboBox1.Text = "화";
                        else if (weekValueList[0] == "3")
                            comboBox1.Text = "수";
                        else if (weekValueList[0] == "4")
                            comboBox1.Text = "목";
                        else if (weekValueList[0] == "5")
                            comboBox1.Text = "금";
                        else if (weekValueList[0] == "6")
                            comboBox1.Text = "토";
                        else
                            comboBox1.Text = "일";

                        string timeValue = weekValueList[1];
                        string[] timeValueList = timeValue.Split(':');

                        textBox7.Text = timeValueList[0];
                        textBox6.Text = timeValueList[1];
                        textBox5.Text = timeValueList[2];
                    }
                    else if (conditionPeriodEvery.ToString() == "month")
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        radioButton3.Checked = true;
                        radioButton4.Checked = false;

                        string dayValue = conditionPeriodTime.ToString();
                        string[] dayValueList = dayValue.Split(' ');
                        comboBox2.Text = dayValueList[0];

                        string timeValue = dayValueList[1];
                        string[] timeValueList = timeValue.Split(':');

                        textBox7.Text = timeValueList[0];
                        textBox6.Text = timeValueList[1];
                        textBox5.Text = timeValueList[2];
                    }
                    else if (conditionPeriodEvery.ToString() == "date")
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                        radioButton4.Checked = true;

                        string dayValue = conditionPeriodTime.ToString();
                        string[] dayValueList = dayValue.Split(' ');
                        string yearMonthDayValue = dayValueList[0];
                        string[] yearMonthDayValueList = yearMonthDayValue.Split('.');
                        comboBox3.Text = yearMonthDayValueList[0];
                        comboBox4.Text = yearMonthDayValueList[1];
                        comboBox2.Text = yearMonthDayValueList[2];

                        string timeValue = dayValueList[1];
                        string[] timeValueList = timeValue.Split(':');

                        textBox7.Text = timeValueList[0];
                        textBox6.Text = timeValueList[1];
                        textBox5.Text = timeValueList[2];
                    }
                }
                else
                {
                    checkBox1.Checked = false;
                    groupBox3.Enabled = false;
                }

                JToken conditionPlace = condition["place"];
                JToken conditionPlaceMapName = conditionPlace["mapName"];

                if (conditionPlaceMapName.ToString() != "none")
                {
                    JToken conditionPlaceXpos = conditionPlace["xpos"];
                    JToken conditionPlaceYpos = conditionPlace["ypos"];

                    checkBox2.Checked = true;
                    groupBox4.Enabled = true;

                    textBox1.Text = conditionPlaceMapName.ToString();
                    textBox8.Text = conditionPlaceXpos.ToString();
                    textBox9.Text = conditionPlaceYpos.ToString();
                }
                else
                {
                    checkBox2.Checked = false;
                    groupBox4.Enabled = false;
                }

                JToken conditionProcedeEvent = condition["procedeEvent"];

                if (conditionProcedeEvent.ToString() != "none")
                {
                    textBox10.Enabled = true;
                }
                else
                {
                    textBox10.Enabled = false;
                }

                JToken script = jo["script"];

                int i = 1;
                while (true)
                {
                    JToken idToken = script["scean" + i++];

                    if (idToken != null)
                    {
                        String time = idToken["time"].ToString();
                        String place = idToken["place"].ToString();
                        String speaker = idToken["speaker"].ToString();
                        String lines = idToken["lines"].ToString();

                        lines = lines.Replace("'", "\\'");

                        String data = time + "," + place + "," + speaker + "," + lines;
                        listBox1.Items.Add(data);

                        Dictionary<String, String> speech = new Dictionary<string, string>();
                        speech.Add("time", time);
                        speech.Add("place", place);
                        speech.Add("speaker", speaker);
                        speech.Add("lines", lines);

                        linesList.Add(speech);
                    }
                    else
                        break;
                }
                sr.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "TMX file|*.tmx";
            openFileDlg.Title = "Open an TMX File";

            if (openFileDlg.ShowDialog() == DialogResult.OK && openFileDlg.FileName != "")
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDlg.FileName);

                string fileName = openFileDlg.FileName;
                string[] splitFileName = fileName.Split('\\');
                string outFileName = "";

                for(int i=0; i<splitFileName.Length; i++)
                {
                    if(splitFileName[i] == "Resources")
                    {
                        for(int j=i + 1; j< splitFileName.Length; j++)
                        {
                            outFileName += splitFileName[j];

                            if((j + 1) != splitFileName.Length)
                                outFileName += "/";
                        }
                        break;
                    }
                }

                textBox1.Text = outFileName;

                sr.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "JSON file|*.json";
            openFileDlg.Title = "Open an JSON File";

            if (openFileDlg.ShowDialog() == DialogResult.OK && openFileDlg.FileName != "")
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDlg.FileName);

                JObject jo = JObject.Parse(sr.ReadToEnd());

                JToken name = jo["name"];
                textBox10.Text = name.ToString();

                sr.Close();
            }
        }
    }
}
