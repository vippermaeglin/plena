using System;
using System.IO;
using System.Net;
using System.Drawing;
using Awesomium.Core;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace M4.WebControlSample
{
    partial class WebDocument : DockContent
    {
        private const String JsFavicon = "(function(){links = document.getElementsByTagName('link'); wHref=window.location.protocol + '//' + window.location.hostname + '/favicon.ico'; for(i=0; i<links.length; i++){s=links[i].rel; if(s.indexOf('icon') != -1){ wHref = links[i].href }; }; return wHref; })();";
        private readonly bool _goToHome;
        private readonly bool _fixedUrl;

        // For this application we enable self-update (see WebControl.SelfUpdate). 
        // The Windows Forms WebControl does not internally control the IsRendering property 
        // but since we are displaying our controls in a tabbed dock-manager,
        // we can pause and resume rendering ourselves.
        // (See documentation of: SelfUpdate, IsRendering, OnEnabledChanged)

        public WebDocument()
        {
            InitializeComponent();
            _goToHome = true;
        }

        public WebDocument(string url)
        {
            InitializeComponent();

            webControl.Source = new Uri(url);
        }

        public WebDocument(string url, string title)
        {
            InitializeComponent();

            webControl.Source = new Uri(url);
            _fixedUrl = true;
            Text = title;
            pageToolStrip.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_goToHome)
                webControl.GoToHome();
        }

        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);

            switch (DockState)
            {
                case DockState.Hidden:
                    // Pause rendering when this window is hidden.
                    webControl.Enabled = false;
                    break;

                case DockState.Document:
                    // A non-activated document, should be covered
                    // by others since it is displayed in a tab-control;
                    // we can safely pause rendering when it is not activated.
                    // This is not the case with tool-windows docked on
                    // the sides of the main window that may be visible while not activated;
                    // this is why we default to true for all other scenarios below.
                    webControl.Enabled = IsActivated;
                    break;

                default:
                    webControl.Enabled = true;
                    break;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            // Transfer focus to the control when the
            // tab acquires it.
            webControl.Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // When the hosting UI has been destroyed,
            // close the WebControl to properly destroy its
            // underlying web-view and dispose resources.
            webControl.Close();
            base.OnFormClosed(e);
        }

        private void UpdateFavicon()
        {
            // Execute some simple javascript that will search for a favicon.
            using (JSValue val = webControl.ExecuteJavascriptWithResult(JsFavicon))
            {
                // Check if we got a valid answer.
                if ((val != null) && (val.Type == JSValueType.String))
                {
                    // We do not need to perform the download of the favicon synchronously.
                    // May be a full icon set (thus big).
                    Task.Factory.StartNew<Bitmap>(GetFavicon, val.ToString()).ContinueWith(t =>
                        {
                            // If the download completed successfully, set the new favicon.
                            // This post-completion procedure is executed synchronously.
                            if ((t.Exception == null) && (t.Result != null))
                            {
                                Icon = Icon.FromHandle(t.Result.GetHicon());
                                DockPanel.Refresh();
                            }
                        },
                        TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private static Bitmap GetFavicon(Object href)
        {
            using (WebClient client = new WebClient())
            {
                Byte[] data = client.DownloadData(href.ToString());

                if ((data != null) && (data.Length > 0))
                    return new Bitmap(new MemoryStream(data));
            }

            return null;
        }

        private void QueryDownload(string url)
        {
            try
            {
                // Create a request for the headers only.
                WebRequest request = WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;

                // Asynchronously perform the request.
                request.BeginGetResponse(ar =>
                {
                    try
                    {
                        WebResponse response = request.EndGetResponse(ar);

                        if (response != null)
                        {
                            // Initialize.
                            string fileName = string.Empty;

                            // Get the "content-disposition" header.
                            // We currently care for the filename only. However, at this point we could
                            // also inform the user of the size of the file by getting the value of the:
                            // "content-length" header.
                            string contentDisposition = response.Headers["content-disposition"];

                            if (!String.IsNullOrEmpty(contentDisposition))
                            {
                                // Look for the filename part.
                                const string fileNamePart = "filename=";
                                int index = contentDisposition.IndexOf(fileNamePart, StringComparison.InvariantCultureIgnoreCase);

                                if (index >= 0)
                                {
                                    fileName = contentDisposition.Substring(index + fileNamePart.Length);
                                    // Cleanup any invalid surrounding characters.
                                    fileName = fileName.Trim(Path.GetInvalidFileNameChars()).Trim(new[] { '"', ';' });
                                }
                            }

                            // No filename suggested or the resource is actually a stream,
                            // served by default with no "Content-Disposition".
                            if (String.IsNullOrWhiteSpace(fileName))
                            {
                                // Attempt to get the filename from the URL's last segment.
                                if (response.ResponseUri.Segments.Length >= 0)
                                {
                                    int fileNamePos = response.ResponseUri.Segments.Length - 1;
                                    fileName = response.ResponseUri.Segments[fileNamePos];
                                }
                            }

                            // If we have a filename, proceed with asking the user
                            // and performing the actual download.
                            if (!String.IsNullOrWhiteSpace(fileName))
                                BeginInvoke((Action<string, string>)RequestDownload, url, fileName);
                        }
                    }
                    catch { }
                }, null);
            }
            catch { }
        }

        private void RequestDownload(string url, string fileName)
        {
            BrowserForm browserForm = BrowserForm;

            if (browserForm == null)
                return;

            using (SaveFileDialog dialog = new SaveFileDialog
                                                {
                                                    FileName = fileName,
                                                    // We set MyDocuments as the default. You can change this as you wish
                                                    // but make sure the specified folder actually exists.
                                                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                                })
            {
                if ((dialog.ShowDialog(browserForm) == DialogResult.OK) && (dialog.FileNames.Length == 1))
                    browserForm.DownloadFile(url, dialog.FileName);
            }
        }

        public BrowserForm BrowserForm { get; set; }

        public bool IsRendering
        {
            get { return webControl.Enabled; }
            set { webControl.Enabled = value; }
        }

        private void webControl_OpenExternalLink(object sender, OpenExternalLinkEventArgs e)
        {
            if (DockPanel != null)
            {
                WebDocument doc = new WebDocument(e.Url);
                doc.Show(DockPanel);
            }
        }

        private void webControl_DomReady(object sender, EventArgs e)
        {
            // DOM is ready. We can start looking for a favicon.
            UpdateFavicon();
        }

        private void webControl_TargetUrlChanged(object sender, UrlEventArgs e)
        {
            BrowserForm browserForm = BrowserForm;

            if (browserForm != null)
                browserForm.Status = e.Url;
        }

        private void webControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BrowserForm browserForm = BrowserForm;

            switch (e.PropertyName)
            {
                case "Title":
                    if (_fixedUrl)
                        break;

                    Text = webControl.Title;

                    if (browserForm != null)
                        browserForm.Title = Text;

                    break;
                case "IsLoadingPage":
                    if (browserForm != null)
                        browserForm.ShowProgress = webControl.IsLoadingPage;

                    break;

                default:
                    break;
            }
        }

        private void webControl_BeginLoading(object sender, BeginLoadingEventArgs e)
        {
            // By now we have already navigated to the address.
            // Clear the old favicon.
            if (Icon != null)
                Icon.Dispose();

            ComponentResourceManager resources = new ComponentResourceManager(typeof(WebDocument));
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            DockPanel.Refresh();
        }

        private void webControl_SelectLocalFiles(object sender, SelectLocalFilesEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog
                                                {
                                                    Title = e.Title,
                                                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                    CheckFileExists = true,
                                                    Multiselect = e.SelectMultipleFiles
                                                })
            {
                if ((dialog.ShowDialog(BrowserForm) == DialogResult.OK) && (dialog.FileNames.Length > 0))
                    e.SelectedFiles = dialog.FileNames;
                else
                    e.Cancel = true;
            }
        }

        private void webControl_Download(object sender, UrlEventArgs e)
        {
            // For the time being, the event provides the URL (that may contain a query) but not
            // the actual filename. We will query the server for the name of the file.
            // (Some browsers, like Chrome, immediately start the download while they figure out
            // the filename and then wait for the user to respond to the Save dialog. This is why
            // when you actually hit "Save", you notice that most of the file is already downloaded)
            QueryDownload(e.Url);
        }

        private void toolStripButton1_MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex += 1;
        }

        private void toolStripButton1_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex -= 1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!webControl.IsLive)
                return;

            webControl.GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!webControl.IsLive)
                return;

            webControl.GoForward();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!webControl.IsLive)
                return;

            webControl.Reload();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            webControl.GoToHome();
        }
    }
}
