#region

using System;
using System.ComponentModel;
using System.Windows.Forms;
using UniTimetable.Model.Solver;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormProgress : Form
    {
        private SolverResult Result_ = SolverResult.Clash;
        private Solver Solver_;
        private float TimeElapsed_;

        public FormProgress()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Solver solver)
        {
            Solver_ = solver;
            if (!Solver_.Timetable.RecomputeSolutions)
                return DialogResult.OK;
            base.ShowDialog();
            return (Result_ == SolverResult.Complete ? DialogResult.OK : DialogResult.Cancel);
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void FormProgress_Load(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            txtProgress.Text = "Calculating: 0%\r\nTime Elapsed: 0s";
            Result_ = SolverResult.Clash;

            backgroundWorker.RunWorkerAsync();
            timer.Start();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Solver_.Compute((BackgroundWorker) sender, e);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateProgress(e.ProgressPercentage);
        }

        private void UpdateProgress(int percent)
        {
            progressBar.Value = percent;
            txtProgress.Text = "Calculating: " + percent + "%\r\n" + txtProgress.Lines[1];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Stop();
            if (e.Cancelled)
            {
                if (Solver_.Solutions.Count > 0 &&
                    MessageBox.Show("Calculation aborted!\nWould you like to use the solutions found so far?",
                        "Find Solutions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Result_ = SolverResult.Complete;
                else
                    Result_ = SolverResult.UserCancel;
            }
            else
            {
                Result_ = (SolverResult) e.Result;
                if (Result_ == SolverResult.NoTimetable)
                {
                    MessageBox.Show("No timetable loaded.", "Find Solutions", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                else if (Result_ == SolverResult.Clash)
                {
                    // TODO: be more helpful here (maybe move this back to main form?)
                    MessageBox.Show("Couldn't find any solutions, there's an inherent clash.", "Find Solutions",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeElapsed_ += 0.1f;
            txtProgress.Text = txtProgress.Lines[0] + "\r\nTime Elapsed: " + TimeElapsed_.ToString("0.0") + "s";
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                backgroundWorker.CancelAsync();
            }
        }
    }
}