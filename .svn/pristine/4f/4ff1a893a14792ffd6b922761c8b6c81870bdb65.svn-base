package tests.enums;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class MyClass {

	private List properties = null;

	public Iterator fct(final List myListZuper) {
		return new Iterator() {
			Iterator internal = properties.iterator();
			int count = (myListZuper.getClass() == null)?0:-1;
			
			public Object next() {
				throw new UnsupportedOperationException();
			}

			public void remove() {
				throw new UnsupportedOperationException();
			}

			public boolean hasNext() {
				return internal.hasNext();
			}
		};
	}

}
