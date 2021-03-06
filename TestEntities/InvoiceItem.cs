	using System;
using System.Collections.Generic;
using System.Text;

namespace TestEntities
{
	public class InvoiceItem
	{
		public int InvoiceId { get; set; }
		public int InvoiceItemId { get; set; }
		public int ItemId { get; set; }
		public double Price { get; set; }
		public int Count { get; set; }

	}
}
