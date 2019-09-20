using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CPUMonitor
{


    public partial class Form1 : Form
    {

        private Thread workerThread = null;
        private bool countersRunning = false;

        object __lock = new object();

        public Form1()
        {
            InitializeComponent();
            updateCountersDelegate = new UpdateCountersDelegate(UpdateCounters);

            var cat = new PerformanceCounterCategory("Processor Information");
            var instances = cat.GetInstanceNames();
            int numCores = instances.Count(s => !s.ToLower().Contains("total"));
            startStopBtn.Top = 2 * CoreHeight + 25;
            startStopBtn.Left = numCores * (CoreWidth + 2) / 2 - startStopBtn.Width / 2 - safeModeStartBtn.Width / 2;

            safeModeStartBtn.Top = 2 * CoreHeight + 25;
            safeModeStartBtn.Left = startStopBtn.Left + startStopBtn.Width + 2;

            onTopCheckBox.Top = startStopBtn.Top;
            onTopCheckBox.Left = 10;

            this.Width = numCores * (CoreWidth + 2) + 20;
            this.Height = 2 * CoreHeight + 60 + startStopBtn.Height;
        }

        public static Double Calculate(CounterSample oldSample, CounterSample newSample)
        {
            double difference = newSample.RawValue - oldSample.RawValue;
            double timeInterval = newSample.TimeStamp100nSec - oldSample.TimeStamp100nSec;
            if (timeInterval != 0) return 100 * (1 - (difference / timeInterval));
            return 0;
        }

        private void RunCounters()
        {
            var pc = new PerformanceCounter("Processor Information", "% Processor Time");
            var cat = new PerformanceCounterCategory("Processor Information");
            var instances = cat.GetInstanceNames().Where(s => !s.ToLower().Contains("total"));
            var cs = new Dictionary<string, CounterSample>();

            foreach (var s in instances)
            {
                Console.WriteLine(s);
                pc.InstanceName = s;
                cs.Add(s, pc.NextSample());
            }

            var values = new List<double>();
            foreach (var s in instances)
            {
                values.Add(0);
            }

            while (countersRunning)
            {
                int i = 0;
                foreach (var s in instances)
                {
                    pc.InstanceName = s;
                    //Console.WriteLine("{0} - {1:f}", s, Calculate(cs[s], pc.NextSample()));
                    double value = Calculate(cs[s], pc.NextSample());
                    if (value < 0)
                    {
                        value = 0;
                    }
                    if (value > 100)
                    {
                        value = 100;
                    }
                    values[i++] = value;
                    cs[s] = pc.NextSample();
                }
                values.Sort();
                values.Reverse();
                lock (__lock)
                {
                    this.Invoke(updateCountersDelegate, values);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void StartStop()
        {
            lock (__lock)
            {
                if (countersRunning)
                {
                    countersRunning = false;
                    RecreateBars(0);
                    workerThread.Join();
                    startStopBtn.Text = "Start";
                    safeModeStartBtn.Visible = true;
                    return;
                }
                startStopBtn.Text = "Stop";
                safeModeStartBtn.Visible = false;
                countersRunning = true;
                workerThread = new Thread(RunCounters);
                workerThread.Start();
            }
        }

        private void startStopBtn_Click(object sender, EventArgs e)
        {
            useSafeMode = false;
            StartStop();
        }

        private delegate void UpdateCountersDelegate(List<Double> counters);
        private UpdateCountersDelegate updateCountersDelegate = null;

        List<ProgressBar> bars = new List<ProgressBar>();
        List<Chart> charts = new List<Chart>();

        bool useSafeMode = false;

        static int CoreWidth = 50;
        static int CoreHeight = 60;

        private void RecreateBars(int numBars)
        {
            foreach(var bar in bars)
            {
                this.Controls.Remove(bar);
            }
            bars.Clear();
            foreach(var chart in charts)
            {
                this.Controls.Remove(chart);
            }
            charts.Clear();
            for (int i=0; i<numBars; ++i)
            {
                ProgressBar bar = null;
                if (useSafeMode)
                {
                    bar = new ProgressBar();
                }
                else
                {
                    bar = new CustomProgressBar();
                }
                this.Controls.Add(bar);
                bars.Add(bar);
                bar.Left = 10 + i * (CoreWidth + 2);
                bar.Top = 10;
                bar.Width = CoreWidth;
                bar.Height = CoreHeight;

                Chart chart = new Chart();
                this.Controls.Add(chart);
                charts.Add(chart);
                chart.Left = 10 + i * (CoreWidth + 2);
                chart.Top = 10 + CoreHeight + 2;
                chart.Width = CoreWidth;
                chart.Height = CoreHeight;
                chart.ChartAreas.Add(new ChartArea());
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                chart.ChartAreas[0].AxisY.Maximum = 100;
                chart.ChartAreas[0].BackColor = Color.DarkGray;
                chart.Series.Add(new Series());
                chart.Series[0].ChartType = SeriesChartType.Area;
                chart.Series[0].IsVisibleInLegend = false;
                chart.Series[0].BorderDashStyle = ChartDashStyle.NotSet;
                chart.Series[0].Color = Color.Cyan;
                
            }
        }

        private void UpdateCounters(List<Double> counters)
        {
            if (bars.Count != counters.Count)
            {
                RecreateBars(counters.Count);
            }

            int i = 0;
            foreach(var v in counters)
            {
                int value = (int)counters[i];
                bars[i].Value = value;
                if (charts[i].Series[0].Points.Count > 59)
                {
                    charts[i].Series[0].Points.RemoveAt(0);
                }
                foreach(var p in charts[i].Series[0].Points)
                {
                    p.XValue--;
                }
                charts[i].Series[0].Points.Add(new DataPoint(60, value));
                ++i;
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (__lock)
            {
                if (workerThread == null)
                {
                    return;
                }
                countersRunning = false;
                RecreateBars(0);
                workerThread.Join();
            }
        }

        private void safeModeStartBtn_Click(object sender, EventArgs e)
        {
            useSafeMode = true;
            StartStop();
        }

        private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = onTopCheckBox.Checked;
        }
    }
}
