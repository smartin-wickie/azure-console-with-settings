using System;
using System.Collections;
using System.Collections.Generic;

namespace TestEntities
{
	public class Invoice
	{
		public int InvoiceId { get; set; }
		public int AccountId { get; set; }
		public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
	}
}
