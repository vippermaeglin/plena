using System;
using System.Linq;
using System.Windows.Forms;
using M4.Properties;
using M4.WebControlSample.Model;

namespace M4.WebControlSample
{
    partial class BrowserForm : UserControl
    {
        private HistoryForm _history;
        private DownloadsForm _downloadsWindow;
        private readonly DownloadCollection _downloads;

        public BrowserForm()
        {
            // See Program.cs for WebCore initialization.

            InitializeComponent();

            _downloads = new DownloadCollection();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Initialize UI.
            if (Settings.Default.History)
                History.Show(dockPanel);

            if (Settings.Default.Downloads)
                DownloadsWindow.Show(dockPanel);

            // Create the initial tab.
            OpenTab();

            // Refresh history entries.
            History.RefreshSource();
        }

        //protected override void OnFormClosing( FormClosingEventArgs e )
        //{
        //    // Hide during cleanup.
        //    Hide();

        //    // Close the views and perform cleanup for every tab.
        //    List<Form> contents = new List<Form>( dockPanel.Contents.OfType<Form>() );

        //    foreach ( Form content in contents )
        //        content.Close();

        //    // Save application settings.
        //    Settings.Default.Save();

        //    base.OnFormClosing( e );
        //}

        //protected override void OnFormClosed( FormClosedEventArgs e )
        //{
        //    base.OnFormClosed( e );

        //    // We may not be the last window open.
        //    if ( Application.OpenForms.OfType<BrowserForm>().Count() == 0 )
        //        WebCore.Shutdown();
        //}

        internal WebDocument OpenTab(string url = null, string title = null)
        {
            // - A tab with no url specified, will open WebCore.HomeURL.
            // - A tab with a predefined title, will not display a toolbar
            //   and address-box. This is used to display fixed web content
            //   such as the Help Contents.
            WebDocument doc = String.IsNullOrEmpty(url) ? new WebDocument() :
                String.IsNullOrEmpty(title) ? new WebDocument(url) : new WebDocument(url, title);
            doc.BrowserForm = this;
            doc.Show(dockPanel);

            return doc;
        }

        public void DownloadFile(string url, string file)
        {
            // Create a download item.
            Download download = new Download(url, file);
            // If the same file had previously been downloaded,
            // let the old one assume the identity of the new.
            Download existingDownload = _downloads.SingleOrDefault(d => d == download);

            // Show the downloads bar.
            //DownloadsVisible = true;

            if (existingDownload != null)
                download = existingDownload;
            else
                _downloads.Add(download);

            // Start downloading.
            download.Start();
            // Show the Downloads window.
            DownloadsWindow.Show(dockPanel);
        }

        public HistoryForm History
        {
            get
            {
                return _history ?? (_history = new HistoryForm { BrowserForm = this });
            }
        }

        public DownloadsForm DownloadsWindow
        {
            get { return _downloadsWindow ?? (_downloadsWindow = new DownloadsForm(this)); }
        }

        public string Status
        {
            get
            {
                return statusLabel.Text;
            }
            set
            {
                if (String.Compare(statusLabel.Text, value, false) == 0)
                    return;

                statusLabel.Text = value;
            }
        }

        public string Title
        {
            get
            {
                return Text;
            }
            set
            {
                Text = String.Format("{0} - {1}", Application.ProductName, value);
            }
        }

        public bool ShowProgress
        {
            get { return progressBar.Visible; }
            set { progressBar.Visible = value; }
        }

        public DownloadCollection Downloads
        {
            get
            {
                return _downloads;
            }
        }

        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            foreach (WebDocument doc in dockPanel.Documents)
                // We pause WebControl rendering for documents
                // that are currently not visible.
                doc.IsRendering = (doc == dockPanel.ActiveDocument);

            if (dockPanel.ActiveDocument != null)
            {
                WebDocument doc = (WebDocument)dockPanel.ActiveDocument;
                Text = String.Format("{0} - {1}", Application.ProductName, doc.Text);
                doc.Focus();
            }
            else
                Text = Application.ProductName;
        }
    }
}
