/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace M4
{
  public partial class ctlForex : UserControl
  {
    private readonly List<ctlForexPair> m_Pairs = new List<ctlForexPair>();

    public ctlForex()
    {
      InitializeComponent();
    }

    //Clears any existing pairs and initializes new pairs
    public void InitializePairs(List<string> Pairs)
    {
      //Clear pairs
      int n;
      for (n = 0; n < m_Pairs.Count; n++)
      {
        m_Pairs[n] = null;
      }

      //Add new pairs        
      for (n = 0; n < Pairs.Count; n++)
      {
        ctlForexPair pair = new ctlForexPair(Pairs[n]) { Parent = this, Visible = true };
        m_Pairs.Add(pair);
      }
    }

    //Order pairs in rows and columns
    private void ctlForex_Resize(object sender, EventArgs e)
    {
      int x = 0;
      int y = 0;
      for (short n = 0; n < m_Pairs.Count; n++)
      {
        if ((x + m_Pairs[n].Width) > Width)
        {
          y += m_Pairs[n].Height;
          x = 0;
        }
        m_Pairs[n].Left = x;
        m_Pairs[n].Top = y;
        x += m_Pairs[n].Width;
      }
    }

    //Returns a pair so the caller may update the values
    public ctlForexPair GetPair(string PairName)
    {
      for (short n = 0; n < m_Pairs.Count; n = (short)(n + 1))
      {
        if (m_Pairs[n].lblPairName.Text == PairName)
        {
          return m_Pairs[n];
        }
      }
      return null;
    }

    //TODO: hookup your real-time forex feed and remove this timer.
    //This simulation timer is for example purposes only.
    private readonly Random _r = new Random();
    private void tmrSimulate_Tick(object sender, EventArgs e)
    {
      if (m_Pairs.Count < 13)
      {
        m_Pairs.Add(new ctlForexPair("EUR/USD"));
        m_Pairs.Add(new ctlForexPair("USD/JPY"));
        m_Pairs.Add(new ctlForexPair("GBP/USD"));
        m_Pairs.Add(new ctlForexPair("USD/CHF"));
        m_Pairs.Add(new ctlForexPair("USD/CAD"));
        m_Pairs.Add(new ctlForexPair("AUD/USD"));
        m_Pairs.Add(new ctlForexPair("EUR/JPY"));
        m_Pairs.Add(new ctlForexPair("EUR/CHF"));
        m_Pairs.Add(new ctlForexPair("GPB/JPY"));
        m_Pairs.Add(new ctlForexPair("GPB/CHF"));
        m_Pairs.Add(new ctlForexPair("CHF/JPY"));
        m_Pairs.Add(new ctlForexPair("NZD/USD"));
        m_Pairs.Add(new ctlForexPair("USD/ZAR"));        
      }

        m_Pairs[0].Name = "EUR/USD";
        m_Pairs[0].UpdateBid(1.3656, 100);
        m_Pairs[0].UpdateAsk(1.3658, 105);

        m_Pairs[1].Name = "USD/JPY";
        m_Pairs[1].UpdateBid(96.441, 80);
        m_Pairs[1].UpdateAsk(96.447, 95);

        m_Pairs[2].Name = "GBP/USD";
        m_Pairs[2].UpdateBid(1.4503, 120);
        m_Pairs[2].UpdateAsk(1.4509, 130);

        m_Pairs[3].Name = "USD/CHF";
        m_Pairs[3].UpdateBid(1.1241, 60);
        m_Pairs[3].UpdateAsk(1.1244, 85);

        m_Pairs[4].Name = "USD/CAD";
        m_Pairs[4].UpdateBid(1.2334, 110);
        m_Pairs[4].UpdateAsk(1.2335, 135);

        m_Pairs[5].Name = "AUD/USD";
        m_Pairs[5].UpdateBid(0.6955, 45);
        m_Pairs[5].UpdateAsk(0.6958, 60);

        m_Pairs[6].Name = "EUR/JPY";
        m_Pairs[6].UpdateBid(131.67, 45);
        m_Pairs[6].UpdateAsk(131.7, 60);

        m_Pairs[7].Name = "EUR/CHF";
        m_Pairs[7].UpdateBid(1.5352, 85);
        m_Pairs[7].UpdateAsk(1.5351, 95);

        m_Pairs[8].Name = "GPB/JPY";
        m_Pairs[8].UpdateBid(139.83, 70);
        m_Pairs[8].UpdateAsk(139.91, 40);

        m_Pairs[9].Name = "GPB/CHF";
        m_Pairs[9].UpdateBid(1.6304, 50);
        m_Pairs[9].UpdateAsk(1.6317, 45);

        m_Pairs[10].Name = "CHF/JPY";
        m_Pairs[10].UpdateBid(85.7, 30);
        m_Pairs[10].UpdateAsk(85.79, 25);

        m_Pairs[11].Name = "NZD/USD";
        m_Pairs[11].UpdateBid(0.5673, 50);
        m_Pairs[11].UpdateAsk(0.5642, 45);

        m_Pairs[12].Name = "USD/ZAR";
        m_Pairs[12].UpdateBid(9.5422, 80);
        m_Pairs[12].UpdateAsk(0.5547, 90);

        for(int n = 0; n < m_Pairs.Count; ++n)
        {
            if(_r.NextDouble() > 0.5)            
                m_Pairs[n].UpdateBid(m_Pairs[n].GetLastBidValue() - 0.0001, m_Pairs[n].GetLastBidSize());            
            else
                m_Pairs[n].UpdateBid(m_Pairs[n].GetLastBidValue() + 0.0001, m_Pairs[n].GetLastBidSize());
            
            if(_r.NextDouble() > 0.5)            
                m_Pairs[n].UpdateAsk(m_Pairs[n].GetLastAskValue() + 0.0001, m_Pairs[n].GetLastAskSize());            
            else            
                m_Pairs[n].UpdateAsk(m_Pairs[n].GetLastAskValue() - 0.0001, m_Pairs[n].GetLastAskSize());            
        }

    }

  }
}
