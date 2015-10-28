
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Sano.PersonalProjects.ColorPicker.Controls {

	public class ColorSlider : UserControl {
		
		public event ValueChangedHandler ValueChanged;

		// private data members
		private bool m_isLeftMouseButtonDown;
		private bool m_isLeftMouseButtonDownAndMoving;
		private int m_currentArrowYLocation;
		private Rectangle m_leftArrowRegion;
		private Rectangle m_rightArrowRegion;

		// readonly
		private readonly Rectangle m_colorRegion = new Rectangle( 13, 7, 18, 256 );
		private readonly Rectangle m_outerRegion = new Rectangle( 10, 4, 26, 264 );
		private readonly Bitmap m_colorBitmap = new Bitmap( 18, 256 );

		// constants
		private const int POINTER_HEIGHT = 10;
		private const int POINTER_WIDTH = 6;

		/// <summary>
		/// Constructor.
		/// </summary>
		
		public ColorSlider() : base() {			
			m_currentArrowYLocation = m_colorRegion.Top;

		}

		/// <summary>
		/// Gets or sets the color slider bitmap.
		/// </summary>

		public Bitmap ColorBitmap {
			get { return m_colorBitmap; }
		}

		protected override void OnPaint(PaintEventArgs e) {

			base.OnPaint (e);

			using ( Graphics g = e.Graphics ) {

				CreateLeftTrianglePointer( g, m_currentArrowYLocation );
				CreateRightTrianglePointer( g, m_currentArrowYLocation ); 

				if ( m_colorBitmap != null ) {
					g.DrawImage( m_colorBitmap, m_colorRegion );
				}

				ControlPaint.DrawBorder3D( g, m_outerRegion );
				g.DrawRectangle( Pens.Black, m_colorRegion );

			}

		}

		protected override void OnMouseDown(MouseEventArgs e) {
					
			if ( e.Button == MouseButtons.Left ) {
							
				m_isLeftMouseButtonDown = true;
				CheckCursorYRegion( e.Y );		
				
			}

			base.OnMouseDown (e);
				
		}

		protected override void OnMouseMove(MouseEventArgs e) {

			if ( m_isLeftMouseButtonDown ) {	

				m_isLeftMouseButtonDownAndMoving = true;
				CheckCursorYRegion( e.Y );			
			
			}
			
			base.OnMouseMove (e);

		}

		protected override void OnMouseUp(MouseEventArgs e) {

			m_isLeftMouseButtonDown = false;
			m_isLeftMouseButtonDownAndMoving = false;

			base.OnMouseUp (e);

		}

		/// <summary>
		/// Calculates the points needed to draw the left triangle pointer for
		/// the value strip.
		/// </summary>
		/// <param name="g">Graphics object.</param>
		/// <param name="y">Current cursor y-value.</param>

		private void CreateLeftTrianglePointer( Graphics g, int y ) {

			Point[] points = { 
								 new Point( m_outerRegion.Left - POINTER_WIDTH - 1, y - ( POINTER_HEIGHT / 2 ) ), 
								 new Point( m_outerRegion.Left - 2, y ), 
								 new Point( m_outerRegion.Left - POINTER_WIDTH - 1, y + ( POINTER_HEIGHT / 2 ) ) };
			
			g.DrawPolygon( Pens.Black, points );
		
		}

		/// <summary>
		/// Calculates the points needed to draw the right triangle pointer for
		/// the color slider.
		/// </summary>
		/// <param name="g">Graphics object.</param>
		/// <param name="y">Current cursor y-value.</param>
		
		private void CreateRightTrianglePointer( Graphics g, int y ) {

			Point[] points = { 
								 new Point( m_outerRegion.Right - 1 + POINTER_WIDTH, y - ( POINTER_HEIGHT / 2 ) ), 
								 new Point( m_outerRegion.Right, y ), 
								 new Point( m_outerRegion.Right - 1 + POINTER_WIDTH, y + ( POINTER_HEIGHT / 2 ) ) };
			
			g.DrawPolygon( Pens.Black, points );
			
		}
		
		/// <summary>
		/// Determines the color slider left triangle pointer invalidation 
		/// region.
		/// </summary>
		/// <param name="arrowY">Current cursor y-value.</param>
		/// <returns>A rectangle object representing the area to be 
		/// invalidated.</returns>

		private Rectangle GetLeftTrianglePointerInvalidationRegion( int arrowY ) {

			int leftPadding = POINTER_WIDTH + 2;
			int x = m_outerRegion.Left - leftPadding;
			int y = arrowY - ( POINTER_HEIGHT / 2 ) - 1;
			int width = POINTER_WIDTH + 2;
			int height = POINTER_HEIGHT + 3;
			
			return new Rectangle( x, y, width, height );

		}
		
		/// <summary>
		/// Determines the color slider right triangle pointer invalidation 
		/// region.
		/// </summary>
		/// <param name="arrowY">Current cursor y-value</param>
		/// <returns>A rectangle object representing the area to be 
		/// invalidated.</returns>
		
		private Rectangle GetRightTrianglePointerInvalidationRegion( int arrowY ) {

			int x = m_outerRegion.Right;
			int y = arrowY - ( POINTER_HEIGHT / 2 ) - 1;
			int width = POINTER_WIDTH + 2;
			int height = POINTER_HEIGHT + 3;
			
			return new Rectangle( x, y, width, height );

		}

		private void CheckCursorYRegion( int y ) {

			int mValue = y;

			if ( m_isLeftMouseButtonDown && !m_isLeftMouseButtonDownAndMoving ) {

				if ( y < m_colorRegion.Top || y > m_colorRegion.Bottom ) {
					return;
				}

			} else if ( m_isLeftMouseButtonDownAndMoving ) {

				if ( y < m_colorRegion.Top ) {
					mValue = m_colorRegion.Top;
				} else if ( y >= m_colorRegion.Bottom ) {
					mValue = m_colorRegion.Bottom - 1;
				} 

			} else {
				return;
			}
								 
			m_currentArrowYLocation = mValue;

			InvalidateArrowRegions( mValue );

			if ( ValueChanged != null ) {
				ValueChanged( this, new ValueChangedEventArgs( 255 - ( mValue - m_colorRegion.Top ) ) );
			}	


		}

		private void InvalidateArrowRegions( int y ) {

			this.Invalidate( m_leftArrowRegion );
			this.Invalidate( m_rightArrowRegion );
				
			m_leftArrowRegion = this.GetLeftTrianglePointerInvalidationRegion( y );
			m_rightArrowRegion = this.GetRightTrianglePointerInvalidationRegion( y );

			this.Invalidate( m_leftArrowRegion );
			this.Invalidate( m_rightArrowRegion );

		}

	} // ColorSlider

} // Sano.PersonalProjects.ColorPicker.Controls