// Copyright (c) Microsoft Corporation. All rights reserved. 

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SemanticLogging.Database.Xml.Utility
{
    internal static class DbConnectionExtensions
    {
        public static async Task SuppressTransactionOpenAsync(this DbConnection connection, CancellationToken token)
        {
            Task openTask;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                // Opt-out of using ambient transactions while opening the connection.
                // Disposing the transaction scope needs to happen in the same thread where it was created,
                // and that is why the await is done after the using finishes.
                openTask = connection.OpenAsync(token);
            }

            await openTask.ConfigureAwait(false);
        }
    }
}