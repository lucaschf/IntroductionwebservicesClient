using IntroductionwebservicesClient.Service;
using IntroductionwebservicesClient.Service.dto;
using IntroductionwebservicesClient.Service.form;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntroductionwebservicesClient
{
    public partial class DashboardUi : Form
    {

        private const String FAILED_TO_COMMUNICATE = "Falha ao comunicar com o servidor";

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
            courseService.InsertCourseAsync(new CourseForm
            {
                name = courseNameTextBox.Text,
                year = int.Parse(courseYearTextBox.Text)
            }).ContinueWith(response =>
            {
                try
                {
                    if (response.Result.StatusCode == HttpStatusCode.Created)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show(this, "Curso inserido com sucesso", "Sucesso");

                            courseNameTextBox.Text = "";
                            courseYearTextBox.Text = DateTime.Now.Year.ToString();
                        }));

                        return;
                    }

                    showInsertionErrorResult(response);
                }
                catch
                {
                    showServerConnectionError();
                }
            });
        }

        private void showServerConnectionError()
        {
            Invoke(new Action(() =>
            {
                MessageBox.Show(this, FAILED_TO_COMMUNICATE, "Sem conexão");
            }));
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
            isLoading(true);

            try
            {
                courseService.FetchAll().ContinueWith(t =>
                {
                    Invoke(new Action(() =>
                    {
                        try
                        {
                            coursesGridView.DataSource = t.Result;
                            courseComboBox.DataSource = t.Result;

                            isLoading(false, "");
                            coursesGridView_CellClick(coursesGridView, new DataGridViewCellEventArgs(0, 0));
                        }
                        catch
                        {
                            isLoading(false, FAILED_TO_COMMUNICATE);
                            showServerConnectionError();
                        }

                        courseComboBox.Enabled = courseComboBox.Items.Count > 0;
                    }));
                });
            }
            catch (Exception) { }
        }

        private void isLoading(bool loading, String statusMessage = "Carregando...")
        {
            serverStatusLabel.Text = statusMessage;

            progressBar.Visible = loading;
            statusPanel.Visible = loading;
            studentsCountPanel.Visible = !loading;
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            if (courseComboBox.SelectedValue == null)
                return;

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
                        MessageBox.Show(this, "Aluno inserido com sucesso", "Sucesso");

                        studentNameTextbox.Text = "";
                        courseComboBox.SelectedIndex = 0;
                        enrolledCheckbox.Checked = false;

                    }));

                    return;
                }

                showInsertionErrorResult(response);
            });
        }

        private void showInsertionErrorResult(Task<HttpResponseMessage> response)
        {
            try
            {
                showValidationError(response);
            }
            catch
            {
                response.Result.Content.ReadAsStringAsync().ContinueWith(message =>
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, message.Result, "Falha");
                    }));
                });
            }
        }

        private void showValidationError(Task<HttpResponseMessage> response)
        {
            response.Result.Content.ReadAsAsync<List<FormErrorDto>>().ContinueWith(message =>
            {
                Invoke(new Action(() =>
                {
                    var msg = "";
                    foreach (FormErrorDto error in message.Result)
                        msg += error.ToString() + "\n";

                    MessageBox.Show(this, msg, "Dados inválidos");
                }));
            });
        }

        private void coursesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = coursesGridView.Rows[e.RowIndex];

                long id = long.Parse(row.Cells[0].Value.ToString());

                studentService.FetchByCourse(id).ContinueWith(t =>
                {
                    Invoke(new Action(() =>
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
