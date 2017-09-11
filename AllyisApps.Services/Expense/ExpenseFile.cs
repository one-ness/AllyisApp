using System.IO;
using System.Web;

namespace AllyisApps.Services.Expense
{
	public class ExpenseFile : HttpPostedFileBase
	{
		private Stream stream;
		private string contentType;
		private string fileName;

		public ExpenseFile(Stream stream, string contentType, string fileName)
		{
			this.stream = stream;
			this.contentType = contentType;
			this.fileName = fileName;
		}

		public override int ContentLength
		{
			get
			{
				return stream == null ? 0 : (int)stream.Length;
			}
		}

		public override string ContentType
		{
			get { return contentType; }
		}

		public override string FileName
		{
			get { return fileName; }
		}

		public override Stream InputStream
		{
			get { return stream; }
		}

		public override void SaveAs(string filename)
		{
			using (var file = File.Open(filename, FileMode.CreateNew))
				stream.CopyTo(file);
		}
	}
}