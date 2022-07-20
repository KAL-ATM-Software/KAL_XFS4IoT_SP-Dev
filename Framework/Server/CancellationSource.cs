using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{

    /// <summary>
    /// Wrapper class around CancellationTokenSource to log when Cancel is requested
    /// </summary>
    public class CancellationSource : IDisposable
    {

        public CancellationSource(ILogger Logger)
        {
            this.Logger = Logger.IsNotNull();
            CTS = new CancellationTokenSource();
        }

        /// <summary>
        /// CancellationToken to use for any async operations
        /// </summary>
        public CancellationToken Token { get => CTS.Token; }

        /// <summary>
        /// Request to cancel any actions using this CancellationSource
        /// </summary>
        public void Cancel()
        {
            Logger.Log("CancellationSource", $"{nameof(Cancel)} called on {nameof(CancellationSource)}.");
            CTS.Cancel();
        }

        private readonly CancellationTokenSource CTS;
        private readonly ILogger Logger;

        public void Dispose()
        {
            CTS.Dispose();
        }
    }
}
