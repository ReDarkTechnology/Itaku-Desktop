using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ItakuDesktop
{
	public class RoundedButton : Button
	{
		// Fields
		int borderSize;
		int borderRadius = 20;
		Color borderColor = Color.PaleVioletRed;
		
		// Properties
		[Category("Code Advance")]
		public int BorderSize
		{
		    get { return borderSize; }
		    set
		    {
		        borderSize = value;
		        Invalidate();
		    }
		}
        
		[Category("Code Advance")]
		public int BorderRadius
		{
		    get { return borderRadius; }
		    set
		    {        
		        borderRadius = value;       
		        Invalidate();
		    }
		}

		[Category("Code Advance")]
		public Color BorderColor
		{
		    get { return borderColor; }
		    set
		    {
		        borderColor = value;
		        Invalidate();
		    }
		}

		[Category("Code Advance")]
		public Color BackgroundColor
		{
		    get { return BackColor; }
		    set { BackColor = value; UpdateFlatStyle(); }
		}

		[Category("Code Advance")]
		public Color TextColor
		{
		    get { return ForeColor; }
		    set { ForeColor = value; }
		}
		
		[Category("Code Advance")]
		public Color OnHoverColor
		{
		    get { return FlatAppearance.MouseOverBackColor; }
		    set { FlatAppearance.MouseOverBackColor = value; }
		}
		
		[Category("Code Advance")]
		public Color OnDownColor
		{
		    get { return FlatAppearance.MouseDownBackColor; }
		    set { FlatAppearance.MouseDownBackColor = value; }
		}
		
		public RoundedButton()
		{
		    this.Size = new Size(150, 40);
		    this.Resize += Button_Resize;
		    UpdateFlatStyle();
		}
		
		public void UpdateFlatStyle()
		{
		    FlatStyle = FlatStyle.Flat;
		    FlatAppearance.BorderSize = 0;
		    FlatAppearance.MouseDownBackColor = BackColor.Add(45);
		    FlatAppearance.MouseOverBackColor = BackColor.Add(15);
        }
		
		void Button_Resize(object sender, EventArgs e)
		{
		    if (borderRadius > this.Height)
		        borderRadius = this.Height;
		}
		
		//Methods
		GraphicsPath GetFigurePath(Rectangle rect, float radius)
		{
		    var path = new GraphicsPath();
		    float curveSize = radius * 2F;
		    path.StartFigure();
		    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
		    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
		    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
		    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
		    path.CloseFigure();
		    return path;
		}
		
		protected override void OnPaint(PaintEventArgs pevent)
		{
		    base.OnPaint(pevent);
		    Rectangle rectSurface = this.ClientRectangle;
		    Rectangle rectBorder = Rectangle.Inflate(rectSurface,-borderSize, -borderSize);
		    int smoothSize = 2;
		    if (borderSize > 0)
		        smoothSize = borderSize;
		    if (borderRadius > 2) //Rounded button
		    {
		        using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
		        using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius-borderSize))
		        using (var penSurface = new Pen(Parent.BackColor, smoothSize))
		        using (var penBorder = new Pen(borderColor, borderSize))
		        {
		             pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
		            //Button surface
		            Region = new Region(pathSurface);
		            //Draw surface border for HD result
		            pevent.Graphics.DrawPath(penSurface, pathSurface);
		            //Button border                    
		            if (borderSize >= 1)
		                //Draw control border
		                pevent.Graphics.DrawPath(penBorder, pathBorder);
		        }
		    }
		    else //Normal button
		    {
		        pevent.Graphics.SmoothingMode = SmoothingMode.None;
		        //Button surface
		        Region = new Region(rectSurface);
		        //Button border
		        if (borderSize >= 1)
		        {
		            using (var penBorder = new Pen(borderColor, borderSize))
		            {
		                penBorder.Alignment = PenAlignment.Inset;
		                pevent.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
		            }
		        }
		    }
		}
		
		protected override void OnHandleCreated(EventArgs e)
		{
		    base.OnHandleCreated(e);
		    Parent.BackColorChanged += Container_BackColorChanged;
		}

		void Container_BackColorChanged(object sender, EventArgs e)
		{
		    Invalidate();
		}
	}
}
