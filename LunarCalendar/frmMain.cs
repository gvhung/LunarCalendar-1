using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LunarCalendar
{
    public partial class frmMain : Form
    {
        //Biến
        private int timeZone = 7;
        private int currentSolarYear = 1;
        private int currentSolarMonth = 1;
        private int currentSolarDay = 1;
        private int currentLunarYear = 1;
        private int currentLunarMonth = 1;
        private bool currentLunarIsLeapMonth = false;
        private int currentLunarDay = 1;
        private int currentYear = 1;
        private int currentMonth = 1;
        //Khởi tạo
        public frmMain()
        {
            InitializeComponent();

            currentSolarYear = DateTime.Today.Year;
            currentSolarMonth = DateTime.Today.Month;
            currentSolarDay = DateTime.Today.Day;
            SolarDate today_Solar = new SolarDate(currentSolarDay, currentSolarMonth, currentSolarYear);
            LunarDate today_Lunar = today_Solar.ToLunarDate(timeZone);
            currentLunarYear = today_Lunar.Year;
            currentLunarMonth = today_Lunar.Month;
            currentLunarIsLeapMonth = today_Lunar.IsLeapMonth;
            currentLunarDay = today_Lunar.Day;

            DayCalendar_FillValue();
            MonthCalendar_FillValue();

            cldLunarDayCalendar.SelectedDate = DateTime.Today;
            cldLunarMonthCalendar.SelectedMonth = DateTime.Today;
        }
        //Phương thức
            //KeyPress
        private void txtLunarCalendar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
            //Tab DayCalendar
        private void txtDayCalendar_Year_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDayCalendar_Today.Focus();
                ((TextBox)sender).Focus();
            }
        }

        private void DayCalendar_FillValue()
        {
            //Solar Date
                //Year
            txtDayCalendar_Solar_Year.Text = currentSolarYear.ToString("0000");
                //Month
            cmbDayCalendar_Solar_Month.Text = currentSolarMonth.ToString("00");
                //Day
            cmbDayCalendar_Solar_Day.Items.Clear();
            int n = SolarDate.GetNumberOfDaysInMonth(currentSolarMonth, currentSolarYear);
            for (int i = 1; i <= n; i++)
            {
                cmbDayCalendar_Solar_Day.Items.Add((i).ToString("00"));
            }
            cmbDayCalendar_Solar_Day.Text = currentSolarDay.ToString("00");

            //Lunar Date
                //Year
            txtDayCalendar_Lunar_Year.Text = currentLunarYear.ToString("0000");
                //Month
            cmbDayCalendar_Lunar_Month.Items.Clear();
            foreach (string month in LunarDate.GetMonths(currentLunarYear, timeZone))
            {
                cmbDayCalendar_Lunar_Month.Items.Add(month);
            }
            string str = currentLunarMonth.ToString("00");
            if (currentLunarIsLeapMonth)
            {
                str += " nhuận";
            }
            cmbDayCalendar_Lunar_Month.Text = str;
                //Day
            cmbDayCalendar_Lunar_Day.Items.Clear();
            int m = LunarDate.GetNumberOfDaysInMonth(currentLunarMonth, currentLunarIsLeapMonth, currentLunarYear, timeZone);
            for (int i = 1; i <= m; i++)
            {
                cmbDayCalendar_Lunar_Day.Items.Add(i.ToString("00"));
            }
            cmbDayCalendar_Lunar_Day.Text = currentLunarDay.ToString("00");
        }
        
        private void cldLunarDayCalendar_SelectedDateChanged(object sender, EventArgs e)
        {
            currentSolarYear = cldLunarDayCalendar.SelectedDate.Year;
            currentSolarMonth = cldLunarDayCalendar.SelectedDate.Month;
            currentSolarDay = cldLunarDayCalendar.SelectedDate.Day;
            SolarDate solarDate = new SolarDate(currentSolarDay, currentSolarMonth, currentSolarYear);
            LunarDate lunarDate = solarDate.ToLunarDate(timeZone);
            currentLunarYear = lunarDate.Year;
            currentLunarMonth = lunarDate.Month;
            currentLunarIsLeapMonth = lunarDate.IsLeapMonth;
            currentLunarDay = lunarDate.Day;

            DayCalendar_UpdateValue();

            if (solarDate == SolarDate.MinValue)
            {
                btnDayCalendar_PreviousDate.Enabled = false;
            }
            else
            {
                btnDayCalendar_PreviousDate.Enabled = true;
            }

            if (solarDate == SolarDate.MaxValue)
            {
                btnDayCalendar_NextDate.Enabled = false;
            }
            else
            {
                btnDayCalendar_NextDate.Enabled = true;
            }
        }

        private void DayCalendar_UpdateValue() //Modify the Selected Date and Update Value
        {
            //Solar Date
                //Year
            txtDayCalendar_Solar_Year.Text = currentSolarYear.ToString("0000");
                //Month
            cmbDayCalendar_Solar_Month.Text = currentSolarMonth.ToString("00");
                //Day
            int n = SolarDate.GetNumberOfDaysInMonth(currentSolarMonth, currentSolarYear);
            if (n != cmbDayCalendar_Solar_Day.Items.Count)
            {
                if (n > cmbDayCalendar_Solar_Day.Items.Count)
                {
                    for (int i = cmbDayCalendar_Solar_Day.Items.Count + 1; i <= n; i++)
                    {
                        cmbDayCalendar_Solar_Day.Items.Add(i.ToString("00"));
                    }
                }
                else
                {
                    if (int.Parse(cmbDayCalendar_Solar_Day.Text) > n)
                    {
                        cmbDayCalendar_Solar_Day.Text = currentSolarDay.ToString("00");
                    }
                    for (int i = cmbDayCalendar_Solar_Day.Items.Count - 1; i >= n; i--)
                    {
                        cmbDayCalendar_Solar_Day.Items.RemoveAt(i);
                    }
                }
            }
            cmbDayCalendar_Solar_Day.Text = currentSolarDay.ToString("00");

            //Lunar Date
                //Year
            txtDayCalendar_Lunar_Year.Text = currentLunarYear.ToString("0000");
                //Month
            ArrayList months = months = LunarDate.GetMonths(currentLunarYear, timeZone);
            for (int i = cmbDayCalendar_Lunar_Month.Items.Count - 1; i >= 0; i--)
            {
                string month = (string)cmbDayCalendar_Lunar_Month.Items[i];
                if (month.Length > 2)
                {
                    bool flag = false;
                    foreach (string month1 in months)
                    {
                        if (month1 == month)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        cmbDayCalendar_Lunar_Month.Items.RemoveAt(i);
                    }
                }
            }
            foreach (string month in months)
            {
                if (month.Length > 2)
                {
                    bool flag = false;
                    foreach (string month1 in cmbDayCalendar_Lunar_Month.Items)
                    {
                        if (month1 == month)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        cmbDayCalendar_Lunar_Month.Items.Add(month);
                    }
                }  
            }
            string str = currentLunarMonth.ToString("00");
            if (currentLunarIsLeapMonth)
            {
                str += " nhuận";
            }
            cmbDayCalendar_Lunar_Month.Text = str;
            //Day
            int m = LunarDate.GetNumberOfDaysInMonth(currentLunarMonth, currentLunarIsLeapMonth, currentSolarYear, timeZone);
            if (m != cmbDayCalendar_Lunar_Day.Items.Count)
            {
                if (m > cmbDayCalendar_Lunar_Day.Items.Count)
                {
                    for (int i = cmbDayCalendar_Lunar_Day.Items.Count + 1; i <= m; i++)
                    {
                        cmbDayCalendar_Lunar_Day.Items.Add((i).ToString("00"));
                    }
                }
                else
                {
                    if (int.Parse(cmbDayCalendar_Lunar_Day.Text) > m)
                    {
                        cmbDayCalendar_Lunar_Day.Text = currentLunarDay.ToString("00");
                    }
                    for (int i = cmbDayCalendar_Lunar_Day.Items.Count - 1; i >= m; i--)
                    {
                        cmbDayCalendar_Lunar_Day.Items.RemoveAt(i);
                    }
                }
            }
            
            cmbDayCalendar_Lunar_Day.Text = currentLunarDay.ToString("00");
        }

        private void txtDayCalendar_Solar_Year_Validating(object sender, CancelEventArgs e)
        {
            //Kiểm tra giá trị năm
            if (currentSolarYear == int.Parse(txtDayCalendar_Solar_Year.Text))
                return;

            try
            {
                int year = int.Parse(txtDayCalendar_Solar_Year.Text);
                if (year < 1 || year > 9999)
                {
                    throw new Exception();
                }
                txtDayCalendar_Solar_Year.Text = year.ToString("0000");
            }
            catch
            {
                MessageBox.Show("Nhập lại giá trị năm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDayCalendar_Solar_Year.SelectAll();
                e.Cancel = true;
            }
        }

        private void txtDayCalendar_Solar_Year_Validated(object sender, EventArgs e)
        {
            currentSolarYear = int.Parse(txtDayCalendar_Solar_Year.Text);
            DayCalendar_ModifySelectedDate_Solar();
            cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
        }

        private void cmbDayCalendar_Solar_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSolarMonth = int.Parse(cmbDayCalendar_Solar_Month.Text);
            DayCalendar_ModifySelectedDate_Solar();
            cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
        }

        private void DayCalendar_ModifySelectedDate_Solar()
        {
            //Day
            int n = SolarDate.GetNumberOfDaysInMonth(currentSolarMonth, currentSolarYear);
            if (currentSolarDay > n)
            {
                currentSolarDay = n;
            }
        }

        private void cmbDayCalendar_Solar_Day_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSolarDay = int.Parse(cmbDayCalendar_Solar_Day.Text);
            cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
        }

        private void txtDayCalendar_Lunar_Year_Validating(object sender, CancelEventArgs e)
        {
            //Kiểm tra giá trị năm
            if (currentLunarYear == int.Parse(txtDayCalendar_Lunar_Year.Text))
                return;

            try
            {
                int year = int.Parse(txtDayCalendar_Lunar_Year.Text);
                if (year < 0 || year > 9999)
                    throw new Exception();
                
                txtDayCalendar_Lunar_Year.Text = year.ToString("0000");
            }
            catch
            {
                MessageBox.Show("Nhập lại giá trị năm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDayCalendar_Lunar_Year.SelectAll();
                e.Cancel = true;
            }
        }

        private void txtDayCalendar_Lunar_Year_Validated(object sender, EventArgs e)
        {
            try
            {
                lblDayCalendar_Error.Visible = false;
                btnDayCalendar_PreviousDate.Enabled = true;
                btnDayCalendar_NextDate.Enabled = true;

                currentLunarYear = int.Parse(txtDayCalendar_Lunar_Year.Text);
                DayCalendar_ModifySelectedDate_Lunar();
                LunarDate currentLunarDate = new LunarDate(currentLunarDay, currentLunarMonth, currentLunarIsLeapMonth, currentLunarYear, timeZone);
                SolarDate currentSolarDate = currentLunarDate.ToSolarDate();
                currentSolarYear = currentSolarDate.Year;
                currentSolarMonth = currentSolarDate.Month;
                currentSolarDay = currentSolarDate.Day;
                cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
            }
            catch (Exception exc)
            {
                lblDayCalendar_Error.Text = "Lỗi: " + exc.Message;
                lblDayCalendar_Error.Visible = true;
                btnDayCalendar_PreviousDate.Enabled = false;
                btnDayCalendar_NextDate.Enabled = false;
            }
        }

        private void cmbDayCalendar_Lunar_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblDayCalendar_Error.Visible = false;
                btnDayCalendar_PreviousDate.Enabled = true;
                btnDayCalendar_NextDate.Enabled = true;

                currentLunarMonth = int.Parse(cmbDayCalendar_Lunar_Month.Text.Substring(0, 2));
                if (cmbDayCalendar_Lunar_Month.Text.Length > 2)
                {
                    currentLunarIsLeapMonth = true;
                }
                DayCalendar_ModifySelectedDate_Lunar();
                LunarDate currentLunarDate = new LunarDate(currentLunarDay, currentLunarMonth, currentLunarIsLeapMonth, currentLunarYear, timeZone);
                SolarDate currentSolarDate = currentLunarDate.ToSolarDate();
                currentSolarYear = currentSolarDate.Year;
                currentSolarMonth = currentSolarDate.Month;
                currentSolarDay = currentSolarDate.Day;
                cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
            }
            catch (Exception exc)
            {
                lblDayCalendar_Error.Text = "Lỗi: " + exc.Message;
                lblDayCalendar_Error.Visible = true;
                btnDayCalendar_PreviousDate.Enabled = false;
                btnDayCalendar_NextDate.Enabled = false;
            }
        }

        private void DayCalendar_ModifySelectedDate_Lunar()
        {
            //Month
            ArrayList months = LunarDate.GetMonths(currentLunarYear, timeZone);
            if (currentLunarIsLeapMonth)
            {
                bool flag = false;
                foreach (string month in months)
                {
                    if (month.Length > 2 && month.Substring(0, 2) == currentLunarMonth.ToString("00"))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    currentLunarIsLeapMonth = false;
                }
            }

            //Day
            int n = LunarDate.GetNumberOfDaysInMonth(currentLunarMonth, currentLunarIsLeapMonth, currentLunarYear, timeZone);
            if (currentLunarDay > n)
            {
                currentLunarDay = n;
            }
        }

        private void cmbDayCalendar_Lunar_Day_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblDayCalendar_Error.Visible = false;
                btnDayCalendar_PreviousDate.Enabled = true;
                btnDayCalendar_NextDate.Enabled = true;

                currentLunarDay = int.Parse(cmbDayCalendar_Lunar_Day.Text);
                LunarDate currentLunarDate = new LunarDate(currentLunarDay, currentLunarMonth, currentLunarIsLeapMonth, currentLunarYear, timeZone);
                SolarDate currentSolarDate = currentLunarDate.ToSolarDate();
                currentSolarYear = currentSolarDate.Year;
                currentSolarMonth = currentSolarDate.Month;
                currentSolarDay = currentSolarDate.Day;
                cldLunarDayCalendar.SelectedDate = new DateTime(currentSolarYear, currentSolarMonth, currentSolarDay);
            }
            catch (Exception exc)
            {
                lblDayCalendar_Error.Text = "Lỗi: " + exc.Message;
                lblDayCalendar_Error.Visible = true;
                btnDayCalendar_PreviousDate.Enabled = false;
                btnDayCalendar_NextDate.Enabled = false;
            }
        }

        private void btnDayCalendar_PreviousDate_Click(object sender, EventArgs e)
        {
            cldLunarDayCalendar.SelectedDate = cldLunarDayCalendar.SelectedDate.AddDays(-1);
        }

        private void btnDayCalendar_NextDate_Click(object sender, EventArgs e)
        {
            cldLunarDayCalendar.SelectedDate = cldLunarDayCalendar.SelectedDate.AddDays(1);
        }

        private void btnDayCalendar_Today_Click(object sender, EventArgs e)
        {
            cldLunarDayCalendar.SelectedDate = DateTime.Today;
        }
        
        private void rdbDayCalendar_CheckedChanged(object sender, EventArgs e)
        {
            txtDayCalendar_Solar_Year.Enabled = rdbDayCalendar_Solar.Checked;
            cmbDayCalendar_Solar_Month.Enabled = rdbDayCalendar_Solar.Checked;
            cmbDayCalendar_Solar_Day.Enabled = rdbDayCalendar_Solar.Checked;

            txtDayCalendar_Lunar_Year.Enabled = rdbDayCalendar_Lunar.Checked;
            cmbDayCalendar_Lunar_Month.Enabled = rdbDayCalendar_Lunar.Checked;
            cmbDayCalendar_Lunar_Day.Enabled = rdbDayCalendar_Lunar.Checked;
        }
            //Tab MonthCalendar
        private void txtMonthCalendar_Year_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnMonthCalendar_Today.Focus();
                ((TextBox)sender).Focus();
            }
        }

        private void MonthCalendar_FillValue()
        {
            txtMonthCalendar_Year.Text = currentSolarYear.ToString("0000");
            cmbMonthCalendar_Month.Text = currentSolarMonth.ToString("00");
        }

        private void cldLunarMonthCalendar_SelectedMonthChanged(object sender, EventArgs e)
        {
            currentMonth = cldLunarMonthCalendar.SelectedMonth.Month;
            currentYear = cldLunarMonthCalendar.SelectedMonth.Year;

            txtMonthCalendar_Year.Text = cldLunarMonthCalendar.SelectedMonth.Year.ToString("0000");
            cmbMonthCalendar_Month.Text = cldLunarMonthCalendar.SelectedMonth.Month.ToString("00");

            if (currentSolarYear == 1 && currentMonth == 1)
            {
                btnMonthCalendar_PreviousMonth.Enabled = false;
            }
            else
            {
                btnMonthCalendar_PreviousMonth.Enabled = true;
            }

            if (currentYear == 9999 && currentMonth == 12)
            {
                btnMonthCalendar_NextMonth.Enabled = false;
            }
            else
            {
                btnMonthCalendar_NextMonth.Enabled = true;
            }

            SolarDate sd = new SolarDate(cldLunarMonthCalendar.SelectedMonth);
            LunarDate ld = sd.ToLunarDate(timeZone);
        }

        private void txtMonthCalendar_Year_Validating(object sender, CancelEventArgs e)
        {
            //Kiểm tra giá trị năm
            if (currentYear == int.Parse(txtMonthCalendar_Year.Text))
                return;

            try
            {
                int year = int.Parse(txtMonthCalendar_Year.Text);
                if (year < 1 || year > 9999)
                {
                    throw new Exception();
                }
                txtMonthCalendar_Year.Text = year.ToString("0000");
            }
            catch
            {
                MessageBox.Show("Nhập lại giá trị năm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMonthCalendar_Year.SelectAll();
                e.Cancel = true;
            }
        }

        private void txtMonthCalendar_Year_Validated(object sender, EventArgs e)
        {
            currentYear = int.Parse(txtMonthCalendar_Year.Text);
            cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear, currentMonth, 1);
        }

        private void cmbMonthCalendar_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMonth = int.Parse(cmbMonthCalendar_Month.Text);
            cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear, currentMonth, 1);
        }

        private void btnMonthCalendar_PreviousMonth_Click(object sender, EventArgs e)
        {
            if (currentMonth == 1)
                cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear - 1, 12, 1);
            else
                cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear, currentMonth - 1, 1);
        }

        private void btnMonthCalendar_NextMonth_Click(object sender, EventArgs e)
        {
            if (currentMonth == 12)
                cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear + 1, 1, 1);
            else
                cldLunarMonthCalendar.SelectedMonth = new DateTime(currentYear, currentMonth + 1, 1);
        }

        private void btnMonthCalendar_Today_Click(object sender, EventArgs e)
        {
            cldLunarMonthCalendar.SelectedMonth = DateTime.Today;
        }
    }
}
