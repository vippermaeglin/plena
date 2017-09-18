/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M4.M4v2.Chart;
using Nevron.UI;
using Nevron.UI.WinForm;
using Nevron.UI.WinForm.Controls;
using Nevron.UI.WinForm.Docking;
using M4.AsyncOperations;

namespace M4
{
    public partial class frmNN : NForm
    {

        #region Members, Initialization and Form

        private AsyncOperation _asyncOp;
        bool _running = false;


        public frmNN()
        {
            _asyncOp = AsyncHelper.CreateOperation();
            InitializeComponent();
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            Run();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Load available technical indicators
        private void frmNN_Load(object sender, EventArgs e)
        {
            //ctlChart chart = frmMain.GInstance.m_ActiveChart;
            CtlPainelChart chart = frmMain.GInstance.MActiveChart;
            if (chart == null) return;

            lstAvailable.Items.Clear();
            lstSelected.Items.Clear();

            List<string> series = chart.GetSeries();
            lstAvailable.Items.AddRange(series.ToArray());

        }

        // Add selected indicators
        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstAvailable.SelectedItem == null) return;
            lstSelected.Items.Add(lstAvailable.SelectedItem);
            lstAvailable.Items.Remove(lstAvailable.SelectedItem);
            cmdAdd.Enabled = lstAvailable.Items.Count != 0;
            cmdRemove.Enabled = lstSelected.Items.Count != 0;
            DialogResult = DialogResult.None;
        }

        // Remove selected indicators
        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lstSelected.SelectedItem == null) return;
            lstAvailable.Items.Add(lstSelected.SelectedItem);
            lstSelected.Items.Remove(lstSelected.SelectedItem);
            cmdAdd.Enabled = lstAvailable.Items.Count != 0;
            cmdRemove.Enabled = lstSelected.Items.Count != 0;
            DialogResult = DialogResult.None;
        }

        #endregion

        #region Processing


        void Run()
        {

            if (_running)
            {
                MessageBox.Show(
                  "The neural network is training. Wait until done.",
                  "Already running",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
                return;
            }

            ProgressWorkerParams workerParams = new ProgressWorkerParams { Alignment = System.Drawing.ContentAlignment.BottomLeft, AllowCancel = true, ControlAnchor = frmMain.GInstance.MActiveChart, Modal = true, };
            ProgressWorker.Run(
              workerParams,
              visualizer =>
              {
                  try
                  {
                      visualizer.SetProgressTitle("Neural Network");
                      visualizer.SetProgressAction("Initializing...");

                      AsyncNeuralNetwork worker = new AsyncNeuralNetwork();
                      bool first = true;
                      worker.ProgressCallback += (object o, int records, int record, ref bool stop) =>
                      {
                          if (first)
                          {
                              first = false;
                              visualizer.InitProgress(0, records);
                          }
                          visualizer.ReportProgress(record);
                          stop = visualizer.CancelReqested;
                      };


                      visualizer.SetProgressAction("Training Neural Network...");

                      List<string> items = new List<string>();
                      foreach (NListBoxItem item in lstSelected.Items)
                          items.Add(item.Text);

                      // Do the work
                      worker.DoWork(items);

                      // Exit if no report            
                      if (worker.ResultCount < 1 || visualizer.CancelReqested) return; // No work to report

                      visualizer.SetProgressAction("Completing...");
                      visualizer.InitProgress(0, worker.ResultCount);

                      for (int n = 0; n < worker.ResultCount; ++n)
                      {
                          visualizer.ReportProgress(n);
                          if (visualizer.CancelReqested)
                              break;
                      }

                  }
                  catch (Exception ex)
                  {
                      _asyncOp.Post(
                        () =>
                        MessageBox.Show(
                          "An error occured. Error :" + Environment.NewLine + ex.Message,
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error));
                  }
                  finally
                  {
                      _running = false;
                  }
              },
              () =>
              { });


        }

        private void CreateAsyncOp()
        {
            _asyncOp = AsyncHelper.CreateOperation();
        }

        #endregion

    }

    #region Async Processing

    class AsyncNeuralNetwork
    {

        public delegate void NeuralNetProgressCallbackHandler(object sender, int totalPoints, int currentPoint, ref bool initiateStop);

        public NeuralNetProgressCallbackHandler ProgressCallback;

        public int ResultCount = 0;

        public AsyncNeuralNetwork()
        {
        }


        /// <summary>
        /// Runs the neural network.
        /// </summary>
        public void DoWork(List<string> seriesNames)
        {

            ResultCount = seriesNames.Count + 1;
            bool cancel = false;

            List<double> ret = new List<double>();
            //ctlChart chart = frmMain.GInstance.m_ActiveChart;
            CtlPainelChart chart = frmMain.GInstance.MActiveChart;
            if (chart == null) return;

            int count = chart.StockChartX1.RecordCount;
            double[] sum = new double[count];

            // Run the neural network
            int current = 0;
            foreach (string item in seriesNames)
            {

                current++;
                if (ProgressCallback != null)
                    ProgressCallback(this, ResultCount, current, ref cancel);

                if (cancel) return;
                List<double> values = ProcessIndicator(item);
                for (int n = 0; n < count; ++n)
                    sum[n] += values[n];
            }

            for (int n = 0; n < sum.Length; ++n)
            {
                sum[n] /= seriesNames.Count;
                sum[n] = sum[n] * 1000;
            }

            if (ProgressCallback != null)
                ProgressCallback(this, ResultCount, current + 1, ref cancel);

            // Display the results in StockChartX
            chart.StockChartX1.RemoveSeries("Neural Network");
            int panel = chart.StockChartX1.AddChartPanel();
            chart.StockChartX1.AddSeries("Neural Network", STOCKCHARTXLib.SeriesType.stLineChart, panel);

            chart.StockChartX1.Freeze(true);
            for (int n = 0; n < count; ++n)
            {
                if (cancel) return;
                double jdate = chart.StockChartX1.GetJDate(chart.StockChartX1.Symbol + ".close", n + 1);
                double value = sum[n];
                if (value == 0) value = (double)STOCKCHARTXLib.DataType.dtNullValue;
                chart.StockChartX1.AppendValue("Neural Network", jdate, value);

            }
            chart.StockChartX1.Freeze(false);
            chart.StockChartX1.Update();

        }


        /// <summary>
        /// Processes the indicator.
        /// </summary>
        /// <param name="Name">The indicator name.</param>
        /// <returns></returns>
        private List<double> ProcessIndicator(string Name)
        {
            List<double> ret = new List<double>();
            //ctlChart chart = frmMain.GInstance.m_ActiveChart;
            CtlPainelChart chart = frmMain.GInstance.MActiveChart;
            if (chart == null) return ret;

            int count = chart.StockChartX1.RecordCount;
            NeuralNetwork nn = new NeuralNetwork();
            List<double> values = new List<double>(count);
            for (int n = 1; n < count + 1; ++n)
                values.Add(chart.StockChartX1.GetValue(Name, n));

            // TODO: Optionally you could allow the end user to set these parameters.
            int periods = 8;
            double learningRate = 1.5;
            int epochs = 5000;
            ret = nn.NeuralIndicator(values, periods, learningRate, epochs, 0.5);

            return ret;
        }


    }

    #endregion

}