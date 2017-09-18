/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.ComponentModel;
using System.Threading;

namespace M4.AsyncOperations
{
  /// <summary>
  /// Helper object to work with async operation
  /// </summary>
  public static class AsyncHelper
  {
    public static AsyncOperation CreateOperation()
    {      
      return AsyncOperationManager.CreateOperation(null);
    }

    public static void Post(this AsyncOperation asyncOp, Action runner)
    {
      asyncOp.Post(state => runner(), null);
    }

    public static void PostOperationCompleted(this AsyncOperation asyncOp, Action runner)
    {
      asyncOp.PostOperationCompleted(state => runner(), null);
    }

    public static void Send(this AsyncOperation asyncOp, Action runner)
    {
      asyncOp.SynchronizationContext.Send(state => runner(), null);
    }

    public static void RunAsync(Action<AsyncOperation> runner)
    {
      if (runner == null)
        throw new ArgumentNullException("runner");

      var asyncOp = CreateOperation();
        int max = 0;
        int ports = 0;
        ThreadPool.GetMaxThreads(out max, out ports);
        if(max<100)ThreadPool.SetMaxThreads(100, 100);
      ThreadPool.QueueUserWorkItem(state => runner(asyncOp));
    }

    public static void RunAsync(Action<AsyncOperation> runner, Action completeHandler)
    {
      if (runner == null)
        throw new ArgumentNullException("runner");
      if (completeHandler == null)
        throw new ArgumentNullException("completeHandler");

      var asyncOp = CreateOperation();
        int max = 0;
        int ports = 0;
        ThreadPool.GetMaxThreads(out max, out ports);
        if(max<100)ThreadPool.SetMaxThreads(100, 100);
      ThreadPool.QueueUserWorkItem(
        state =>
          {
            try
            {
              runner(asyncOp);
            }
            finally
            {
              asyncOp.PostOperationCompleted(completeHandler);
            }
          });
    }

    public static void RunAsyncSTA(Action<AsyncOperation> runner, Action completeHandler)
    {
      if (runner == null)
        throw new ArgumentNullException("runner");
      if (completeHandler == null)
        throw new ArgumentNullException("completeHandler");

      var asyncOp = CreateOperation();
      Thread th = new Thread(() =>
                               {
                                 try
                                 {
                                   runner(asyncOp);
                                 }
                                 finally
                                 {
                                   asyncOp.PostOperationCompleted(completeHandler);
                                 }
                               });
      th.SetApartmentState(ApartmentState.STA);
      th.IsBackground = true;
      th.Start();
    }
  }
}
