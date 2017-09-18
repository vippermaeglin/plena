using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using M4.Properties;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.RichTextBox;
using Telerik.WinControls.RichTextBox.FileFormats.Html;
using Telerik.WinControls.RichTextBox.FileFormats.OpenXml.Docx;
using Telerik.WinControls.RichTextBox.FileFormats.Pdf;
using Telerik.WinControls.RichTextBox.FileFormats.Rtf;
using Telerik.WinControls.RichTextBox.FileFormats.Xaml;
using Telerik.WinControls.RichTextBox.FormatProviders;
using Telerik.WinControls.RichTextBox.FormatProviders.Txt;
using Telerik.WinControls.RichTextBox.Layout;
using Telerik.WinControls.RichTextBox.Lists;
using Telerik.WinControls.RichTextBox.Model;
using Telerik.WinControls.RichTextBox.Model.Styles;
using Telerik.WinControls.RichTextBox.UI;
using Telerik.WinControls.UI;
//using TelerikEditor.Properties;

namespace TelerikEditor
{
	public partial class MainForm : RadRibbonForm
	{
		private bool stylesInitializing = false;
		private bool firstShow = true;
		private bool singleInstance;
		private System.Windows.Forms.Timer closeTimer;

		public MainForm()
		{
			Initialize();
		}

		public MainForm(bool singleInstance)
		{
			// TODO: Complete member initialization
			this.singleInstance = singleInstance;
			this.Opacity = 0;
			this.ShowInTaskbar = false;

			Initialize();
		}

		private void Initialize()
		{
			InitializeComponent();


			RadMenuInsertTableItem insertTableBoxItem = new RadMenuInsertTableItem();
			this.radDropDownButtonElementTables.Items.Insert(0, insertTableBoxItem);
			insertTableBoxItem.SelectionChanged += new EventHandler<TableSelectionChangedEventArgs>(OnInsertTableSelectionChanged);

			this.radMenuItemInsertTable.Click += new EventHandler(radMenuItemInsertTable_Click);
			ThemeResolutionService.ApplicationThemeName = "Office2010Blue";

			this.radRibbonBar1.RibbonBarElement.ApplicationButtonElement.Text = "File";
			this.radRibbonBar1.RibbonBarElement.ApplicationButtonElement.ForeColor = Color.White;
			this.radRibbonBar1.RibbonBarElement.ApplicationButtonElement.DisplayStyle = DisplayStyle.Text;

			this.radDropDownListFontSize.DropDownStyle = RadDropDownStyle.DropDownList;
			this.radDropDownListFont.DropDownStyle = RadDropDownStyle.DropDownList;

			this.radPageView1.SelectedPage = this.radPageViewPageSaveAsWord;

			DocxFormatProvider docxProvider = new DocxFormatProvider();
			RadDocument document = docxProvider.Import(Resources.RichTextBox_for_WinForms);
			this.AttachDocument(document);
			this.radRichTextBox1.Document = document;
			this.radBtnPrintLayout.ToggleState = ToggleState.On;
			this.radRichTextBox1.CurrentEditingStyleChanged += new EventHandler(radRichTextBox1_CurrentEditingStyleChanged);

			this.radPageViewPageSaveAsHtml.Item.Click += new EventHandler(this.radBtnSave_Click);
			this.radPageViewPageSaveAsPDF.Item.Click += new EventHandler(this.radBtnSave_Click);
			this.radPageViewPageSaveAsRTF.Item.Click += new EventHandler(this.radBtnSave_Click);
			this.radPageViewPageSaveAsText.Item.Click += new EventHandler(this.radBtnSave_Click);
			this.radPageViewPageSaveAsWord.Item.Click += new EventHandler(this.radBtnSave_Click);
			this.radPageViewPageSaveAsXAML.Item.Click += new EventHandler(this.radBtnSave_Click);

			this.radDropDownListFont.DropDownAnimationEnabled = false;
			this.radDropDownListFontSize.DropDownAnimationEnabled = false;

			AttachSignal();
			this.closeTimer = new System.Windows.Forms.Timer();
			this.closeTimer.Interval = 3000;
			this.closeTimer.Tick += new EventHandler(closeTimer_Tick);
			this.closeTimer.Start();
		}

        private void InitializeUI()
        {
            this.stylesInitializing = true;
            StyleDefinition styleDefinition = this.radRichTextBox1.CurrentEditingStyle;
            this.InitializeCurrentFontStyles(styleDefinition);
            this.InitializeCurrentParagraphStyles(styleDefinition);

            BaselineAlignment baselineAlignment = (BaselineAlignment)styleDefinition.GetPropertyValue(Span.BaselineAlignmentProperty);
            this.subscriptButtonElement.ToggleState = baselineAlignment == BaselineAlignment.Subscript ? ToggleState.On : ToggleState.Off;
            this.superscriptButtonElement.ToggleState = baselineAlignment == BaselineAlignment.Superscript ? ToggleState.On : ToggleState.Off;

            this.stylesInitializing = false;
        }

        private void InitializeCurrentFontStyles(StyleDefinition styleDefinition)
        {
            UnderlineType underlineType = (UnderlineType)styleDefinition.GetPropertyValue(Span.UnderlineTypeProperty);
            this.radBtnUnderlineStyle.ToggleState = underlineType != UnderlineType.None ? ToggleState.On : ToggleState.Off;

            string fontFamiliy = (string)styleDefinition.GetPropertyValue(Span.FontFamilyProperty);
            this.radDropDownListFont.SuspendSelectionEvents = true;
            this.radDropDownListFont.Text = fontFamiliy;
            this.radDropDownListFont.SelectedValue = fontFamiliy;
            this.radDropDownListFont.SuspendSelectionEvents = false;

            float fontSize = (float)styleDefinition.GetPropertyValue(Span.FontSizeProperty);
            fontSize = (float)Math.Round(Unit.DipToPoint(fontSize), 1);
            this.EnsureFontSize(fontSize.ToString());

            TextStyle fontStyle = (TextStyle)styleDefinition.GetPropertyValue(Span.FontStyleProperty);

            this.radBtnBoldStyle.ToggleState = fontStyle.HasFlag(TextStyle.Bold) ? ToggleState.On : ToggleState.Off;
            this.radBtnItalicStyle.ToggleState = fontStyle.HasFlag(TextStyle.Italic) ? ToggleState.On : ToggleState.Off;

            bool strikeThrough = (bool)styleDefinition.GetPropertyValue(Span.StrikethroughProperty);
            this.radBtnStrikethrough.ToggleState = strikeThrough ? ToggleState.On : ToggleState.Off;
        }

        private void InitializeCurrentParagraphStyles(StyleDefinition styleDefinition)
        {
            Paragraph paragraph = this.radRichTextBox1.Document.CaretPosition.GetCurrentParagraphBox().AssociatedParagraph;
            RadTextAlignment textAlignment = paragraph.TextAlignment;

            this.radBtnAligntLeft.ToggleState = textAlignment == RadTextAlignment.Left ? ToggleState.On : ToggleState.Off;
            this.radBtnAlignCenter.ToggleState = textAlignment == RadTextAlignment.Center ? ToggleState.On : ToggleState.Off;
            this.radBtnAligntRight.ToggleState = textAlignment == RadTextAlignment.Right ? ToggleState.On : ToggleState.Off;
            this.radBtnAlignJustify.ToggleState = textAlignment == RadTextAlignment.Justify ? ToggleState.On : ToggleState.Off;

            ListNumberingFormat? format = null;

            if (paragraph.IsInList)
            {
                format = paragraph.ListItemInfo.List.Style.Levels[paragraph.ListItemInfo.ListLevel].NumberingFormat;
            }

            this.radBtnBulletList.ToggleState = format == ListNumberingFormat.Bullet ?
                                                ToggleState.On : ToggleState.Off;

            this.radNumberingList.ToggleState = format == ListNumberingFormat.Decimal ?
                                                ToggleState.On : ToggleState.Off;            
        }

        private void AttachDocument(RadDocument document)
        {
            document.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Document_PropertyChanged);
        }

        private void DetachDocument(RadDocument document)
        {
            document.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Document_PropertyChanged);
        }
		
        private static void AttachSignal()
		{
			try
			{
				Mutex mutex = Mutex.OpenExisting("TelerikEditorExample");
				if (mutex != null)
				{
					Thread thread = new Thread(new ParameterizedThreadStart(ExitApp));
					thread.Start(mutex);
				}
			}
			catch { }
		}

		private static void ExitApp(object sync)
		{
			Mutex mutex = sync as Mutex;
			mutex.WaitOne();

			Application.Exit();
		}

		private void EnsureFontSize(string fontSize)
		{
			this.radDropDownListFontSize.SuspendSelectionEvents = true;

			foreach (RadListDataItem item in this.radDropDownListFontSize.Items)
			{
				if (string.Compare(item.Text, fontSize) == 0)
				{
					this.radDropDownListFontSize.SelectedIndex = item.RowIndex;
					this.radDropDownListFontSize.Text = item.Text;
					this.radDropDownListFontSize.SuspendSelectionEvents = false;
					return;
				}
			}

			this.radDropDownListFontSize.Text = fontSize;
			this.radDropDownListFontSize.SelectedIndex = -1;

			this.radDropDownListFontSize.SuspendSelectionEvents = false;
		}

        private void OpenDocument()
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Word Documents (*.docx)|*.docx|Web Pages (*.htm,*.html)|*.htm;*.html|Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|XAML Files (*.xaml)|*.xaml";

                if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string extension = Path.GetExtension(openDialog.SafeFileName).ToLower();

                    IDocumentFormatProvider provider = GetProviderByExtension(extension);

                    if (provider == null)
                    {
                        RadMessageBox.Show("Unable to find format provider for extension " + extension, "Error");
                        return;
                    }

                    using (Stream stream = openDialog.OpenFile())
                    {
                        RadDocument document = provider.Import(stream);
                        this.DetachDocument(this.radRichTextBox1.Document);
                        this.radRichTextBox1.Document = document;
                        this.AttachDocument(document);
                        document.LayoutMode = DocumentLayoutMode.Paged;
                    }
                }

                this.radRichTextBox1.Focus();
            }
        }
        
        private void SaveDocument(string format)
		{
			using (SaveFileDialog saveDialog = new SaveFileDialog())
			{
				if (format == "docx")
				{
					saveDialog.Filter = "Word Document (*.docx)|*.docx";
				}
				else if (format == "rtf")
				{
					saveDialog.Filter = "Rich Text Format (*.rtf)|*.rtf";
				}
				else if (format == "html")
				{
					saveDialog.Filter = "Web Page (*.html)|*.html";
				}
				else if (format == "xaml")
				{
					saveDialog.Filter = "XAML File (*.xaml)|*.xaml";
				}
				else if (format == "txt")
				{
					saveDialog.Filter = "Text File (*.txt)|*.txt";
				}
				else if (format == "pdf")
				{
					saveDialog.Filter = "PDF File (*.pdf)|*.pdf";
				}
				else
				{
					saveDialog.Filter = "Word Document (*.docx)|*.docx|PDF File (*.pdf)|*.pdf|Web Page (*.html)|*.html|Rich Text Format (*.rtf)|*.rtf|Text File (*.txt)|*.txt|XAML File (*.xaml)|*.xaml";
				}

				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					string extension = System.IO.Path.GetExtension(saveDialog.FileName);

					IDocumentFormatProvider provider = GetProviderByExtension(extension);

					if (provider == null)
					{
						RadMessageBox.Show("Unable to find format provider for extension " + extension, "Error");
						return;
					}

					using (Stream output = saveDialog.OpenFile())
					{
						provider.Export(this.radRichTextBox1.Document, output);
					}
				}

				this.radRichTextBox1.Focus();
			}
		}

		private IDocumentFormatProvider GetProviderByExtension(string extension)
		{
			if (extension == ".xaml")
			{
				return new XamlFormatProvider();
			}

			if (extension == ".docx")
			{
				return new DocxFormatProvider();
			}

			if (extension == ".rtf")
			{
				return new RtfFormatProvider();
			}

			if (extension == ".html" || extension == ".htm")
			{
				return new HtmlFormatProvider();
			}

			if (extension == ".txt")
			{
				return new TxtFormatProvider();
			}

			if (extension == ".pdf")
			{
				return new PdfFormatProvider();
			}

			return null;
		}

        #region Event handlers

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            InstalledFontCollection font = new InstalledFontCollection();

            List<string> families = new List<string>();

            foreach (FontFamily familiy in font.Families)
            {
                if (familiy.IsStyleAvailable(FontStyle.Regular) &&
                    familiy.IsStyleAvailable(FontStyle.Italic) &&
                    familiy.IsStyleAvailable(FontStyle.Bold))
                {
                    families.Add(familiy.Name);
                }
            }

            this.radDropDownListFont.DataSource = families;

            this.InitializeUI();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (this.singleInstance)
            {
                if (firstShow)
                {
                    this.Visible = false;
                    this.firstShow = false;
                    this.Opacity = 1;
                }
                else
                {
                    this.ShowInTaskbar = true;
                }
            }
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            if (singleInstance)
            {
                e.Cancel = true;
                this.Hide();
            }

            base.OnClosing(e);
        }

        private void OnInsertTableSelectionChanged(object sender, TableSelectionChangedEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.radRichTextBox1.InsertTable(e.RowIndex + 1, e.ColumnIndex + 1);
            }
        }

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            Process[] process = Process.GetProcessesByName("TelerikExamples");
            if (process.Length == 0)
            {
                this.closeTimer.Stop();
                this.closeTimer.Dispose();

                if (this.Visible)
                {
                    this.singleInstance = false;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void radRichTextBox1_CurrentEditingStyleChanged(object sender, EventArgs e)
        {
            this.InitializeUI();
        }

        private void Document_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RadDocument document = sender as RadDocument;
            this.radBtnPrintLayout.ToggleState = document.LayoutMode == DocumentLayoutMode.Paged ? ToggleState.On : ToggleState.Off;
            this.radBtnWebLayout.ToggleState = document.LayoutMode == DocumentLayoutMode.Flow ? ToggleState.On : ToggleState.Off;
        }

        private void radDropDownListFont_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            this.radRichTextBox1.ChangeFontFamily(Convert.ToString(this.radDropDownListFont.SelectedValue));
            this.radDropDownListFont.ClosePopup();
            this.radRichTextBox1.Focus();
        }

        private void radDropDownListFontSize_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.radDropDownListFontSize.SelectedItem != null)
            {
                string selectedText = this.radDropDownListFontSize.SelectedItem.Text;
                float fontSize = float.Parse(selectedText);
                fontSize = (float)Math.Round(Unit.PointToDip(fontSize), 1);
                this.radRichTextBox1.ChangeFontSize(fontSize);
                this.radDropDownListFontSize.ClosePopup();
                this.radRichTextBox1.Focus();
            }
        }
        
        private void radMenuItemOpenDoc_Click(object sender, EventArgs e)
		{
			this.radRibbonBar1.BackstageControl.HidePopup();
			this.OpenDocument();
		}

		private void radBtnSave_Click(object sender, EventArgs e)
		{
			string format = null;

			if (sender == this.radMenuItemSaveHtmlDocument ||
				sender == this.radPageViewPageSaveAsHtml.Item)
			{
				format = "html";
			}
			else if (sender == this.radMenuItemSavePDFDocument ||
					 sender == this.radPageViewPageSaveAsPDF.Item)
			{
				format = "pdf";
			}
			else if (sender == this.radMenuItemSavePlainText ||
					 sender == this.radPageViewPageSaveAsText.Item)
			{
				format = "txt";
			}
			else if (sender == this.radMenuItemSaveWordDoc ||
					 sender == this.radPageViewPageSaveAsWord.Item)
			{
				format = "docx";
			}
			else if (sender == this.radMenuItemSaveXAML ||
					 sender == this.radPageViewPageSaveAsXAML.Item)
			{
				format = "xaml";
			}
			else if (sender == this.radMenutItemSaveRTF ||
					 sender == this.radPageViewPageSaveAsRTF.Item)
			{
				format = "rtf";
			}

			this.radRibbonBar1.BackstageControl.HidePopup();
			this.SaveDocument(format);
		}

		private void radMenuItemNew_Click(object sender, EventArgs e)
		{
			if (RadMessageBox.Show("Do you want to save changes you made to Document1?", "Telerik Word",
									MessageBoxButtons.YesNo, RadMessageIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				this.SaveDocument(null);
			}

			this.DetachDocument(this.radRichTextBox1.Document);
			this.radRichTextBox1.Document = new RadDocument();
			this.AttachDocument(this.radRichTextBox1.Document);
			this.radRichTextBox1.LayoutMode = DocumentLayoutMode.Paged;
			this.radRibbonBar1.BackstageControl.HidePopup();
		}

		private void radMenuItemLandscape_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionPageOrientation(PageOrientation.Landscape);
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemPortrait_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionPageOrientation(PageOrientation.Portrait);
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemRotate180_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionPageOrientation(PageOrientation.Rotate180);
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemRotate270_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionPageOrientation(PageOrientation.Rotate270);
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemModerate_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionMargin(PageMarginTypesConverter.ToPadding(PageMarginTypes.Moderate));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemNarrow_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionMargin(PageMarginTypesConverter.ToPadding(PageMarginTypes.Narrow));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemNormal_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionMargin(PageMarginTypesConverter.ToPadding(PageMarginTypes.Normal));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemOffice2003_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangeSectionMargin(PageMarginTypesConverter.ToPadding(PageMarginTypes.Office2003));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemSizeA0_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangePageSize(PaperTypeConverter.ToSize(PaperTypes.A0));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemSizeA1_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangePageSize(PaperTypeConverter.ToSize(PaperTypes.A1));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemSizeA2_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangePageSize(PaperTypeConverter.ToSize(PaperTypes.A2));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemSizeA3_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangePageSize(PaperTypeConverter.ToSize(PaperTypes.A3));
			this.radRichTextBox1.Focus();
		}

		private void radMenuItemSizeA4_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.ChangePageSize(PaperTypeConverter.ToSize(PaperTypes.A4));
			this.radRichTextBox1.Focus();
		}

		private void backstageButtonItemExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void hyperlinkButtonElement_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.DocumentView.ShowInsertHyperlinkDialog();
		}

		private void bookmarkButtonElement_Click(object sender, EventArgs e)
		{
			this.radRichTextBox1.DocumentView.ShowManageBookmarksDialog();
		}

        private void radBtnBackColor_Click(object sender, System.EventArgs e)
        {
            RadColorDialog colorDialog = new RadColorDialog();

            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.radRichTextBox1.ChangeParagraphBackground(colorDialog.SelectedColor);
            }

            this.radRichTextBox1.Focus();
        }

        private void radBtnEnableSpellCheck_Click(object sender, System.EventArgs e)
        {
            bool enabled = this.radRichTextBox1.IsSpellCheckingEnabled;
            this.radRichTextBox1.IsSpellCheckingEnabled = !enabled;
            this.radRichTextBox1.Focus();
        }

        private void radBtnPageBreak_Click(object sender, EventArgs e)
        {
            this.radRichTextBox1.InsertPageBreak();
            this.radRichTextBox1.Focus();
        }

        private void radBtnInsertPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                if (openDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    string extension = Path.GetExtension(openDialog.SafeFileName);
                    this.radRichTextBox1.InsertImage(openDialog.OpenFile());
                }

                this.radRichTextBox1.Focus();
            }
        }

        private void radBtnSpellCheck_Click(object sender, EventArgs e)
        {
            this.radSpellChecker1.Check(this.radRichTextBox1);
            this.radRichTextBox1.Focus();
        }

        private void radBtnDecreaseIndent_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.DecrementParagraphLeftIndent();
            this.radRichTextBox1.Focus();
        }

        private void radBtnIncreaseIndent_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.IncrementParagraphLeftIndent();
            this.radRichTextBox1.Focus();
        }

        private void radBtnShowFormatting_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.ToggleFormattingSymbols();
            this.radRichTextBox1.Focus();
        }

        private void radBtnFontSizeIncrease_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.IncrementFontSize();
            this.radRichTextBox1.Focus();
        }

        private void radBtnFontSizeDecrease_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.DecrementFontSize();
            this.radRichTextBox1.Focus();
        }

        private void radBtnFormattingClear_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.ClearFormatting();
            this.radRichTextBox1.Focus();
        }

        private void radBtnUndo_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.Undo();
            this.radRichTextBox1.Focus();
        }

        private void radBtnRedo_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.Redo();
            this.radRichTextBox1.Focus();
        }

        private void radBtnCut_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.Cut();
            this.radRichTextBox1.Focus();
        }

        private void radBtnCopy_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.Copy();
            this.radRichTextBox1.Focus();
        }

        private void radButtonPaste_Click(object sender, System.EventArgs e)
        {
            this.radRichTextBox1.Paste();
            this.radRichTextBox1.Focus();
        }

        private void radBtnHighlight_Click(object sender, System.EventArgs e)
        {
            RadColorDialog colorDialog = new RadColorDialog();

            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.radRichTextBox1.ChangeTextHighlightColor(colorDialog.SelectedColor);
            }

            this.radRichTextBox1.Focus();
        }

        private void radBtnFontColor_Click(object sender, System.EventArgs e)
        {
            RadColorDialog colorDialog = new RadColorDialog();

            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.radRichTextBox1.ChangeTextForeColor(colorDialog.SelectedColor);
            }

            this.radRichTextBox1.Focus();
        }

        private void radMenuItemInsertTable_Click(object sender, EventArgs e)
        {
            using (InsertTableForm insertForm = new InsertTableForm())
            {
                insertForm.Owner = this;

                if (insertForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.radRichTextBox1.InsertTable(insertForm.RowsCount, insertForm.ColumnsCount);
                }
            }
        }

        private void radBtnBoldStyle_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleBold();
            this.radRichTextBox1.Focus();
        }

        private void radBtnUnderlineStyle_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleUnderline();
            this.radRichTextBox1.Focus();
        }

        private void radBtnItalicStyle_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleItalic();
            this.radRichTextBox1.Focus();
        }

        private void radBtnStrikethrough_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleStrikethrough();
            this.radRichTextBox1.Focus();
        }

        private void radBtnBulletList_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            ListStyle style = DefaultListStyles.None;
            if (args.ToggleState == ToggleState.On)
            {
                style = DefaultListStyles.Bulleted;
            }

            this.radRichTextBox1.ChangeListStyle(style);
            this.radRichTextBox1.Focus();
        }

        private void radNumberingList_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            ListStyle style = DefaultListStyles.None;
            if (args.ToggleState == ToggleState.On)
            {
                style = DefaultListStyles.Numbered;
            }

            this.radRichTextBox1.ChangeListStyle(style);
            this.radRichTextBox1.Focus();
        }

        private void radBtnAligntLeft_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            RadTextAlignment textAlignment = RadTextAlignment.Justify;
            if (args.ToggleState == ToggleState.On)
            {
                textAlignment = RadTextAlignment.Left;
            }

            this.radRichTextBox1.ChangeTextAlignment(textAlignment);
            this.radRichTextBox1.Focus();
        }

        private void radBtnAlignCenter_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            RadTextAlignment textAlignment = RadTextAlignment.Left;
            if (args.ToggleState == ToggleState.On)
            {
                textAlignment = RadTextAlignment.Center;
            }

            this.radRichTextBox1.ChangeTextAlignment(textAlignment);
            this.radRichTextBox1.Focus();
        }

        private void radBtnAligntRight_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            RadTextAlignment textAlignment = RadTextAlignment.Left;
            if (args.ToggleState == ToggleState.On)
            {
                textAlignment = RadTextAlignment.Right;
            }

            this.radRichTextBox1.ChangeTextAlignment(textAlignment);
            this.radRichTextBox1.Focus();
        }

        private void radBtnAlignJustify_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            RadTextAlignment textAlignment = RadTextAlignment.Left;
            if (args.ToggleState == ToggleState.On)
            {
                textAlignment = RadTextAlignment.Justify;
            }

            this.radRichTextBox1.ChangeTextAlignment(textAlignment);
            this.radRichTextBox1.Focus();
        }

        private void radBtnWebLayout_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            DocumentLayoutMode layout = DocumentLayoutMode.Paged;
            if (args.ToggleState == ToggleState.On)
            {
                layout = DocumentLayoutMode.Flow;
            }

            this.radRichTextBox1.LayoutMode = layout;
            this.radRichTextBox1.Focus();
        }

        private void radBtnPrintLayout_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            DocumentLayoutMode layout = DocumentLayoutMode.Flow;
            if (args.ToggleState == ToggleState.On)
            {
                layout = DocumentLayoutMode.Paged;
            }

            this.radRichTextBox1.LayoutMode = layout;
            this.radRichTextBox1.Focus();
        }

        private void subscriptButtonElement_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleSubscript();
            this.radRichTextBox1.Focus();
        }

        private void superscriptButtonElement_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (stylesInitializing)
            {
                return;
            }

            this.radRichTextBox1.ToggleSuperscript();
            this.radRichTextBox1.Focus();
        }

        #endregion
    }
}
