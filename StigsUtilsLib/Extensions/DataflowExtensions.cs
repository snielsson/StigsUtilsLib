using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace StigsUtils.Extensions {
	public static class DataflowExtensions
	{
		public static void Post<T>(this ActionBlock<T> @this, IEnumerable<T> items) {
			foreach (var item in items) @this.Post(item);
		}
		public static async Task SendAsync<T>(this ActionBlock<T> @this, IEnumerable<T> items) {
			foreach (var item in items) await @this.SendAsync(item);
		}
	}
}