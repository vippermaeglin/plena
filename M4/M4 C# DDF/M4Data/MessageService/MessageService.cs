using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Windows.Forms;
using M4Core.Entities;

namespace M4Data.MessageService
{
    public enum MSRequestOwner
    {
        M4,
        DServer
        // (...)
    }

    public enum MSRequestType
    {
        //M4:
        GetUserLogin,
        MessageStatus,

        //DServer:
        GetSymbolsList,
        GetHistoricalData, // new object[]{string Symbol, int Periodicity}
        GetRealTimeData,
        SummaryCreated
    }

    public enum MSMessageStatusType 
    { 
        UpdatedAll,
        InsertedSymbol,
        InsertedSymbols,
        UpdatedSymbol,
        ReloadedSymbol,
        DeletedSymbol,
        SystemMessage
    }


    public enum MSRequestStatus
    {
        Pending,
        Processing,
        Done,
        Failed
    }

    public class MSRequest
    {
        public string ID;
        public MSRequestStatus MSStatus;
        public MSRequestType MSType;
        public MSRequestOwner MSOwner;
        public object[] MSParams;
        public MSRequest(string id,MSRequestStatus status, MSRequestType type,MSRequestOwner owner, object[] msparams)
        {
            ID = id;
            MSStatus = status;
            MSType = type;
            MSOwner = owner;
            MSParams = msparams;
        }
    }
    
    public class MessageService
    {
        private static MessageService _instance;
        private static ConcurrentQueue<MSRequest> _mainRequests = new ConcurrentQueue<MSRequest>();
        private static ConcurrentQueue<MSRequest> _serverRequests = new ConcurrentQueue<MSRequest>();

        public static MessageService Instance()
        {
            return _instance ?? (_instance = new MessageService());
        }
        
        public static void SubmitRequest(MSRequest Request)
        {
            switch (Request.MSType)
            {
                case MSRequestType.MessageStatus:
                    _mainRequests.Enqueue(Request);
                    break;
                case MSRequestType.GetUserLogin:
                    _mainRequests.Enqueue(Request);
                    break;
                case MSRequestType.GetSymbolsList:
                    if (Request.MSStatus == MSRequestStatus.Pending)
                    {
                        _serverRequests.Enqueue(Request);
                    }
                    else
                    {
                        //Request already exists?
                        if(!_mainRequests.Contains(Request))_mainRequests.Enqueue(Request);
                    }
                    break;
                case MSRequestType.GetHistoricalData:
                    if(Request.MSStatus == MSRequestStatus.Pending)
                    {
                        try
                        {
                            _serverRequests.Enqueue(Request);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("MessageService():GetHistoricalData: "+ exception.Message);
                        }
                        
                    }
                    else
                    {
                        _mainRequests.Enqueue(Request);
                    }
                    break;
            }
        }

        public static Queue<MSRequest> GetRequest(MSRequestOwner Owner)
        {
            Queue<MSRequest> requests = new Queue<MSRequest>();
            MSRequest outRequest;
            switch(Owner)
            {
                case MSRequestOwner.M4:
                    while(_mainRequests.TryDequeue(out outRequest)) requests.Enqueue(outRequest);
                    break;
                case MSRequestOwner.DServer:
                    while (_serverRequests.TryDequeue(out outRequest)) requests.Enqueue(outRequest);
                    break;
            }
            return requests;
        }

    }
    
}
