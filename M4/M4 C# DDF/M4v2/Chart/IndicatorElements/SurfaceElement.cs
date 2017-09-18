using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Drawing;

namespace M4.M4v2.Chart.IndicatorElements
{
    public class SurfaceElement : LightVisualElement
    {
        SurfaceObject currentObject;

        public event EventHandler CurrentObjectChanged;

        public SurfaceObject CurrentObject
        {
            get
            {
                return currentObject;
            }
            set
            {
                if (currentObject != value)
                {
                    currentObject = value;
                    Children.Remove(currentObject);
                    Children.Add(currentObject);
                    if (CurrentObjectChanged != null)
                    {
                        CurrentObjectChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected override void InitializeFields()
        {
            base.InitializeFields();
            DrawFill = true;
            DrawBorder = false;
            GradientStyle = GradientStyles.Solid;
            BackColor = Color.White;
        }

        protected override SizeF ArrangeOverride(SizeF finalSize)
        {
            foreach (SurfaceObject element in Children)
            {
                element.Arrange(new RectangleF(element.Offset.X, element.Offset.Y, element.DesiredSize.Width, element.DesiredSize.Height));
            }
            return finalSize;
        }
    }
}
