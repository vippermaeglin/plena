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

namespace M4
{  

  class NeuralNetwork
  {    

    private double m_LearningRate;

    //Neurons 
    double Neuron1;
    double Neuron2;
    double Neuron4;
    double Neuron3;
    double Neuron5;
    double Neuron6;

    //Weights 
    double W14;
    double W13;
    double W15;
    double W24;
    double W23;
    double W25;
    double W46;
    double W36;
    double W56;

    //Biases 
    double B4;
    double B3;
    double B5;
    double B6;

    private void initialize()
    {
      Random rnd = new Random();
      W13 = (rnd.NextDouble() * -2) + 1;
      W14 = (rnd.NextDouble() * -2) + 1;
      W15 = (rnd.NextDouble() * -2) + 1;
      W23 = (rnd.NextDouble() * -2) + 1;
      W24 = (rnd.NextDouble() * -2) + 1;
      W25 = (rnd.NextDouble() * -2) + 1;
      W36 = (rnd.NextDouble() * -2) + 1;
      W46 = (rnd.NextDouble() * -2) + 1;
      W56 = (rnd.NextDouble() * -2) + 1;
      B3 = (rnd.NextDouble() * -2) + 1;
      B4 = (rnd.NextDouble() * -2) + 1;
      B5 = (rnd.NextDouble() * -2) + 1;
      B6 = (rnd.NextDouble() * -2) + 1;
    }

    private void train(double Input1, double Input2, double Target)
    {
      double OutErr = 0;
      double HiddenErr = 0;
      feedForward(Input1, Input2);
      OutErr = (Target - Neuron6) * Neuron6 * (1 - Neuron6);
      W36 = W36 + m_LearningRate * OutErr * Neuron3;
      W46 = W46 + m_LearningRate * OutErr * Neuron4;
      W56 = W56 + m_LearningRate * OutErr * Neuron5;
      HiddenErr = Neuron3 * (1 - Neuron3) * OutErr * W36;
      W13 = W13 + m_LearningRate * HiddenErr * Neuron1;
      W23 = W23 + m_LearningRate * HiddenErr * Neuron2;
      HiddenErr = Neuron4 * (1 - Neuron4) * OutErr * W46;
      W14 = W14 + m_LearningRate * HiddenErr * Neuron1;
      W24 = W24 + m_LearningRate * HiddenErr * Neuron2;
      HiddenErr = Neuron5 * (1 - Neuron5) * OutErr * W56;
      W15 = W15 + m_LearningRate * HiddenErr * Neuron1;
      W25 = W25 + m_LearningRate * HiddenErr * Neuron2;

    }

    private double feedForward(double Input1, double Input2)
    {
      Neuron1 = Input1;
      Neuron2 = Input2;
      Neuron3 = activation((Neuron1 * W13) + (Neuron2 * W23) + B3);
      Neuron4 = activation((Neuron1 * W14) + (Neuron2 * W24) + B4);
      Neuron5 = activation((Neuron1 * W15) + (Neuron2 * W25) + B5);
      Neuron6 = activation((Neuron3 * W36) + (Neuron4 * W46) + (Neuron5 * W56) + B6);
      return Neuron6;
    }

    private double activation(double Value)
    {      
      return 1 / (1 + System.Math.Exp((double)Value * -1));
    }

    public List<double> NeuralIndicator(List<double> Source, int Periods, double LearningRate, int Epochs, double PercentTrain)
    {

      int iRecordCount = Source.Count;
      
      //Field NNOutput = new Field(iRecordCount, "NeuralIndicator");
      double[] NNOutput = new double[iRecordCount];
      
      //Field fInput1 = new Field(iRecordCount, "Input1");
      double[] fInput1 = new double[iRecordCount];

      //Field fInput2 = new Field(iRecordCount, "Input2");
      double[] fInput2 = new double[iRecordCount];

      //Field fTarget = new Field(iRecordCount, "Target");
      double[] fTarget = new double[iRecordCount];

      //General G = new General();
      //Note Nt = default(Note);
      int Record = 0;
      int TrainRecords = 0;
      int Epoch = 0;
      int Start = 0;
      double Input1 = 0;
      double Input2 = 0;
      double Target = 0;
      double Max = 0;
      double Min = 0;

      if (PercentTrain > 0.98 | PercentTrain < 0.2)
        throw new Exception("Invalid PercentTrain");

      //Divide Source into training set and forecast set 
      TrainRecords = (int)((double)iRecordCount * PercentTrain);

      //Build neural network data sets 
      Start = Periods + 1;
      int position = Start;

      for (Record = Start; Record < iRecordCount; Record++)
      {

        Input1 = Source[position - Periods];
        Input2 = Source[position];

        fInput1[position] = Input1;
        fInput2[position] = Input2;

        position++;
      }

      //Training set target values 
      Start = Periods + 1;
      position = Start;
      for (Record = Start; Record <= TrainRecords - Periods; Record++)
      {

        Target = Source[position] - Source[position + 1];

        fTarget[position] = Target;

        position++;
      }

      //Normalize vectors 
      Max = MaxValueFromInterval(fInput1, 1, iRecordCount);      
      Min = MinValueFromInterval(fInput1, 1, iRecordCount);      
      for (Record = 1; Record < iRecordCount; Record++)
      {
        fInput1[Record] = normalize(Max, Min, fInput1[Record]);
      }

      Max = MaxValueFromInterval(fInput2, 1, iRecordCount);
      Min = MinValueFromInterval(fInput2, 1, iRecordCount);
      for (Record = 1; Record < iRecordCount; Record++)
      {
        fInput2[Record] = normalize(Max, Min, fInput2[Record]);
      }

      Max = MaxValueFromInterval(fTarget, 1, iRecordCount);
      Min = MinValueFromInterval(fTarget, 1, iRecordCount);
      for (Record = 1; Record <= TrainRecords; Record++)
      {
        fTarget[Record] = normalize(Max, Min, fTarget[Record]);
      }

      Start = TrainRecords;

      //Initialize neural network 
      m_LearningRate = LearningRate;
      initialize();

      //Train neural network 
      for (Epoch = 1; Epoch <= Epochs; Epoch++)
      {
        for (Record = 1; Record <= TrainRecords; Record++)
        {
          train(fInput1[Record], fInput2[Record], fTarget[Record]);          
        }
        // TODO: Update progress here
        System.Windows.Forms.Application.DoEvents();
      }

      //Output neural network forecasts from TrainRecords + 1 to RecordCount 
      for (Record = TrainRecords; Record < iRecordCount; Record++)
      {
        NNOutput[Record] = feedForward(fInput1[Record], fInput2[Record]);
      }
      
      return new List<double>(NNOutput);

    }

    private double maxVal(double Value1, double Value2)
    {
      double functionReturnValue = 0;
      if (Value1 > Value2)
      {
        functionReturnValue = Value1;
      }
      else if (Value2 > Value1)
      {
        functionReturnValue = Value2;
      }
      return functionReturnValue;
    }

    private double minVal(double Value1, double Value2)
    {
      double functionReturnValue = 0;
      if (Value1 < Value2)
      {
        functionReturnValue = Value1;
      }
      else if (Value2 < Value1)
      {
        functionReturnValue = Value2;
      }
      return functionReturnValue;
    }

    private double normalize(double Max, double Min, double Value)
    {
      if (Max == Min) return 0;

      return (Value - Min) / (Max - Min);
    }

    private double MaxValueFromInterval(double[] Source, int StartPeriod, int EndPeriod)
    {
      if (Source.Length < 1) return 0;
      int Record;
      double value = 0;
      if (StartPeriod > EndPeriod) return Source[0];
      for (Record = StartPeriod; Record < EndPeriod; Record++)
      {
        if (Source[Record] <= value) continue;
        value = Source[Record];        
      }
      return value;
    }

    private double MinValueFromInterval(double[] Source, int StartPeriod, int EndPeriod)
    {
      if (Source.Length < 1) return 0;
      int Record;
      double value = 0;
      if (StartPeriod > EndPeriod) return Source[0];
      for (Record = StartPeriod; Record < EndPeriod; Record++)
      {
        if (Source[Record] >= value) continue;
        value = Source[Record];
      }
      return value;
    }
 

  }
}