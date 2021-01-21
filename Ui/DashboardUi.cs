using IntroductionwebservicesClient.Service;
using IntroductionwebservicesClient.Service.dto;
using IntroductionwebservicesClient.Service.form;
using System;
using System.Net;
using System.Windows.Forms;

namespace IntroductionwebservicesClient
{
    public partial class DashboardUi : Form
    {
        private CourseService courseService = new CourseService();
        private StudentService studentService = new StudentService();

        public DashboardUi()
        {
            InitializeComponent();

            courseYearTextBox.Text = DateTime.Now.Year.ToString();
            tabControl_SelectedIndexChanged(tabControl, null);
        }

        private void addCourseButton_Click(object sender, EventArgs e)
        {
            new CourseService().InsertCourseAsync(new CourseForm
            {
                name = courseNameTextBox.Text,
                year = int.Parse(courseYearTextBox.Text)
            }).ContinueWith(response =>
            {
                if (response.Result.StatusCode == HttpStatusCode.Created)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "Curso inserido com sucesso");

                        courseNameTextBox.Text = "";
                        courseYearTextBox.Text = DateTime.Now.Year.ToString();
                    }));

                    return;
                }

                response.Result.Content.ReadAsStringAsync().ContinueWith(message =>
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, message.Result, "Falha ao inserir curso");
                    }));
                });
            });
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == dashboardTab
               || tabControl.SelectedTab == studentsRegistrationTab)
            {
                loadCourses();
            }
        }

        private void loadCourses()
        {
            courseService.FetchAll().ContinueWith(t =>
            {
                coursesGridView.Invoke(new MethodInvoker(delegate
                {
                    coursesGridView.DataSource = t.Result;
                    courseComboBox.DataSource = t.Result;

                    addStudentButton.Enabled = courseComboBox.Items.Count > 0;

                    try
                    {
                        coursesGridView_CellClick(coursesGridView, new DataGridViewCellEventArgs(0, 0));
                    }
                    catch (Exception) { }
                }));
            });
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            studentService.InsertStudentAsync(new StudentForm
            {
                name = studentNameTextbox.Text,
                courseName = (courseComboBox.SelectedItem as CourseDto).name,
                enrolled = enrolledCheckbox.Checked
            }).ContinueWith(response =>
            {
                if (response.Result.StatusCode == HttpStatusCode.Created)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "Aluno inserido com sucesso");

                        studentNameTextbox.Text = "";
                        courseComboBox.SelectedIndex = 0;
                        enrolledCheckbox.Checked = false;

                    }));

                    return;
                }

                response.Result.Content.ReadAsStringAsync().ContinueWith(message =>
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, message.Result, "Falha ao inserir aluno");
                    }));
                });
            });
        }

        private void coursesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try{
                DataGridViewRow row = coursesGridView.Rows[e.RowIndex];

                long id = long.Parse(row.Cells[0].Value.ToString());

                studentService.FetchByCourse(id).ContinueWith(t =>
                {
                    studentsGridView.Invoke(new MethodInvoker(delegate
                    {
                        studentsGridView.DataSource = t.Result;

                        lblCourseStudents.Text = "Alunos: " + t.Result.Count;
                        lblEnrolledCourseStudents.Text = "Alunos matriculados: " + t.Result.FindAll(st => st.Enrolled).Count;
                    }));
                });
            }
            catch { }
        }
    }
}
