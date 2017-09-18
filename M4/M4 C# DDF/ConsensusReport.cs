using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M4
{
  class ConsensusReport
  {

    public delegate void ProgressCallbackHandler(object sender, int totalPoints, int currentPoint, ref bool initiateStop);

    public ProgressCallbackHandler ProgressCallback;

    public int ResultCount = 1000;

    public ConsensusReport()
    {

    }


    public void RunConsensusReport()
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
