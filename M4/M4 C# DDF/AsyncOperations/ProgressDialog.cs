/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System.ComponentModel;
//using Nevron.UI.WinForm.Controls;

namespace M4
{
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;

	using M4.AsyncOperations;

	public partial class ProgressDialog : Form, IProgressVisualizer
	{
		private readonly AsyncOperation _asyncOp;

		private ProgressWorkerParams _params;

		public ProgressDialog()
		{
			_asyncOp = AsyncHelper.CreateOperation();

			InitializeComponent();
		}

		public void SetProgressTitle(string text)
		{
			_asyncOp.Post(() => lblTitle.Text = text);
		}

		public void SetProgressAction(string text)
		{
			_asyncOp.Post(() => lblAction.Text = text);
		}

		public void InitProgress(int min, int max)
		{
			_asyncOp.Post(
				() =>
					{
						if (min == max && min == -1)
						{
							pbBar.Style = ProgressBarStyle.Marquee;
						}
						else
						{
							pbBar.Minimum = min;
							pbBar.Maximum = max;
							pbBar.Style = ProgressBarStyle.Blocks;
							pbBar.Value = 0;
						}
					});
		}

		public void ReportProgress(int current)
		{
			_asyncOp.Post(() => pbBar.Value = current);
		}

		public void Complete()
		{
			_asyncOp.PostOperationCompleted(Dispose);
		}

		public bool CancelReqested
		{ 
			get; private set;
		}

		public IProgressVisualizer CreateVisualizer(ProgressWorkerParams workerParams)
		{
			_params = workerParams;
			btnCancel.Enabled = workerParams.AllowCancel;

			_asyncOp.Post(
				()=>
					{
						Location = GetLocation();
						if (!Visible)
						{
							if (!Modal)
							{
								Show();
							}
							else
							{
								ShowDialog();
							}
						}
					});
			return this;
		}

		private Point GetLocation()
		{
			Point p = Point.Empty;
			if (!_params.Location.IsEmpty)
			{
				p = _params.Location;
				if (_params.ControlAnchor != null)
				{
					p = _params.ControlAnchor.PointToScreen(p);
				}
			}
			else
			{
				Rectangle relativeRect;
				if (_params.ControlAnchor != null)
				{
					relativeRect = new Rectangle(0, 0, _params.ControlAnchor.Width, _params.ControlAnchor.Height);
					relativeRect = _params.ControlAnchor.RectangleToScreen(relativeRect);
				}
				else
				{
					relativeRect = Screen.PrimaryScreen.WorkingArea; 
				}

				switch (_params.Alignment)
				{
					case ContentAlignment.BottomCenter:
						p = new Point(relativeRect.Left + relativeRect.Width / 2 - Width / 2, relativeRect.Bottom - Height);
						break;							
					case ContentAlignment.BottomLeft:
						p = new Point(relativeRect.Left, relativeRect.Bottom - Height);
						break;
					case ContentAlignment.BottomRight:
						p = new Point(relativeRect.Right - Width, relativeRect.Bottom - Height);
						break;
					case ContentAlignment.MiddleCenter:
						p = new Point(relativeRect.Left + relativeRect.Width / 2 - Width / 2, relativeRect.Top + relativeRect.Height / 2 - Height / 2);
						break;
					case ContentAlignment.MiddleLeft:
						p = new Point(relativeRect.Left, relativeRect.Top + relativeRect.Height / 2 - Height / 2);
						break;
					case ContentAlignment.MiddleRight:
						p = new Point(relativeRect.Right - Width, relativeRect.Top + relativeRect.Height / 2 - Height / 2);
						break;
					case ContentAlignment.TopCenter:
						p = new Point(relativeRect.Left + relativeRect.Width / 2 - Width / 2, relativeRect.Top);
						break;
					case ContentAlignment.TopLeft:
						p = new Point(relativeRect.Left, relativeRect.Top);
						break;
					case ContentAlignment.TopRight:
						p = new Point(relativeRect.Left - relativeRect.Width, relativeRect.Top);
						break;
				}
			}

			Debug.Assert(!p.IsEmpty, "Position can't be empty.");

			return p;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			OnCancelRequested();
		}

		private void OnCancelRequested()
		{
			this.CancelReqested = true;
			this.btnCancel.Enabled = false;
		}

		private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing)
			{
				return;
			}

			if (!_params.AllowCancel)
			{
				e.Cancel = true;
				return;
			}

			OnCancelRequested();

			e.Cancel = true;
		}
	}
}
