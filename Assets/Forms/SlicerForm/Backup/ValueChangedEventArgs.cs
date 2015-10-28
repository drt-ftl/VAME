
using System;
using System.Windows.Forms;

namespace Sano.PersonalProjects.ColorPicker.Controls {

	public class ValueChangedEventArgs : EventArgs {

		private int m_value;

		public int Value {
			get { return m_value; }
		}

		public ValueChangedEventArgs( int newValue ) {
			m_value = newValue;
		}

	}

} // Sano.PersonalProjects.ColorPicker.Controls