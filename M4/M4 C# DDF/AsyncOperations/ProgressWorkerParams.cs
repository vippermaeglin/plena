/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

namespace M4.AsyncOperations
{
	using System.Drawing;
	using System.Windows.Forms;

	public class ProgressWorkerParams
	{
		/// <summary>
		/// Gets or sets whether the progress dialog is Modal or Modallless dialog
		/// </summary>
		public bool Modal { get; set; }

		/// <summary>
		/// Gets or sets whether user can cancel progress 
		/// </summary>
		public bool AllowCancel { get; set; }

		/// <summary>
		/// Gets or sets Alignment relative to <see cref="ControlAnchor"/>. This property
		/// is ignored if <see cref="Location"/> is set.
		/// </summary>
		public ContentAlignment Alignment { get; set; }

		/// <summary>
		/// Gets or sets the Control relative to which the Progress Dialog will be aligned
		/// </summary>
		public Control ControlAnchor { get; set; }

		/// <summary>
		/// Gets or sets the parent form for Progress Dialog
		/// </summary>
		public Form Parent { get; set; }

		/// <summary>
		/// Gets or sets the manual location of PorgressDialog, relative to <see cref="ContainerControl"/>.
		/// Is <see cref="ControlAnchor"/> is null position will be set relative to screen.
		/// </summary>
		public Point Location { get; set; }

		public ProgressWorkerParams()
		{
			//Set default values
			Modal = true;
			AllowCancel = true;
			Alignment = ContentAlignment.MiddleCenter;
			ControlAnchor = null;
			Parent = null;
			Location = Point.Empty;
		}
	}
}
