using System;
using System.IO;
using System.Net;
using Awesomium.Core;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using M4.WebControlSample.Common;

namespace M4.WebControlSample.Model
{
    internal class Download : ViewModel
    {
        private const string Shell = "explorer.exe";
        private string _file;
        WebClient _client;
        private string _url;

        //DelegateCommand openCommand;
        //DelegateCommand openFolderCommand;

        public Download(string url, string file)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            Url = url;
            FileName = new FileInfo(file).Name;
            _file = file;
        }

        public override int GetHashCode()
        {
            return String.Format("{0}_{1}", _url, _file).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Download)
                return this == (Download)obj;

            return false;
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _file = null;
            _client = null;
        }

        public void Start()
        {
            if (!IsDownloading)
                DownloadFile();
        }

        public void Cancel()
        {
            if (IsDownloading && _client.IsBusy)
                _client.CancelAsync();
        }

        private void DownloadFile()
        {
            if (_client == null)
                _client = new WebClient();

            DownloadProgressChangedEventHandler progressHandler = (sender, e) =>
            {
                if (String.Compare(FileSize, "N/A", false) == 0)
                    FileSize = e.TotalBytesToReceive.GetFileSize();

                Progress = e.ProgressPercentage;
            };

            AsyncCompletedEventHandler completeHandler = null;
            completeHandler = (sender, e) =>
            {
                using (_client)
                {
                    _client.DownloadProgressChanged -= progressHandler;
                    _client.DownloadFileCompleted -= completeHandler;

                    IsCancelled = e.Cancelled;
                    IsDownloading = false;
                    Progress = 0;

                    if (!e.Cancelled && File.Exists(_file))
                    {
                        FileSize = new FileInfo(_file).GetFileSize();
                        IsDownloadComplete = true;
                    }
                }

                _client = null;
            };

            _client.DownloadProgressChanged += progressHandler;
            _client.DownloadFileCompleted += completeHandler;

            try
            {
                _client.DownloadFileAsync(new Uri(Url), _file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FileSize = "N/A";
            IsCancelled = false;
            IsDownloadComplete = false;
            IsDownloading = true;
        }

        public void OpenDownloadedFile()
        {
            if (CanOpenDownloadedFile())
            {
                try
                {
                    Process.Start(_file);
                }
                catch { }
            }
        }

        public void OpenDownloadedFileFolder()
        {
            if (CanOpenDownloadedFile())
            {
                try
                {
                    Process.Start(Shell, String.Format(@"/select, ""{0}""", _file));
                }
                catch { }
            }
        }

        public bool CanOpenDownloadedFile()
        {
            return !IsCancelled && IsDownloadComplete && File.Exists(_file);
        }

        public string Url
        {
            get
            {
                return _url;
            }
            protected set
            {
                if (String.Compare(_url, value, true) == 0)
                    return;

                _url = value;
                RaisePropertyChanged("URL");
            }
        }


        private int _Progress;
        public int Progress
        {
            get
            {
                return _Progress;
            }
            protected set
            {
                if (_Progress == value)
                    return;

                _Progress = value;
                RaisePropertyChanged("Progress");
            }
        }


        private bool _IsDownloading;
        public bool IsDownloading
        {
            get
            {
                return _IsDownloading;
            }
            protected set
            {
                if (_IsDownloading == value)
                    return;

                _IsDownloading = value;
                RaisePropertyChanged("IsDownloading");
            }
        }


        private bool _IsCancelled;
        public bool IsCancelled
        {
            get
            {
                return _IsCancelled;
            }
            protected set
            {
                if (_IsCancelled == value)
                    return;

                _IsCancelled = value;
                RaisePropertyChanged("IsCancelled");
            }
        }


        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            protected set
            {
                if (String.Compare(_FileName, value, true) == 0)
                    return;

                _FileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        private string _FileSize;
        public string FileSize
        {
            get
            {
                return _FileSize;
            }
            protected set
            {
                if (String.Compare(_FileSize, value, true) == 0)
                    return;

                _FileSize = value;
                RaisePropertyChanged("FileSize");
            }
        }

        private bool _IsDownloadComplete;
        public bool IsDownloadComplete
        {
            get
            {
                return _IsDownloadComplete;
            }
            protected set
            {
                if (_IsDownloadComplete == value)
                    return;

                _IsDownloadComplete = value;
                RaisePropertyChanged("IsDownloadComplete");
            }
        }

        public static bool operator ==(Download d1, Download d2)
        {
            if (ReferenceEquals(d1, null))
                return ReferenceEquals(d2, null);

            if (ReferenceEquals(d2, null))
                return ReferenceEquals(d1, null);

            return String.Compare(d1.Url, d2.Url, true) == 0 &&
                String.Compare(d1._file, d2._file, true) == 0;
        }

        public static bool operator !=(Download d1, Download d2)
        {
            return !(d1 == d2);
        }
    }
}
