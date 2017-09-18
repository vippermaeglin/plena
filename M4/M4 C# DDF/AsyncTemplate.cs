/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M4.AsyncOperations;

namespace M4
{


  // This class can be used as a template for adding new asynchronous operations to M4.


  class AsyncTemplate
  {


    private AsyncOperation _asyncOp;
    bool _running = false;


    public AsyncTemplate()
    {
      _asyncOp = AsyncHelper.CreateOperation();
    }
    

    void UseThisFunctionToDoTheAsyncWork()
    {

      if (_running)
      {
        MessageBox.Show(
          "The work is in progress. Wait until done.",
          "Already running",
          MessageBoxButtons.OK,
          MessageBoxIcon.Exclamation);
        return;
      }

      ProgressWorkerParams workerParams = new ProgressWorkerParams { Alignment = System.Drawing.ContentAlignment.BottomLeft, AllowCancel = true, ControlAnchor = this, Modal = true, };
      ProgressWorker.Run(
        workerParams,
        visualizer =>
        {
          try
          {
            visualizer.SetProgressTitle("Feature name goes here");
            visualizer.SetProgressAction("Working...");

            YourWorkerClass worker = new YourWorkerClass();
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


            visualizer.SetProgressAction("Initializing...");            

            // Do the work
            worker.DoWork();

            // Exit if no report            
            if (worker.ResultCount < 1 || visualizer.CancelReqested) return; // No work to report
            
            visualizer.SetProgressAction("Completing...");
            visualizer.InitProgress(0, worker.ResultCount);

            for(int n = 0; n < worker.ResultCount; ++n)
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



  }



  class YourWorkerClass
  {

    public delegate void ProgressCallbackHandler(object sender, int totalPoints, int currentPoint, ref bool initiateStop);

    public ProgressCallbackHandler ProgressCallback;

    public int ResultCount = 1000;

    public YourWorkerClass()
    {

    }


    public void DoWork()
    {

      bool cancel = false;

      for (int n = 0;  !cancel && n < 1000; ++n)
      {

        if (ProgressCallback != null)
        {
          ProgressCallback(this, ResultCount, n, ref cancel);
          System.Threading.Thread.Sleep(5);          
        }

      }

    }
  }



}