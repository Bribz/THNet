using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Managers
{
    public enum ConnectionApprovalCode
    {
        APPROVED = 0x0,
        BANNED = 0x1,
        KICKED = 0x2,

        NULL = 0xF
    }

    /// <summary>
    /// Connection Approval Manager. Approves connecitons to server.
    /// </summary>
    public class ConnectionApprovalManager
    {
        private Dictionary<ConnectionApprovalCode, string> ConnectionErrors;

        public ConnectionApprovalManager()
        {
            InitializeErrorCodes();
        }

        protected virtual void InitializeErrorCodes()
        {
            ConnectionErrors = new Dictionary<ConnectionApprovalCode, string>();
            ConnectionErrors.Add(ConnectionApprovalCode.APPROVED, "");
            ConnectionErrors.Add(ConnectionApprovalCode.BANNED, "IP banned from use.");
            ConnectionErrors.Add(ConnectionApprovalCode.KICKED, "Kicked from Server.");
            ConnectionErrors.Add(ConnectionApprovalCode.NULL, "Unknown Error!");
        }

        public virtual string GetDenyReason(ConnectionApprovalCode code)
        {
            return ConnectionErrors[code];
        }

        /// <summary>
        /// Check account against banned list, kicked list, or whatever limiters
        /// </summary>
        /// <returns>Is Connection Approved</returns>
        public virtual ConnectionApprovalCode ApproveConnection(string token, IPEndPoint address)
        {
            var strAddress = address.Address.ToString();

            return ApproveConnection(token, strAddress);
        }

        /// <summary>
        /// Check account against banned list, kicked list, or whatever limiters
        /// </summary>
        /// <returns>Is Connection Approved</returns>
        public virtual ConnectionApprovalCode ApproveConnection(string token, string address)
        {
            return ConnectionApprovalCode.APPROVED;
        }
    }
}
