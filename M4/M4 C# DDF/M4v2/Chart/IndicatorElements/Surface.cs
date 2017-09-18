using Telerik.WinControls;

namespace M4.M4v2.Chart.IndicatorElements
{
    public class Surface : RadControl
    {
        public SurfaceElement Element;

        protected override void CreateChildItems(RadElement parent)
        {
            base.CreateChildItems(parent);
            Element = new SurfaceElement();
            parent.Children.Add(Element);
        }
    }
}
