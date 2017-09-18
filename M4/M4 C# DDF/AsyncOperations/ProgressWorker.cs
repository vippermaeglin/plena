/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;

namespace M4.AsyncOperations
{
	using System.Windows.Forms;

	public static class ProgressWorker
	{
		public static void Run(ProgressWorkerParams workerParams, Action<IProgressVisualizer> work)
		{
			Run(workerParams, work, null);
		}
    
		public static void Run(ProgressWorkerParams workerParams, Action<IProgressVisualizer> work, Action completeHandler)
		{
			if (work == null)
			{
				throw new ArgumentNullException("work");
			}

			IProgressVisualizer progressVisualizer = null;

			AsyncHelper.RunAsync(
				operation =>
					{
						progressVisualizer = (new DefaultProgressService()).CreateVisualizer(workerParams);

						work(progressVisualizer);
					},
					() =>
						{
							progressVisualizer.Complete();
							if (completeHandler != null)
							{
								completeHandler();
							}
						});
			Application.DoEvents();
		}

    /// <summary>
    /// Runs an asynchronious operation using a STA thread
    /// </summary>
    /// <param name="workerParams"></param>
    /// <param name="work"></param>
    /// <param name="completeHandler"></param>
    public static void RunSTA(ProgressWorkerParams workerParams, Action<IProgressVisualizer> work, Action completeHandler)
    {
      if (work == null)
      {
        throw new ArgumentNullException("work");
      }

      IProgressVisualizer progressVisualizer = null;

      AsyncHelper.RunAsyncSTA(
        operation =>
          {
            progressVisualizer = (new DefaultProgressService()).CreateVisualizer(workerParams);

            work(progressVisualizer);
          },
        () =>
          {
            progressVisualizer.Complete();
            if (completeHandler != null)
            {
              completeHandler();
            }
          });
      Application.DoEvents();
    }
	}
}
